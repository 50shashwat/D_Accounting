using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace D_Accounting
{
    internal class UndoRedoControl
    {
        /// <summary>
        /// Done commands
        /// </summary>
        private Stack<D_Command> DoneCommands = new Stack<D_Command>();

        /// <summary>
        /// Undone commands
        /// </summary>
        private Stack<D_Command> UndoneCommands = new Stack<D_Command>();

        public bool DoneEmpty
        {
            get
            {
                return DoneCommands.Count == 0;
            }
        }

        public bool UndoneEmpty
        {
            get
            {
                return UndoneCommands.Count == 0;
            }
        }

        /// <summary>
        /// Execute a command for the first time
        /// </summary>
        /// <param name="c">The new command</param>
        internal void Do(D_Command c)
        {
            c.Execute();
            DoneCommands.Push(c);
            UndoneCommands.Clear();
        }

        /// <summary>
        /// Undoes the last done command
        /// </summary>
        internal void Undo()
        {
            if (DoneCommands.Count == 0)
                return;

            D_Command c = DoneCommands.Pop();
            c.ExecuteReverse();
            UndoneCommands.Push(c);
        }

        /// <summary>
        /// Redoes the last undone command
        /// </summary>
        internal void Redo()
        {
            if (UndoneCommands.Count == 0)
                return;

            D_Command c = UndoneCommands.Pop();
            c.Execute();
            DoneCommands.Push(c);
        }
    }
}
