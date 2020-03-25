using SQLite;

namespace SinglesApp.Core.Models
{
    public class JukeboxSelections
    {
        [PrimaryKey, AutoIncrement]
        public int selectionId { get; set; }
        public string selectionName { get; set; }
    }
}
