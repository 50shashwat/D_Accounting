using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace D_Accounting
{
    public class MainViewModel : INotifyPropertyChanged
    {
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

        public FileInfo DataFilePath
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
                                                        "d_accounting_data.xml"));

        /// <summary>
        /// Default constructor of the main VM
        /// </summary>
        public MainViewModel()
        {
            try
            {
                ListCasesXmlReaderParser readParser = new ListCasesXmlReaderParser(DataFilePath);
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

        #region Menu bar commands
        public ICommand NewCommand
        {
            get
            {
                return new CommandHandler(New);
            }
        }

        private void New()
        {
            DoCommand(new NewCommand(this, mCases));
        }
        #endregion

        #region Save command
        public ICommand SaveCommand
        {
            get
            {
                return new CommandHandler(Save);
            }
        }

        private void Save()
        {
            new ListCasesXmlWriterParser(DataFilePath).Write(mCases);
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
            DoCommand(new AddAccountCommand(this, mCases, mWrittenAccount));
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
            DoCommand(new DeleteAccountCommand(this, mCases, mWrittenAccount));
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
            DoCommand(new AddOperationCommand(this, mCases, mSelectedAccount));
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
