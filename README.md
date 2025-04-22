# Descripció del Projecte

Aquest projecte consisteix en una aplicació web completa desenvolupat en .NET, que inclou una API REST i un client que utilitza aquesta API. El projecte està dissenyat per gestionar un catàleg de videojocs, permetre als usuaris registrar-se, iniciar sessió, votar els jocs i comunicar-se mitjançant un xat.

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
