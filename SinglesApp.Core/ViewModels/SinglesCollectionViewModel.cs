using MvvmCross.Core.ViewModels;
using SinglesApp.Core.Models;
using SinglesApp.Core.Repositories;
using System.Linq;
using System.Collections.Specialized;
using System.ComponentModel;
using SinglesApp.Core.Controllers;
using System;
using System.IO;
using System.Collections.Generic;
using SQLite;

namespace SinglesApp.Core.ViewModels
{
    public class SinglesCollectionViewModel : MvxViewModel
    {
        SingleDbRepository singleDbRepository;
        FilterController filterController;

        private MvxObservableCollection<SingleViewModel> _singles;
        public MvxObservableCollection<SingleViewModel> singles
        {
            get => _singles;
            set
            {
                _singles = value;
                RaisePropertyChanged(() => singles);
            }
        }

        private MvxObservableCollection<SingleViewModel> _selectionSingles;
        public MvxObservableCollection<SingleViewModel> selectionSingles
        {
            get => _selectionSingles;
            set
            {
                _selectionSingles = value;
                RaisePropertyChanged(() => selectionSingles);
            }
        }

        private MvxObservableCollection<JukeboxSelections> _selections;
        public MvxObservableCollection<JukeboxSelections> selections
        {
            get => _selections;
            set
            {
                _selections = value;
                RaisePropertyChanged(() => selections);
            }
        }

        private JukeboxSelections _selection;
        public JukeboxSelections selection
        {
            get => _selection;
            set
            {
                _selection = value;
                ChangeSelectionSingles();
                RaisePropertyChanged(() => selection);
            }
        }

        private SingleViewModel _newSingle;
        public SingleViewModel newSingle
        {
            get => _newSingle;
            set
            {
                _newSingle = value;
                RaisePropertyChanged(() => newSingle);
            }
        }

        private FilterViewModel _filter;
        public FilterViewModel filter
        {
            get => _filter;
            set
            {
                _filter = value;
                RaisePropertyChanged(() => filter);
            }
        }

        public IMvxCommand createNewSelectionCommand { get; }
        public IMvxCommand<SingleViewModel> addToSelectionCommand { get; }
        public IMvxCommand<SingleViewModel> removeFromSelectionCommand { get; }
        public IMvxCommand addNewSingleCommand { get; }
        public IMvxCommand exportSelectionCommand { get; }

        public SinglesCollectionViewModel()
        {
            createNewSelectionCommand = new MvxCommand(() => CreateNewSelection());
            addToSelectionCommand = new MvxCommand<SingleViewModel>(toAdd => AddToSelection(toAdd));
            removeFromSelectionCommand = new MvxCommand<SingleViewModel>(toRemove => RemoveFromSelection(toRemove));
            addNewSingleCommand = new MvxCommand(() => AddToCollection());
            exportSelectionCommand = new MvxCommand(() => ExportSelection());
        }

        private void ExportSelection()
        {
            List<string> selectionForFile = new List<string>();

            foreach (SingleViewModel single in selectionSingles)
            {
                selectionForFile.Add(string.Format("{0,-10}{1,-40}{2}", single.singleId, single.artistName, single.aSideTitle));
            }

            File.WriteAllLines(singleDbRepository.databaseLocation.Substring(0, singleDbRepository.databaseLocation.LastIndexOf("\\")) + "\\" + selection.selectionName.Replace('/', ' ').Replace(':', ' ') + ".txt", selectionForFile);
        }

        public void SetDatabaseLocation(string databaseLocation)
        {
            try
            {
                singleDbRepository = new SingleDbRepository(databaseLocation);
            }
            catch (SQLiteException)
            {
                throw;
            }

            filterController = new FilterController(singleDbRepository);

            newSingle = new SingleViewModel();
            singles = new MvxObservableCollection<SingleViewModel>(singleDbRepository.GetAllSingles());

            singles.CollectionChanged += Singles_CollectionChanged;
            GetSingleSelectionCount();

            selections = new MvxObservableCollection<JukeboxSelections>(singleDbRepository.GetSelections());
            selection = selections[selections.Count - 1];

            ChangeSelectionSingles();

            filter = filterController.CreateFilter(singles);
            filter.PropertyChanged += Filter_PropertyChanged;
        }

        private void GetSingleSelectionCount()
        {
            foreach (SingleViewModel recordSingle in singles)
            {
                recordSingle.PropertyChanged += EntityViewModelPropertyChanged;
                MvxObservableCollection<SingleViewModel> results = new MvxObservableCollection<SingleViewModel>(singleDbRepository.GetSingleSelections(recordSingle.singleId));
                recordSingle.numberOfSelections = results.Count;
            }
        }

        private void Filter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            singles = filterController.UpdateFilter();
        }

        private void Singles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (SingleViewModel item in e.OldItems)
                {
                    item.PropertyChanged -= EntityViewModelPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (SingleViewModel item in e.NewItems)
                {
                    item.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }
        }
        public void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SingleViewModel changedRecord = (SingleViewModel)sender;
            singleDbRepository.UpdateSingle(changedRecord);
        }

        private void ChangeSelectionSingles()
        {
            selectionSingles = new MvxObservableCollection<SingleViewModel>(singleDbRepository.GetSelectionSingles(selection.selectionId));
        }

        private void CreateNewSelection()
        {
            selection = singleDbRepository.CreateNewSelection();
            selections.Add(selection);
        }

        private void AddToSelection(SingleViewModel singleToAdd)
        {
            if (selectionSingles.Count < 20 && selectionSingles.FirstOrDefault(s => s.singleId == singleToAdd.singleId) == null)
            {
                selectionSingles.Add(singleToAdd);
                singleDbRepository.AddSingleToSelection(singleToAdd.singleId, selection.selectionId);
            }

            GetSingleSelectionCount();
        }

        private void RemoveFromSelection(SingleViewModel singleToRemove)
        {
            selectionSingles.Remove(singleToRemove);
            singleDbRepository.RemoveSingleFromSelection(singleToRemove.singleId, selection.selectionId);

            GetSingleSelectionCount();
        }

        private void AddToCollection()
        {
            singleDbRepository.AddSingleToCollection(newSingle);
            singles = new MvxObservableCollection<SingleViewModel>(singleDbRepository.GetAllSingles());
        }

        public void RemoveFromCollection(SingleViewModel singleToRemove)
        {
            singles.Remove(singleToRemove);
            singleDbRepository.RemoveSingleFromCollection(singleToRemove);
        }
    }
}