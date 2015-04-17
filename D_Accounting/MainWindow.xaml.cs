using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            for (int i = 0; i < rowNumber; ++i)
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });

            // Add columns
            for (int i = 0; i < columnNumber; ++i)
            {
                ColumnDefinition colDef = new ColumnDefinition();

                if (i == columnNumber - 1)
                    colDef.Width = new GridLength(1, GridUnitType.Star);
                else
                    colDef.Width = new GridLength(1, GridUnitType.Auto);

                grid.ColumnDefinitions.Add(colDef);
            }

            UpdateLayout();
        }

        private void Event_ClickMenuItem_ChangeDataFilePathDialog(object sender, RoutedEventArgs e)
        {
            ModifyDataFilePathDialog changeNamDialog = new ModifyDataFilePathDialog(ViewModel.Settings.DataFilePath);
            bool? result = changeNamDialog.ShowDialog();

            if (result == true)
                ViewModel.NewDataFilePath();
        }
    }
}
