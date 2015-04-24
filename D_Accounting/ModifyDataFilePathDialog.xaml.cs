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
    public partial class ModifyDataFilePathDialog : Window
    {
        private System.IO.FileInfo oldValue = null;

        public ModifyDataFilePathDialog(System.IO.FileInfo initValue)
        {
            InitializeComponent();

            oldValue = initValue;
        }

        public CommandHandler SavePathCommand
        {
            get
            {
                return new CommandHandler(delegate()
                    {
                        SetDialogResultAndClose(true);
                    });
            }
        }

        public CommandHandler CancelCommand
        {
            get
            {
                return new CommandHandler(delegate()
                    {
                        SetDialogResultAndClose(false);
                    });
            }
        }

        private void SetDialogResultAndClose(bool result)
        {
            DialogResult = result;
            Close();
        }

        private void Event_TextBox_FilePath_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).CaretIndex = (sender as TextBox).Text.Length;
        }

        private void Event_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!DialogResult.HasValue || !DialogResult.Value)
                (App.Current.Resources["mMainViewModel"] as MainViewModel).Settings.DataFilePath = oldValue;
        }
    }
}
