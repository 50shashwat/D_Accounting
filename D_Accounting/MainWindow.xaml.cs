using Microsoft.Win32;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace D_Accounting
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel = null;

        private Grid MainDataGrid = null;

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = App.Current.Resources["mMainViewModel"] as MainViewModel;

            // References the close method of the View for the ViewModel
            ViewModel.CloseAction = this.Close;

            // Updates the View when an item changed
            ((INotifyCollectionChanged)mItemsControl.Items).CollectionChanged += Event_ItemControl_CollectionChanged;

            // Add message box request handler
            ViewModel.MessageBoxRequest += new EventHandler<ShowMessageBoxEventArgs>(Event_ViewModel_MessageBoxRequest);
        }

        private void Event_ViewModel_MessageBoxRequest(object sender, ShowMessageBoxEventArgs e)
        {
            e.Show();
        }

        /// <summary>
        /// Loaded event on Grid : add row and column definitions
        /// And defines the MainDataGrid
        /// </summary>
        /// <param name="sender">Grid</param>
        /// <param name="e"></param>
        private void Event_LoadMainDataGrid_CreateRowsColumns(object sender, RoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            MainDataGrid = grid;

            UpdateMainDataGridLayout();
        }

        private void Event_ItemControl_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateMainDataGridLayout();
        }

        private void UpdateMainDataGridLayout()
        {
            Grid grid = MainDataGrid;
            ListCases list = ViewModel.Cases;

            if (grid == null || list == null)
                return;

            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();

            int rowNumber = list.RowCount;
            int columnNumber = list.ColumnCount;

            // Add rows
            Parallel.For(0, rowNumber, delegate(int i)
            {
                Application.Current.Dispatcher.InvokeAsync(new Action(() =>
                    {
                        grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
                    }));
            });

            // Add columns
            for (int i = 0; i < columnNumber - 1; ++i)
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            
            UpdateLayout();
        }

        private void Event_LoadFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;

            bool? isOkay = dlg.ShowDialog(this);
            if (isOkay.GetValueOrDefault())
            {
                try
                {
                    ViewModel.Load(dlg.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show(this, "Sorry, an error occured. We could not load your file. Try a valid xml file, created by this application.", "File loading error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Event_SaveAs(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            bool? isOkay = dlg.ShowDialog(this);
            if (isOkay.GetValueOrDefault())
            {
                ViewModel.SaveAs(dlg.FileName);
            }
        }
    }
}
