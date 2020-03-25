using SQLite;

namespace SinglesApp.Core.Models
{
    public class Genres
    {
        [PrimaryKey, AutoIncrement]
        public int GenreId { get; set; }
        public string GenreName { get; set; }
    }
}
