using SQLite;

namespace SinglesApp.Core.Models
{
    public class JukeboxSelectionSingles
    {
        [PrimaryKey]
        public int SelectionId { get; set; }

        [PrimaryKey]
        public int SingleId { get; set; }
    }
}
