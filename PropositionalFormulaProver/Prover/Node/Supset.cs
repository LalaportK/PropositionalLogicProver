using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropositionalFormulaProver.Prover;

namespace PropositionalFormulaProver.Prover.Node
{
    public class Supset : INode
    {
        INode left;
        INode right;
        string symbol = @"\supset ";

        public Supset(INode right, INode left)
        {
            this.left = left;
            this.right = right;
        }
        public string getDebugString() { return "(" + this.left.getDebugString() + this.symbol + this.right.getDebugString() + ")"; }
        public string getString() => getChildString(left) + this.symbol + getChildString(right);
        public INode getLeft() => this.left;
        public INode getRight() => this.right;
        public INode deepCopy() => new Supset(this.left.deepCopy(), this.right.deepCopy());

        private string getChildString(INode node)
        {
            string ret = "";

            if (isLowerChild(node))
                ret = node.getString();
            else
                ret = "(" + node.getString() + ")";

            return ret;
        }

        private bool isLowerChild(INode node) => node is Atom || node is Negation || node is Conjunction || node is Disjunction;
    }
}
