using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D_Accounting
{
    public class LoadDataCommand : D_Command
    {
        private ListCases OldCases = null;
        private ListCases NewCases = null;

        public LoadDataCommand(MainViewModel vm, ListCases lc, System.IO.FileInfo dataFile) : base(vm, lc)
        {
            OldCases = lc.Clone() as ListCases;
            NewCases = new ListCasesXmlReaderParser(dataFile).Read();
        }

        public override void Execute()
        {
            this.ListCases.CopyContent(NewCases);
        }

        public override void ExecuteReverse()
        {
            this.ListCases.CopyContent(OldCases);
        }
    }
}
