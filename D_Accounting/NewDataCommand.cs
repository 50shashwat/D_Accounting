using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D_Accounting
{
    public class NewDataCommand : D_Command
    {
        private ListCases oldListCase = null;

        public NewDataCommand(MainViewModel vm, ListCases lc) : base(vm, lc)
        {
            oldListCase = lc.Clone() as ListCases;
        }

        public override void Execute()
        {
            ListCases.CopyContent(new ListCases());
        }

        public override void ExecuteReverse()
        {
            ListCases.CopyContent(oldListCase);
        }
    }
}
