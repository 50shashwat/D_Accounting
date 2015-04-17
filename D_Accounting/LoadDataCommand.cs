using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Accounting
{
    public class LoadDataCommand : D_Command
    {
        private System.IO.FileInfo DataFilePath = null;

        private ListCases OldCases = null;

        public LoadDataCommand(MainViewModel vm, ListCases lc, System.IO.FileInfo dataFile) : base(vm, lc)
        {
            DataFilePath = dataFile;
            OldCases = lc.Clone() as ListCases;
        }

        public override void Execute()
        {
            this.ListCases = new ListCasesXmlReaderParser(DataFilePath).Read();
        }

        public override void ExecuteReverse()
        {
            this.ListCases = OldCases.Clone() as ListCases;
        }
    }
}
