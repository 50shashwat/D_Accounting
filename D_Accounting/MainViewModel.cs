using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;

namespace D_Accounting
{
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Application settings
        /// </summary>
        public AppSettingsHandler Settings
        {
            get
            {
                return mSettings;
            }
        }
        private AppSettingsHandler mSettings = new AppSettingsHandler();

        public void NewDataFilePath()
        {
            mSettings.SaveSettings();
            if (mSettings.DataFilePath.Exists)
            {
                // TODO DISPLAY MessageBox : Do you want to load the new data file ? (it exists !)
            }

            OnPropertyChanged("LoadCommand");
        }

        /// <summary>
        /// Close action of the main window
        /// </summary>
        public Action CloseAction
        {
            private get;
            set;
        }

        /// <summary>
        /// The data
        /// </summary>
        public ListCases Cases
        {
            get;
            private set;
        }

        /// <summary>
        /// The undo-redo controller
        /// </summary>
        private UndoRedoControl UndoRedoC = new UndoRedoControl();

        /// <summary>
        /// The written account (adding & deleting)
        /// </summary>
        public String WrittenAccount
        {
            get
            {
                return mWrittenAccount;
            }
            set
            {
                mWrittenAccount = value;
                OnPropertyChanged("WrittenAccount");

                if (mWrittenAccount == null || mWrittenAccount.Trim().Equals(""))
                {
                    CanExecuteAddAccount = false;
                    CanExecuteRemoveAccount = false;
                }
                else
                {
                    if (Cases.AccountNames.Contains(mWrittenAccount))
                    {
                        CanExecuteAddAccount = false;
                        CanExecuteRemoveAccount = true;
                    }
                    else if (Cases.ColumnTitles.Contains(mWrittenAccount))
                    {
                        CanExecuteAddAccount = false;
                        CanExecuteRemoveAccount = false;
                    }
                    else
                    {
                        CanExecuteAddAccount = true;
                        CanExecuteRemoveAccount = false;
                    }
                }
                OnPropertyChanged("AddAccountCommand");
                OnPropertyChanged("RemoveAccountCommand");
            }
        }
        private String mWrittenAccount;

        /// <summary>
        /// The selected account (adding operations)
        /// </summary>
        public String SelectedAccount
        {
            set
            {
                mSelectedAccount = value;
                OnPropertyChanged("SelectedAccount");

                if (mSelectedAccount == null || !Cases.AccountNames.Contains(mSelectedAccount))
                    CanExecuteAddOperation = false;
                else
                    CanExecuteAddOperation = true;
                OnPropertyChanged("AddOperationCommand");
            }
        }
        private string mSelectedAccount;


        /// <summary>
        /// Default constructor of the main VM
        /// </summary>
        public MainViewModel()
        {
            try
            {
                ListCasesXmlReaderParser readParser = new ListCasesXmlReaderParser(mSettings.DataFilePath);
                Cases = readParser.Read();
            }
            catch (Exception)
            {
                Cases = new ListCases();
            }
        }

        // ***** Commands ***** //
        #region Commands
        #region Close command
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
        #endregion // Close command

        #region Save command
        public ICommand SaveCommand
        {
            get
            {
                return new CommandHandler(Save);
            }
        }
        private ICommand mSaveCommand;

        private void Save()
        {
            if (mSettings.DataFilePath.Exists)
            {
                // TODO : MessageBox : are you sure you want to erase content of the current data file?
            }

            new ListCasesXmlWriterParser(mSettings.DataFilePath).Write(Cases);
        }
        #endregion // Save command

        #region Add account command
        public ICommand AddAccountCommand
        {
            get
            {
                return new CommandHandler(AddAccount, CanExecuteAddAccount);
            }
        }

        private bool CanExecuteAddAccount { get; set; }

        private void AddAccount()
        {
            DoCommand(new AddAccountCommand(this, Cases, mWrittenAccount));
            WrittenAccount = "";
        }
        #endregion // Add account command

        #region Remove account command
        public ICommand RemoveAccountCommand
        {
            get
            {
                return new CommandHandler(RemoveAccount, CanExecuteRemoveAccount);
            }
        }

        private bool CanExecuteRemoveAccount { get; set; }

        private void RemoveAccount()
        {
            DoCommand(new DeleteAccountCommand(this, Cases, mWrittenAccount));
            WrittenAccount = "";
        }

        #endregion // Remove account command

        #region Add operation command
        public ICommand AddOperationCommand
        {
            get
            {
                return new CommandHandler(AddOperation, CanExecuteAddOperation);
            }
        }

        private bool CanExecuteAddOperation { get; set; }

        private void AddOperation()
        {
            DoCommand(new AddOperationCommand(this, Cases, mSelectedAccount));
        }
        #endregion

        #region Load command
        public ICommand LoadCommand
        {
            get
            {
                return new CommandHandler(Load, CanExecuteLoadCommand);
            }
        }

        private bool CanExecuteLoadCommand
        {
            get
            {
                return mSettings.DataFilePath.Exists;
            }
        }

        private void Load()
        {
            //DoCommand(...);
            try
            {
                Cases = new ListCasesXmlReaderParser(mSettings.DataFilePath).Read();
                OnPropertyChanged("Cases");
            }
            catch (XmlException)
            {
                // TODO : how to display (MVVM) an messagebox with the error ??
            }
        }
        #endregion

        #region UndoRedo commands
        /// <summary>
        /// Undo command
        /// </summary>
        public ICommand UndoCommand
        {
            get
            {
                return new CommandHandler(Undo, !UndoRedoC.DoneEmpty);
            }
        }

        private void Undo()
        {
            UndoRedoC.Undo();
            UpdateUndoRedoCommands();
        }

        /// <summary>
        /// Redo command
        /// </summary>
        public ICommand RedoCommand
        {
            get
            {
                return new CommandHandler(Redo, !UndoRedoC.UndoneEmpty);
            }
        }

        private void Redo()
        {
            UndoRedoC.Redo();
            UpdateUndoRedoCommands();
        }

        #endregion // UndoRedo commands

        private void DoCommand(D_Command c)
        {
            UndoRedoC.Do(c);
            UpdateUndoRedoCommands();
        }

        private void UpdateUndoRedoCommands()
        {
            OnPropertyChanged("UndoCommand");
            OnPropertyChanged("RedoCommand");
        }

        #endregion // Commands

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
