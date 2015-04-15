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
        public WriteDataFilePathDialog()
        {
            InitializeComponent();
        }

        private void Click_Button_OpenFileChooser(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileChooser = new Microsoft.Win32.OpenFileDialog();

            bool? result = fileChooser.ShowDialog(this);

            if (result == true)
            {
                (App.Current.Resources["mMainViewModel"] as MainViewModel).DataFilePath = new System.IO.FileInfo(fileChooser.FileName);
            }
        }
    }
}
