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

        public AddOperationCommand(MainViewModel vm, string selectedAccount) : base(vm)
        {
            SelectedAcccount = selectedAccount;
        }

        public override void Execute()
        {
            ViewModel.Cases.AddOperation(SelectedAcccount);
        }

        public override void ExecuteReverse()
        {
            ViewModel.Cases.DeleteOperation(SelectedAcccount);
        }
    }
}
