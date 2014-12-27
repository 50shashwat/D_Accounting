﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace D_Accounting
{
    public class MainViewModel : INotifyPropertyChanged
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
        /// Default constructor of the main VM
        /// </summary>
        public MainViewModel()
        {
            Cases = new ListCases();
        }

        // ***** Commands ***** //
        // TODO : undo/redo : save command : design pattern command
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
                // TODO : maybe the save command could have an CanExecute attribute : so if saved and no new changes done => inactive button
                return mSaveCommand ?? (mSaveCommand = new CommandHandler(Save));
            }
        }
        private ICommand mSaveCommand;
        
        private void Save()
        {
            Console.Beep();
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
            Cases.AddAccount(mWrittenAccount);
            WrittenAccount = "";
            // TODO : the view has to update the grid row and column definition
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
            Cases.RemoveAccount(mWrittenAccount);
            WrittenAccount = "";
        }

        #endregion // Remove account command

        // TODO : save command

        #endregion // Commands



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}