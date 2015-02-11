using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Accounting
{
    public class AddOperationCommand : D_Command
    {
        private string SelectedAcccount;

        public AddOperationCommand(MainViewModel vm, ListCases lc, string selectedAccount) : base(vm, lc)
        {
            SelectedAcccount = selectedAccount;
        }

        public override void Execute()
        {
            ListCases.AddOperation(SelectedAcccount);
        }

        public override void ExecuteReverse()
        {
            // TODO : delete operation
        }
    }
}
