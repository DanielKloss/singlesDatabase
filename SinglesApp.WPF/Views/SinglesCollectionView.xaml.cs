using Microsoft.Win32;
using MvvmCross.Wpf.Views;
using SinglesApp.Core.ViewModels;
using SQLite;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SinglesApp.WPF.Views
{
    public partial class SinglesCollectionView : MvxWpfView
    {
        SinglesCollectionViewModel viewModel;

        public SinglesCollectionView()
        {
            InitializeComponent();
        }

        private void pageRoot_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = (SinglesCollectionViewModel)DataContext;

            GetDatabase();
        }

        private void GetDatabase()
        {
            try
            {
                if (Properties.Settings.Default.databaseLocation == "" || Properties.Settings.Default.databaseLocation == null)
                {
                    OpenFileDialog dialog = new OpenFileDialog();
                    if (dialog.ShowDialog() == true)
                    {
                        Properties.Settings.Default.databaseLocation = dialog.FileName;
                        Properties.Settings.Default.Save();

                        viewModel.SetDatabaseLocation(dialog.FileName);

                    }
                }
                else
                {
                    viewModel.SetDatabaseLocation(Properties.Settings.Default.databaseLocation);
                }
            }
            catch (SQLiteException exception)
            {
                MessageBox.Show(exception.Message, "Could not open Database");
                GetDatabase();
            }
        }

        private void TextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SingleViewModel singleToDelete = (SingleViewModel)((TextBlock)sender).Tag;

            MessageBoxResult deleteResult = MessageBox.Show(string.Format("Are you sure you want to delete {0} by {1}?\n\nIt will also be removed from any jukebox selections you have made.", singleToDelete.aSideTitle, singleToDelete.artistName), "Delete Single", MessageBoxButton.YesNo);

            if (deleteResult == MessageBoxResult.Yes)
            {
                viewModel.RemoveFromCollection(singleToDelete);
            }
        }
    }
}
