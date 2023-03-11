
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesAPI.Models
{
    public class Genere //Type / Category of Movies
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        [MaxLength(length:100)]
        public string Name { get; set; }
    }
}
