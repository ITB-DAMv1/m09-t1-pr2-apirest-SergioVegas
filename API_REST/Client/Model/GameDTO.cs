namespace Client.Model
{
    public class GameDTO
    {
         public int Id { get; set; }
        public string? Title { get; set; } 
        public string? Description { get; set; } 
        public string? Developer { get; set; }
        public int VoteCount { get; set; }
    }
}
