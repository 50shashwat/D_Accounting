using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Accounting
{
    public class AddAccountCommand : D_Command
    {
        private string AccountName;

        public AddAccountCommand(MainViewModel vm, ListCases lc, string accountName) : base(vm, lc)
        {   
            AccountName = accountName;
        }

        public override void Execute()
        {
            this.ListCases.AddAccount(AccountName);
        }

        public override void ExecuteReverse()
        {
            this.ListCases.RemoveAccount(AccountName);
        }
    }
}
