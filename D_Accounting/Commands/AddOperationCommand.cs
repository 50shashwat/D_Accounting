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
