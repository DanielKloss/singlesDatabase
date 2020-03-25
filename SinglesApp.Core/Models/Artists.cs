using SQLite;

namespace SinglesApp.Core.Models
{
    public class Artists
    {
        [PrimaryKey, AutoIncrement]
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
    }
}
