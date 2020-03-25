using MvvmCross.Core.ViewModels;
using SinglesApp.Core.Repositories;
using SinglesApp.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SinglesApp.Core.Controllers
{
    public class FilterController
    {
        SingleDbRepository _singleDbRepository;
        FilterViewModel filter;

        public FilterController(SingleDbRepository singleDbRepository)
        {
            _singleDbRepository = singleDbRepository;
        }

        public FilterViewModel CreateFilter(IEnumerable<SingleViewModel> singles)
        {
            filter = new FilterViewModel()
            {
                searchTerm = "",
                dinked = FilterBooleanOption.All,
                jukeboxCardPrinted = FilterBooleanOption.All,
                pictureCover = FilterBooleanOption.All,
                singleLabelPrinted = FilterBooleanOption.All,
                yearFrom = singles.Min(s => s.singleYear),
                yearTo = singles.Max(s => s.singleYear),
                timesSelectedFrom = singles.Min(s => s.numberOfSelections),
                timesSelectedTo = singles.Max(s => s.numberOfSelections)
            };

            return filter;
        }

        public MvxObservableCollection<SingleViewModel> UpdateFilter()
        {
            IEnumerable<SingleViewModel> filteredSingles;

            filteredSingles = _singleDbRepository.GetAllSingles().Where(s => s.artistName.IndexOf(filter.searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                                                                 s.aSideTitle.IndexOf(filter.searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                                                                 s.bSideTitle.IndexOf(filter.searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                                                                 (s.genreName != null && s.genreName.IndexOf(filter.searchTerm, StringComparison.OrdinalIgnoreCase) >= 0));

            filteredSingles = filteredSingles.Where(s => s.singleYear >= filter.yearFrom && s.singleYear <= filter.yearTo);
            filteredSingles = filteredSingles.Where(s => s.numberOfSelections >= filter.timesSelectedFrom && s.numberOfSelections <= filter.timesSelectedTo);

            if (filter.dinked != FilterBooleanOption.All)
            {
                if (filter.dinked == FilterBooleanOption.Yes)
                {
                    filteredSingles = filteredSingles.Where(s => s.dinked == true);
                }
                else
                {
                    filteredSingles = filteredSingles.Where(s => s.dinked == false);
                }
            }

            if (filter.jukeboxCardPrinted != FilterBooleanOption.All)
            {
                if (filter.jukeboxCardPrinted == FilterBooleanOption.Yes)
                {
                    filteredSingles = filteredSingles.Where(s => s.jukeboxCardPrinted == true);
                }
                else
                {
                    filteredSingles = filteredSingles.Where(s => s.jukeboxCardPrinted == false);
                }
            }

            if (filter.pictureCover != FilterBooleanOption.All)
            {
                if (filter.pictureCover == FilterBooleanOption.Yes)
                {
                    filteredSingles = filteredSingles.Where(s => s.pictureCover == true);
                }
                else
                {
                    filteredSingles = filteredSingles.Where(s => s.pictureCover == false);
                }
            }

            if (filter.singleLabelPrinted != FilterBooleanOption.All)
            {
                if (filter.singleLabelPrinted == FilterBooleanOption.Yes)
                {
                    filteredSingles = filteredSingles.Where(s => s.singleLabelPrinted == true);
                }
                else
                {
                    filteredSingles = filteredSingles.Where(s => s.singleLabelPrinted == false);
                }
            }

            return new MvxObservableCollection<SingleViewModel>(filteredSingles);
        }
    }
}
