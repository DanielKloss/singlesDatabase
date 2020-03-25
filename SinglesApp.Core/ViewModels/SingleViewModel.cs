using MvvmCross.Core.ViewModels;

namespace SinglesApp.Core.ViewModels
{
    public class SingleViewModel : MvxViewModel
    {
        public int singleId { get; set; }

        private string _artistName;
        public string artistName
        {
            get => _artistName;
            set
            {
                _artistName = value;
                RaisePropertyChanged(() => artistName);
            }
        }

        private string _aSideTitle;
        public string aSideTitle
        {
            get => _aSideTitle;
            set
            {
                _aSideTitle = value;
                RaisePropertyChanged(() => aSideTitle);
            }
        }

        private string _bSideTitle;
        public string bSideTitle
        {
            get => _bSideTitle;
            set
            {
                _bSideTitle = value;
                RaisePropertyChanged(() => bSideTitle);
            }
        }

        private int? _singleYear;
        public int? singleYear
        {
            get => _singleYear;
            set
            {
                _singleYear = value;
                RaisePropertyChanged(() => singleYear);
            }
        }

        private string _genreName;
        public string genreName
        {
            get => _genreName;
            set
            {
                _genreName = value;
                RaisePropertyChanged(() => genreName);
            }
        }

        private int _numberOfSelections;
        public int numberOfSelections
        {
            get => _numberOfSelections;
            set
            {
                _numberOfSelections = value;
                RaisePropertyChanged(() => numberOfSelections);
            }
        }

        private bool _pictureCover;
        public bool pictureCover
        {
            get => _pictureCover;
            set
            {
                _pictureCover = value;
                RaisePropertyChanged(() => pictureCover);
            }
        }

        private bool _dinked;
        public bool dinked
        {
            get => _dinked;
            set
            {
                _dinked = value;
                RaisePropertyChanged(() => dinked);
            }
        }

        private bool _jukeboxCardPrinted;
        public bool jukeboxCardPrinted
        {
            get => _jukeboxCardPrinted;
            set
            {
                _jukeboxCardPrinted = value;
                RaisePropertyChanged(() => jukeboxCardPrinted);
            }
        }

        private bool _singleLabelPrinted;
        public bool singleLabelPrinted
        {
            get => _singleLabelPrinted;
            set
            {
                _singleLabelPrinted = value;
                RaisePropertyChanged(() => singleLabelPrinted);
            }
        }
    }
}
