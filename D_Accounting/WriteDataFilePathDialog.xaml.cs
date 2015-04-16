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
using System.Windows.Shapes;

namespace D_Accounting
{
    /// <summary>
    /// Logique d'interaction pour WriteDataFilePathDialog.xaml
    /// </summary>
    public partial class WriteDataFilePathDialog : Window
    {
        private System.IO.FileInfo oldValue = null;

        public WriteDataFilePathDialog(System.IO.FileInfo initValue)
        {
            InitializeComponent();

            oldValue = initValue;
        }

        private void Event_ClickSaveButton_ReturnNewValue(object sender, RoutedEventArgs e)
        {
            Close();   
        }

        private void Event_ClientCancelButton_ReturnOldValue(object sender, RoutedEventArgs e)
        {
            (App.Current.Resources["mMainViewModel"] as MainViewModel).DataFilePath = oldValue;

            Close();
        }
    }
}
