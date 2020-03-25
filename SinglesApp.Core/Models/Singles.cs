using SQLite;

namespace SinglesApp.Core.Models
{
    public class Singles
    {
        [PrimaryKey, AutoIncrement]
        public int SingleId { get; set; }
        public int ArtistId { get; set; }
        public string ASideTitle { get; set; }
        public string BSideTitle { get; set; }
        public bool PictureCover { get; set; }
        public int? SingleYear { get; set; }
        public bool Dinked { get; set; }
        public bool JukeboxCardPrinted { get; set; }
        public bool SingleLabelPrinted { get; set; }
    }
}
