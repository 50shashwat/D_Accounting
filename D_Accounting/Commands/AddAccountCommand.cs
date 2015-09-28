namespace D_Accounting
{
    public class AddAccountCommand : D_Command
    {
        private string AccountName;

        public AddAccountCommand(MainViewModel vm, string accountName) : base(vm)
        {   
            AccountName = accountName;
        }

        public override void Execute()
        {
            ViewModel.Cases.AddAccount(AccountName);
        }

        public override void ExecuteReverse()
        {
            ViewModel.Cases.RemoveAccount(AccountName);
        }
    }
}
