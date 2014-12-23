using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace D_Accounting
{
    public class MainViewModel
    {
        public Action CloseAction
        {
            get;
            set;
        }

        public ListCases Cases
        {
            get;
            private set;
        }

        /// <summary>
        /// Default constructor of the main VM
        /// </summary>
        public MainViewModel()
        {
            Cases = new ListCases();
        }

        // ***** Commands ***** //

        public ICommand CloseCommand
        {
            get
            {
                return mCloseCommand ?? (mCloseCommand = new CommandHandler(Close));
            }
        }
        private ICommand mCloseCommand;

        private void Close()
        {
            // Ask if save...
            CloseAction();
        }

        public ICommand SaveCommand
        {
            get
            {
                return mSaveCommand ?? (mSaveCommand = new CommandHandler(Save));
            }
        }
        private ICommand mSaveCommand;
        
        private void Save()
        {
            Console.Beep();
        }
    }
}
