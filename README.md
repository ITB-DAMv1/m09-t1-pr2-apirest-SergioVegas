# Esquema de l’estructura de cada projecte.
🗂 API_GAMES (Projecte de la API)

📂 Model/ → Definició d’entitats (Game, ApplicationUser).

📂 Data/ → ApplicationDbContext.cs per gestionar la base de dades.

📂 Controllers/ → Controladors per a jocs i autenticació (GamesController, AuthController).

📂 Seed/ → UserSeeder.cs i GameSeeder.cs per inicialitzar dades.

📂 Tools/ → RoleTools.cs per gestionar rols d’usuaris.

📂 Hubs/ → Xat.cs per la implementació de SignalR.

🗂 Client (Web o Razor Pages)

📂 Services/ → GameFetcher.cs per consumir la API.

📂 Pages/ → Games.cshtml, Profile.cshtml per mostrar jocs i info d’usuaris.

📂 Models/ → Game.cs per representar els jocs al client.
