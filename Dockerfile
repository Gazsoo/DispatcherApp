FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# STAGE 2: Build the React Frontend
FROM node:24 AS client-build
WORKDIR /app
# Copy package.json and package-lock.json
COPY src/frontend-app/package*.json ./
RUN npm install
# Copy the rest of the frontend code
COPY src/frontend-app/ .
# Build the React app (output usually goes to /build or /dist)
RUN npm run build

# STAGE 3: Build the ASP.NET Backend
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS backend-build
WORKDIR /app
# Copy the csproj and restore dependencies
COPY DispatcherApp.sln ./
COPY src/DispatcherApp.API/*.csproj ./src/DispatcherApp.API/
COPY src/DispatcherApp.BLL/*.csproj ./src/DispatcherApp.BLL/
COPY src/DispatcherApp.Common/*.csproj ./src/DispatcherApp.Common/
COPY src/DispatcherApp.DAL/*.csproj ./src/DispatcherApp.DAL/
COPY test/DispatcherApp.Tests/*.csproj ./test/DispatcherApp.Tests/
COPY src/frontend-app/*.esproj ./src/frontend-app/
COPY docker-compose.dcproj ./
RUN dotnet restore "DispatcherApp.sln"
# Copy the rest of the backend code
COPY . .
WORKDIR /app/src/DispatcherApp.API
RUN dotnet publish "DispatcherApp.API.csproj" -c Release -o /app/publish

# STAGE 4: Final Runtime Image
FROM base AS final
WORKDIR /app
EXPOSE 19955
EXPOSE 5207

# Copy the compiled .NET app
COPY --from=backend-build /app/publish .

# Copy the compiled React app into the wwwroot folder of the .NET app
# Note: Check if your React build outputs to 'build' or 'dist'
COPY --from=client-build /app/dist ./wwwroot

ENTRYPOINT ["dotnet", "DispatcherApp.API.dll"]