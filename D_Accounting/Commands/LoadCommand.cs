using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public LoadCommand(MainViewModel vm, ListCases lc, string newFilePath, ListCases newCases) : base(vm, lc)
        {
            OldCases = lc;
            OldFilePath = vm.DataFilePath;

            NewCases = newCases;
            NewFilePath = new FileInfo(newFilePath);
        }

        public override void Execute()
        {
            ViewModel.Cases = NewCases;
            ViewModel.DataFilePath = NewFilePath;
        }

        public override void ExecuteReverse()
        {
            ViewModel.Cases = OldCases;
            ViewModel.DataFilePath = OldFilePath;
        }
    }
}
