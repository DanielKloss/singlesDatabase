using MvvmCross.Core.ViewModels;

namespace SinglesApp.Core.ViewModels
{
    public class FilterViewModel : MvxViewModel
    {
        private string _searchTerm;
        public string searchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                RaisePropertyChanged(() => searchTerm);
            }
        }

        private int? _yearFrom;
        public int? yearFrom
        {
            get => _yearFrom;
            set
            {
                _yearFrom = value;
                RaisePropertyChanged(() => yearFrom);
            }
        }

        private int? _yearTo;
        public int? yearTo
        {
            get => _yearTo;
            set
            {
                _yearTo = value;
                RaisePropertyChanged(() => yearTo);
            }
        }

        private int? _timesSelectedFrom;
        public int? timesSelectedFrom
        {
            get => _timesSelectedFrom;
            set
            {
                _timesSelectedFrom = value;
                RaisePropertyChanged(() => timesSelectedFrom);
            }
        }

        private int? _timesSelectedTo;
        public int? timesSelectedTo
        {
            get => _timesSelectedTo;
            set
            {
                _timesSelectedTo = value;
                RaisePropertyChanged(() => timesSelectedTo);
            }
        }

        private FilterBooleanOption _dinked;
        public FilterBooleanOption dinked
        {
            get => _dinked;
            set
            {
                _dinked = value;
                RaisePropertyChanged(() => dinked);
            }
        }

        private FilterBooleanOption _pictureCover;
        public FilterBooleanOption pictureCover
        {
            get => _pictureCover;
            set
            {
                _pictureCover = value;
                RaisePropertyChanged(() => pictureCover);
            }
        }

        private FilterBooleanOption _jukeboxCardPrinted;
        public FilterBooleanOption jukeboxCardPrinted
        {
            get => _jukeboxCardPrinted;
            set
            {
                _jukeboxCardPrinted = value;
                RaisePropertyChanged(() => jukeboxCardPrinted);
            }
        }

        private FilterBooleanOption _singleLabelPrinted;
        public FilterBooleanOption singleLabelPrinted
        {
            get => _singleLabelPrinted;
            set
            {
                _singleLabelPrinted = value;
                RaisePropertyChanged(() => singleLabelPrinted);
            }
        }
    }

    public enum FilterBooleanOption
    {
        Yes,
        No,
        All
    }
}
