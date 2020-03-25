using SQLite;

namespace SinglesApp.Core.Models
{
    public class SingleGenres
    {
        [PrimaryKey]
        public int SingleId { get; set; }

        [PrimaryKey]
        public int GenreId { get; set; }
    }
}
