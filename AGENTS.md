# Repository Guidelines

## Project Structure & Module Organization

`DispatcherApp.sln` wires the .NET backend in `src/`: `DispatcherApp.API` (entry point), `DispatcherApp.BLL` (business rules), `DispatcherApp.DAL` (data access), and `DispatcherApp.Models` (shared contracts). The React client sits in `src/frontend-app` with build output in `dist`. Tests reside in `test/DispatcherApp.Tests`, and shared docs live in `docs/`. Generated binaries drop into `artifacts/`; keep config secrets out of version control.

## Build, Test, and Development Commands

Restore once per clone with `dotnet restore DispatcherApp.sln`, then compile via `dotnet build DispatcherApp.sln -c Debug`. Start the API using `dotnet run --project src/DispatcherApp.API/DispatcherApp.API.csproj` (Swagger at `/swagger`). In `src/frontend-app`, run `npm install`, `npm run dev -- --open`, and `npm run build` for production bundles. Validate before PRs with `dotnet test test/DispatcherApp.Tests/DispatcherApp.Tests.csproj --collect:"XPlat Code Coverage"` and `npm run lint`.

## Coding Style & Naming Conventions

`.editorconfig` enforces spaces, LF endings, and four-space indents for C#. Treat warnings as errors per `Directory.Build.props`. Use explicit types (avoid `var`), PascalCase for types and methods, and `_camelCase` for private fields (`s_` for static). Keep namespaces aligned with folder paths. Frontend files follow two-space indents and ESLint rules; name React components with PascalCase files such as `DispatchBoard.tsx`.

## Testing Guidelines

Backend tests use xUnit in `test/DispatcherApp.Tests`. Name classes `<Subject>Tests` and choose descriptive `[Fact]`/`[Theory]` methods like `Creates_dispatch_for_valid_payload`. Collect coverage locally with `dotnet test ... --collect:"XPlat Code Coverage"`; coverage artifacts land in `artifacts/TestResults`. Automated UI tests are pending—document manual checks in your PR until they exist.

## Commit & Pull Request Guidelines

Keep commits consise, and context-rich; recent examples show sentence-case verbs. Limit the subject to one change, add detail in the body when necessary, and link issues with `Refs #123`. Pull requests should outline motivation, list executed commands (`dotnet test`, `npm run lint`), and attach screenshots for UI work. Flag migrations or contract changes prominently and point reviewers to updated docs.
