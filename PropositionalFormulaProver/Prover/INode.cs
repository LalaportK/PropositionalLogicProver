using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropositionalFormulaProver.Prover
{
    public interface INode
    {
        string getString();
        string getDebugString();
        INode deepCopy();
    }
}
