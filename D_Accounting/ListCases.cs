using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using D_AccountingCore;

namespace D_Accounting
{
    public class ListCases : ObservableCollection<AbstractCase>
    {
        public ListCases()
        {
            Add(new FixDescriptionCase() { Row = 0, Column = 0, Name = "Account name" });

            Add(new FixDescriptionCase() { Row = 1, Column = 0, Name = "Initial amount" });
            Add(new FixDescriptionCase() { Row = 2, Column = 0, Name = "Real amount" });
            Add(new FixDescriptionCase() { Row = 3, Column = 0, Name = "Theoretical amount" });

            Add(new FixDescriptionCase() { Row = 0, Column = 1, Name = "OK?" });
            Add(new FixDescriptionCase() { Row = 0, Column = 2, Name = "Description" });

            Add(new GrayUnaccessibleCase() { Row = 1, Column = 1 });
            Add(new GrayUnaccessibleCase() { Row = 1, Column = 2 });
            Add(new GrayUnaccessibleCase() { Row = 2, Column = 1 });
            Add(new GrayUnaccessibleCase() { Row = 2, Column = 2 });
            Add(new GrayUnaccessibleCase() { Row = 3, Column = 1 });
            Add(new GrayUnaccessibleCase() { Row = 3, Column = 2 });
        }

        /// <summary>
        /// The number of rows contained by the data table
        /// </summary>
        public int RowCount
        {
            get
            {
                int nbRow = 0;
                List<int> countedRows = new List<int>();
                foreach (AbstractCase c in this)
                    if (!countedRows.Contains(c.Row))
                    {
                        ++nbRow;
                        countedRows.Add(c.Row);
                    }

                return nbRow;
            }
        }

        /// <summary>
        /// The number of columns contained by the data table
        /// </summary>
        public int ColumnCount
        {
            get
            {
                int nbColumn = 0;
                List<int> countedColumns = new List<int>();
                foreach (AbstractCase c in this)
                    if (!countedColumns.Contains(c.Column))
                    {
                        ++nbColumn;
                        countedColumns.Add(c.Column);
                    }
                return nbColumn;
            }
        }
        
        /// <summary>
        /// Override of the InsertItem
        ///     - every add or insert goes by this method
        ///     - insert so that the list is sorted by rows and columns
        /// </summary>
        /// <param name="index">Index where to add : modified to add it to the right position</param>
        /// <param name="item">Item to insert</param>
        protected override void InsertItem(int index, AbstractCase item)
        {
            for (int i = 0; i < Count; ++i)
            {
                switch (Math.Sign(this[i].CompareTo(item)))
                {
                    case 0:
                    case 1:
                        base.InsertItem(i, item);
                        return;
                    case -1:
                        break;
                }
            }  

            base.InsertItem(index, item);
        }
    }
}
