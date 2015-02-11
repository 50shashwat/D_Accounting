using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace D_Accounting
{
    // TODO : delete operation button
    // TODO : format date for operations
    // TODO : sort table automatically by date (not sure)
    // TODO : save command
    // TODO : canExecute save just if something changed since last save
    // TODO : undo/redo (+ keyboard Ctrl-Z)
    // TODO : menu bar (undo/redo + save + load + change default load file name)
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
            get;
            private set;
        }

        /// <summary>
        /// Done commands
        /// </summary>
        private Stack<D_Command> DoneCommands = new Stack<D_Command>();

        /// <summary>
        /// Undone commands
        /// </summary>
        private Stack<D_Command> UndoneCommands = new Stack<D_Command>();

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
            Cases = new ListCases();
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
                return mSaveCommand ?? (mSaveCommand = new CommandHandler(Save));
            }
        }
        private ICommand mSaveCommand;

        bool undoRedo = true; // DELETE later
        private void Save()
        {
            // CHANGE later => test
            if (undoRedo)
            {
                Undo();
                undoRedo = false;
            }
            else
            {
                Redo();
                undoRedo = true;
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

        private bool CanExecuteAddAccount { get; set; }

        private void AddAccount()
        {
            Do(new AddAccountCommand(this, Cases, mWrittenAccount));
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
            Do(new DeleteAccountCommand(this, Cases, mWrittenAccount));
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
            Do(new AddOperationCommand(this, Cases, mSelectedAccount));
        }
        #endregion

        #endregion // Commands

        #region Do_Undo_Redo
        /// <summary>
        /// Execute a command for the first time
        /// </summary>
        /// <param name="c">The new command</param>
        public void Do(D_Command c)
        {
            c.Execute();
            DoneCommands.Push(c);
        }

        /// <summary>
        /// Undoes the last done command
        /// </summary>
        public void Undo()
        {
            if (DoneCommands.Count == 0)
                return;

            D_Command c = DoneCommands.Pop();
            c.ExecuteReverse();
            UndoneCommands.Push(c);
        }

        /// <summary>
        /// Redoes the last undone command
        /// </summary>
        public void Redo()
        {
            if (UndoneCommands.Count == 0)
                return;

            D_Command c = UndoneCommands.Pop();
            c.Execute();
            DoneCommands.Push(c);
        }
        #endregion // Do_Undo_Redo

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
