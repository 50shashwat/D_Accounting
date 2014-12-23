using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();

            (Resources["mMainViewModel"] as MainViewModel).CloseAction = this.Close;
        }

        /// <summary>
        /// Loaded event on Grid : add row and column definitions
        /// </summary>
        /// <param name="sender">Grid</param>
        /// <param name="e"></param>
        private void Event_LoadMainDataGrid_CreateRowsColumns(object sender, RoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            ListCases list = (Resources["mMainViewModel"] as MainViewModel).Cases;

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
        }
    }
}
