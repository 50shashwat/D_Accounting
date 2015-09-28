namespace D_Accounting
{
    public class NewCommand : D_Command
    {
        private ListCases OldList = null;
        
        public NewCommand(MainViewModel vm) : base(vm)
        {
            OldList = ViewModel.Cases;
        }

        public override void Execute()
        {
            ViewModel.Cases = new ListCases();
        }

        public override void ExecuteReverse()
        {
            ViewModel.Cases = OldList;
        }
    }
}
