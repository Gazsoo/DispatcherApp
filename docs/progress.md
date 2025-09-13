# Docs
## day one:
- Egyedül dolgozom a projekt folyamán a feladaton de, olyan struktúra amin ha többen, később is dolgoznának akkor is könnyen szeparáltan lehet dolgozni (testek) (kényelmes ejlesztési környezet).
- Architectúra választása, clean architecture, lehet túl sok erre a projektre és nagyon lelassítaná a projektet, dependency inversion mindenhol és interfaces.
- A projekt felépítése, N-layer architektúra, DAL az adataok elérése, definiálása, Model a közös adatmodellek és entitésoknak, BLL az üzleti logika leírására és API a prezentációs rétegként. Ezek a forráskód részben és külön egy test könyvtárban a különböző testek, kezdetben az egységtestek.
- Web api template-el kezdés dotnet new webapi -n DispatcherApp.API
- dotnet new sln -n DispatcherApp \
dotnet new classlib -n DispatcherApp.Models \
dotnet sln add DispatcherApp.API/DispatcherApp.API.csproj

- jasontaylordev Jason Taylor  Clean Architecture Solution Template for ASP.NET Core  https://github.com/jasontaylordev/CleanArchitecture?tab=readme-ov-file