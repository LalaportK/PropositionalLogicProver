using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropositionalFormulaProver.Prover;

namespace PropositionalFormulaProver.Prover.Node
{
    public class Disjunction : INode
    {
        INode left;
        INode right;
        string symbol = @"\lor ";

        public Disjunction(INode right, INode left)
        {
            this.left = left;
            this.right = right;
        }
        public string getDebugString() { return "(" + this.left.getDebugString() + this.symbol + this.right.getDebugString() + ")"; }
        public string getString() => getChildString(left) + this.symbol + getChildString(right);

        public INode getLeft() => this.left;
        public INode getRight() => this.right;
        public INode deepCopy() => new Disjunction(left.deepCopy(), right.deepCopy());

        private string getChildString(INode node)
        {
            string ret = "";

            if (isLowerChild(node))
                ret = node.getString();
            else
                ret = "(" + node.getString() + ")";

            return ret;
        }

        private bool isLowerChild(INode node) => node is Atom || node is Negation || node is Conjunction;
    }
}
