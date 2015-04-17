using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void NewDataFilePath()
        {
            mSettings.SaveSettings();
            if (mSettings.DataFilePath.Exists)
                MessageBox_Show(LoadNewDataFileAnswer, "Do you want to load the new data file?", "Load?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

            OnPropertyChanged("LoadCommand");
        }

        private void LoadNewDataFileAnswer(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                Load();
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
                return new CommandHandler(Close);
            }
        }

        private void Close()
        {
            // TODO : If something has been modified
            if (true)
                MessageBox_Show(MessageBoxCloseAnswer, "Changes have been made.\nDo you want to save the changes before exiting?", "Save?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
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

        #region Save command
        public ICommand SaveCommand
        {
            get
            {
                return new CommandHandler(Save); // CanSave if no changes have been made
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

        private bool CanExecuteRemoveAccount;

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
            try
            {
                //DoCommand(new LoadDataCommand(this, Cases, mSettings.DataFilePath));
                Cases = new ListCasesXmlReaderParser(mSettings.DataFilePath).Read();
                OnPropertyChanged("Cases");
            }
            catch (XmlException)
            {
                // TODO : how to display (MVVM) an messagebox with the error ??
                MessageBox_Show(null, "Error loading data file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
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
