# Descripció del Projecte

Aquest projecte consisteix en una aplicació web completa desenvolupat en .NET, que inclou una API REST i un client que utilitza aquesta API. El projecte està dissenyat per gestionar un catàleg de videojocs, permetre als usuaris registrar-se, iniciar sessió, votar els jocs i comunicar-se mitjançant un xat.

## 🏗️ Arquitectura General

*   **Model:** Client-Servidor desacoblat.
*   **Components:**
    *   `🚀 API_GAMES`: Backend **ASP.NET Core** (API RESTful). Gestiona dades, lògica i autenticació.
    *   `🖥️ Client`: Frontend (probablement **ASP.NET Core Razor Pages/MVC**). Consumeix l'API i presenta la UI.
*   **Benefici Principal:** Separació de responsabilitats (SoC), permetent desenvolupament, escalat i manteniment independents.

## 🚀 Backend (`API_GAMES`) - Punts Clau

*   **Tecnologia:** ASP.NET Core.
*   **Accés a Dades:** **Entity Framework Core** (ORM) per interactuar amb la BD (`ApplicationDbContext`). Migracions gestionades (`__EFMigrationsHistory`).
*   **Autenticació/Autorització:** **ASP.NET Identity** (Taules `AspNetUsers`, `AspNetRoles`, etc.). Gestionat via `AuthController`.
*   **API:** Controladors (`GamesController`) exposen endpoints RESTful per als recursos.
*   **Temps Real:** **SignalR** (`Xat.cs`) per a funcionalitats en viu (xat).
*   **Organització:** Models (`Game`), Seeders (dades inicials), Eines (`RoleTools`).

## 🖥️ Frontend (`Client`) - Punts Clau

*   **Tecnologia:** Probablement ASP.NET Core (Razor Pages/MVC).
*   **Consum API:** Serveis (`GameFetcher.cs`) encapsulen les crides HTTP a `API_GAMES` (usant `HttpClient`).
*   **Interfície:** Pàgines/Vistes (`.cshtml`) defineixen la UI (`Games.cshtml`, `Profile.cshtml`).
*   **Models UI:** Models/ViewModels (`Game.cs`) adaptats per a les vistes.

## ✅ Justificacions Essencials

*   **Separació Frontend/Backend:** Flexibilitat, escalabilitat, mantenibilitat.
*   **Stack .NET Core:** Rendiment, multiplataforma, ecosistema integrat (EF Core, Identity, SignalR).
*   **EF Core:** Simplifica accés a BD i migracions.
*   **ASP.NET Identity:** Solució estàndard i robusta per a seguretat d'usuaris.
*   **SignalR:** Solució eficient per a comunicació en temps real.
# Esquema de l’estructura de cada projecte.

## 🗂 API_GAMES (Projecte de la API)

📂 Model/ → Definició d’entitats (Game, ApplicationUser).

📂 Data/ → ApplicationDbContext.cs per gestionar la base de dades.

📂 Controllers/ → Controladors per a jocs i autenticació (GamesController, AuthController).

📂 Seed/ → UserSeeder.cs i GameSeeder.cs per inicialitzar dades.

📂 Tools/ → RoleTools.cs per gestionar rols d’usuaris.

📂 Hubs/ → Xat.cs per la implementació de SignalR.

## 🗂 Client (Web o Razor Pages)

📂 Services/ → GameFetcher.cs per consumir la API.

📂 Pages/ → Games.cshtml, Profile.cshtml per mostrar jocs i info d’usuaris.

📂 Models/ → Game.cs per representar els jocs al client.

# Diagrama de BBDD final

![image](https://github.com/user-attachments/assets/7757bdf1-7a86-41e5-a12a-c5af566742d3)

1. Nucli d'Identitat (ASP.NET Identity): La majoria de les taules (AspNetUsers, AspNetRoles, AspNetUserRoles, AspNetUserClaims, AspNetRoleClaims, AspNetUserLogins, AspNetUserTokens) formen part de l'esquema estàndard d'ASP.NET Identity. Aquest sistema s'encarrega de:
- Gestionar usuaris (AspNetUsers): Emmagatzema la informació principal dels usuaris (nom, email, hash de contrasenya, etc.).
- Gestionar rols (AspNetRoles): Defineix els diferents rols dins l'aplicació (ex: Administrador, Usuari).
- Relacionar usuaris i rols (AspNetUserRoles): Taula intermèdia (molts-a-molts) que assigna rols als usuaris.
- Gestionar claims (AspNetUserClaims, AspNetRoleClaims): Permet associar informació addicional (claims) a usuaris o rols específics.
- Gestionar logins externs (AspNetUserLogins): Emmagatzema informació si els usuaris inicien sessió amb proveïdors externs (Google, Facebook, etc.).
- Gestionar tokens (AspNetUserTokens): Guarda tokens per a diverses funcionalitats (confirmació d'email, restabliment de contrasenya, etc.).
2. Domini de l'Aplicació (Jocs):
- Games: Aquesta taula conté informació específica de l'aplicació, en aquest cas, sobre jocs (Títol, Descripció, Desenvolupador, Imatge).
- ApplicationUserGame: És una taula intermèdia (junction table) que estableix una relació molts-a-molts entre els usuaris (AspNetUsers) i els jocs (Games). Això significa que un usuari pot estar associat a múltiples jocs, i un joc pot estar associat a múltiples usuaris. La finalitat exacta d'aquesta relació depèn de la lògica de l'aplicació (podria representar jocs que un usuari té, jocs preferits, jocs revisats, etc.). Les claus GamesId i UsersId són les claus foranes que apunten a les taules Games i AspNetUsers respectivament.
3. Metadades de Migracions (Entity Framework):
__EFMigrationsHistory: Aquesta taula la fa servir Entity Framework Core per portar un registre de quines migracions de base de dades s'han aplicat, assegurant que l'esquema de la base de dades estigui sincronitzat amb el model de dades del codi.

# Bibliografia
- https://jonathanbucaro.com/blog/almacenar-imagenes-en-sql-server/ [01/04/2025][Autor anónim]
- https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many[15/04/2025][Autor anónim]
- https://learn.microsoft.com/es-es/aspnet/core/security/authorization/simple?view=aspnetcore-9.0 [16/04/2025][Autor anónim]
- https://www.youtube.com/watch?v=Z6moywGIwtY [22/04/2025] [Anthony Code]
- https://learn.microsoft.com/es-es/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-9.0 [21/04/2025] [Autor anónim]
