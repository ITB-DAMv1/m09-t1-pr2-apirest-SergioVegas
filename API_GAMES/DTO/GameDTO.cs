using System.ComponentModel.DataAnnotations;

namespace API_REST.DTO
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Developer { get; set; } = string.Empty;
        public string? Image { get; set; }
        public int VoteCount { get; set; }
    }
}
