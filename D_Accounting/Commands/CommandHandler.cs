using System;
using System.Windows.Input;

namespace D_Accounting
{
    public class CommandHandler : ICommand
    {
        private Action action;
        private bool canExecute;

        public CommandHandler(Action a, bool canExe = true)
        {
            action = a;
            canExecute = canExe;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            action();
        }
    }
}
