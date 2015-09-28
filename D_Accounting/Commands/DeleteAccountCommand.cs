using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Accounting
{
    public class DeleteAccountCommand : D_Command
    {
        private string AccountName;

        public DeleteAccountCommand(MainViewModel vm, string accountName) : base(vm)
        {
            AccountName = accountName;
        }

        public override void Execute()
        {
            ViewModel.Cases.RemoveAccount(AccountName);
        }

        public override void ExecuteReverse()
        {
            ViewModel.Cases.AddAccount(AccountName);
        }
    }
}
