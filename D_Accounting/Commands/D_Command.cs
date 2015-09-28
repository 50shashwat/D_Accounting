namespace D_Accounting
{
    public abstract class D_Command
    {
        protected MainViewModel ViewModel;

        public D_Command(MainViewModel vm)
        {
            ViewModel = vm;
        }

        public abstract void Execute();

        public abstract void ExecuteReverse();
    }
}
