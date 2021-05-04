using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropositionalFormulaProver.Prover;

namespace PropositionalFormulaProver.Prover.Node
{
    public class Negation: INode
    {
        INode child;
        string symbol = @"\neg ";

        public Negation(INode child) { this.child = child; }
        public string getDebugString() { return symbol + "(" + child.getDebugString() + ")"; }
        public string getString() {
            string childString = "";
            if (child is Atom)
                childString = child.getString();
            else
                childString = "(" + child.getString() + ")";
            return symbol + childString;
        }
        public INode getChild() => this.child;
        public void setChild(INode child) { this.child = child; }
        public INode deepCopy() => new Negation(child.deepCopy());
    }
}
