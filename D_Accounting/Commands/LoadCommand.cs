using System.IO;

namespace D_Accounting.Commands
{
    public class LoadCommand : D_Command
    {
        private ListCases OldCases;
        private FileInfo OldFilePath;
        private ListCases NewCases;
        private FileInfo NewFilePath;
        
        /// <summary>
        /// Loading a file stores the old data and the new data in the command.
        /// So, when undoing or redoing this command, you won't access the disk again.
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="lc"></param>
        /// <param name="newCases"></param>
        public LoadCommand(MainViewModel vm, string newFilePath, ListCases newCases) : base(vm)
        {
            OldCases = vm.Cases;
            OldFilePath = vm.Settings.DataFilePath;

            NewCases = newCases;
            NewFilePath = new FileInfo(newFilePath);
        }

        public override void Execute()
        {
            ViewModel.Cases = NewCases;
            ViewModel.Settings.DataFilePath = NewFilePath;
        }

        public override void ExecuteReverse()
        {
            ViewModel.Cases = OldCases;
            ViewModel.Settings.DataFilePath = OldFilePath;
        }
    }
}
