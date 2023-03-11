using MoviesAPI.Models;

namespace MoviesAPI.DTOs
{
    public class MovieDetailsDTO
    {
        public int Id { get; set; }
    
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }

        public string StoryLine { get; set; }
        public byte[] Poster { get; set; }
        public byte GenereId { get; set; }
        public string GenereName { get; set; }
    }
}
