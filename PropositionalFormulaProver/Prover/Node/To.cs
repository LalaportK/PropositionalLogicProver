using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropositionalFormulaProver.Prover;

namespace PropositionalFormulaProver.Prover.Node
{
    /// <summary>
    /// The top level node of a formula.
    /// </summary>
    public class To : INode
    {
        INode left;
        INode right;
        string symbol = @"\to ";

        public To(INode left, INode right)
        {
            this.left = left;
            this.right = right;
        }
        public string getDebugString() { return "(" + this.left.getDebugString() + this.symbol + this.right.getDebugString() + ")"; }
        public string getString() { return this.left.getString() + this.symbol + this.right.getString(); }
        public LeftOrRight getLeft() => (LeftOrRight)left;
        public LeftOrRight getRight() => (LeftOrRight)right;
        public INode deepCopy() => new To(left.deepCopy(), right.deepCopy());
    }
}
