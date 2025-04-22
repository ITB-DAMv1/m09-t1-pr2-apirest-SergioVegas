using API_GAMES.Data;
using API_GAMES.Model;
using System.Linq;

public static class DbInitializer
{
    public static void Seed(ApplicationDbContext context)
    {
        if (!context.Games.Any()) // Verifica si la base de datos está vacía
        {
            var games = new List<Game>
            {
               new Game { Title = "Hollow Knight", Description = "El modo de juego de Hollow Knight se enfoca principalmente en exploración, plataformas y combate.", Developer = "Team Cherry", Image= "https://www.nintendo.com/eu/media/images/10_share_images/games_15/wiiu_download_software_5/H2x1_WiiUDS_HollowKnight_image1600w.jpg" },
                new Game { Title = "The Binding of isaac", Description = "The Binding of Isaac (o simplemente Isaac) es un videojuego independiente de rol de acción y de mazmorras (dungeon crawler) roguelikehecho en Adobe Flash,", Developer = "Edmund McMillen", Image ="https://static.wikia.nocookie.net/bindingofisaac/images/8/8e/BoI_IconMain.jpg/revision/latest?cb=20130824193220&path-prefix=es" },
                new Game { Title = "Pokemon Emerald", Description = "Es la reedición que cierra la tercera generación iniciada por Pokémon Rubí y Zafiro.En ella nos adentrábamos en la 3ª generación, en la cual 135 nuevos Pokémon nos acechaban.", Developer = "GAME FREAK",Image = "https://www.pokemon.com/static-assets/content-assets/cms-es-es/img/video-games/emerald/emerald_boxart_0003_spanish.png" },
                new Game { Title = "Pokemon Platinum", Description = "Es la reedición del dúo Pokémon Diamante y Pokémon Perla. Pocos días después de su lanzamiento en Japón se vendieron alrededor de un millón de copias, convirtiéndose en el juego más rápidamente vendido de la historia para Nintendo DS1. El 22 de Marzo de 2009 salió a la venta en Estados Unidos y en España salió el 22 de mayo del mismo año.", Developer = "GAME FREAK", Image = "https://www.pokemon.com/static-assets/content-assets/cms-es-es/img/video-games/platinum/platinum_boxart_0003_spanish.png"},
                new Game { Title = "R.E.P.O", Description = "R.E.P.O. is an online co-op horror game featuring physics, proximity voice chat and scary monsters. You and up to 5 friends can venture into terrifying environments to extract valuable objects using your physics-based grabbing tool.\r\n\r\n", Developer = "semiwork", Image ="https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/3241660/2cff5912c1add2e009eb1c1c630a47ac06cb81a1/capsule_616x353.jpg?t=1743517226" }
            };

            context.Games.AddRange(games);
            context.SaveChanges();
        }
        
    }
}
