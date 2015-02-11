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

        public DeleteAccountCommand(MainViewModel vm, ListCases lc, string accountName) : base(vm, lc)
        {
            AccountName = accountName;
        }

        public override void Execute()
        {
            this.ListCases.RemoveAccount(AccountName);
        }

        public override void ExecuteReverse()
        {
            this.ListCases.AddAccount(AccountName);
        }
    }
}
