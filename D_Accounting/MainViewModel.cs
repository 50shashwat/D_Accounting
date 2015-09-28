using D_Accounting.Commands;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
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
            get
            {
                return mCases;
            }
            internal set
            {
                mCases = value;
                OnPropertyChanged("Cases");
            }
        }
        private ListCases mCases;

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
                    if (mCases.AccountNames.Contains(mWrittenAccount))
                    {
                        CanExecuteAddAccount = false;
                        CanExecuteRemoveAccount = true;
                    }
                    else if (mCases.ColumnTitles.Contains(mWrittenAccount))
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

                if (mSelectedAccount == null || !mCases.AccountNames.Contains(mSelectedAccount))
                    CanExecuteAddOperation = false;
                else
                    CanExecuteAddOperation = true;
                OnPropertyChanged("AddOperationCommand");
            }
        }
        private string mSelectedAccount;

        /*public FileInfo DataFilePath
        {
            get
            {
                return mDataFilePath;
            }
            set
            {
                mDataFilePath = value;
                OnPropertyChanged("DataFilePath");
            }
        }
        private FileInfo mDataFilePath = new FileInfo(Path.Combine(
                                                        Directory.GetCurrentDirectory(),
                                                        "d_accounting_data.xml"));*/

        /// <summary>
        /// Default constructor of the main VM
        /// </summary>
        public MainViewModel()
        {
            try
            {
                ListCasesXmlReaderParser readParser = new ListCasesXmlReaderParser(mSettings.DataFilePath);
                mCases = readParser.Read();
            }
            catch (Exception)
            {
                mCases = new ListCases();
            }
        }

        // ***** Commands ***** //
        #region Commands
        #region Close command
        public ICommand CloseCommand
        {
            get
            {
                return new CommandHandler(Close);
            }
        }

        private void Close()
        {
            // TODO : If something has been modified
            MessageBox_Show(MessageBoxCloseAnswer, "Changes may have been made.\nDo you want to save the possible changes before exiting?", "Save?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
        }
        private void MessageBoxCloseAnswer(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Cancel)
                return;

            if (result == MessageBoxResult.Yes)
                Save();

            CloseAction();
        }
        #endregion // Close command

        #region Menu bar commands (but not only)
        public ICommand NewCommand
        {
            get
            {
                return new CommandHandler(New);
            }
        }

        private void New()
        {
            DoCommand(new NewCommand(this));
        }

        /// <summary>
        /// Might throw an exception if it cannot read the file.
        /// </summary>
        /// <param name="filePath"></param>
        public void Load(string filePath) // TODO should be a command
        {
            // Read file
            ListCasesXmlReaderParser fileParser = new ListCasesXmlReaderParser(new FileInfo(filePath));
            ListCases loadedFileData = fileParser.Read();

            DoCommand(new LoadCommand(this, filePath, loadedFileData));
        }

        public void SaveAs(string fileName)
        {
            mSettings.DataFilePath = new FileInfo(fileName);
            Save();
        }
        #endregion

        #region Save command
        public ICommand SaveCommand
        {
            get
            {
                return new CommandHandler(Save); // TODO CanSave if no changes have been made
            }
        }

        private void Save()
        {
            try
            {
                new ListCasesXmlWriterParser(mSettings.DataFilePath).Write(Cases);
            }
            catch (Exception)
            {
                MessageBox_Show(null, "Could not save file at " + mSettings.DataFilePath + ".", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
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

        private bool CanExecuteAddAccount;

        private void AddAccount()
        {
            DoCommand(new AddAccountCommand(this, mWrittenAccount));
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

        private bool CanExecuteRemoveAccount;

        private void RemoveAccount()
        {
            DoCommand(new DeleteAccountCommand(this, mWrittenAccount));
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
            DoCommand(new AddOperationCommand(this, mSelectedAccount));
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
        private void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<ShowMessageBoxEventArgs> MessageBoxRequest;
        protected void MessageBox_Show(Action<MessageBoxResult> resultAction, string messageBoxText, string caption = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None, MessageBoxResult defaultResult = MessageBoxResult.None, MessageBoxOptions options = MessageBoxOptions.None)
        {
            if (this.MessageBoxRequest != null)
            {
                this.MessageBoxRequest(this, new ShowMessageBoxEventArgs(resultAction, messageBoxText, caption, button, icon, defaultResult, options));
            }
        }
    }
}
