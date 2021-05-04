using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropositionalFormulaProver.Prover;

namespace PropositionalFormulaProver.Prover.Node
{
    public class LeftOrRight : INode
    {
        List<INode> children = new List<INode>();

        public void appendChild(INode child) { this.children.Add(child); }
        public string getDebugString() { return children.Any() ? string.Join(", ", children.Select(node => node.getDebugString()).Reverse().ToArray()) : ""; }
        public string getString() { return children.Any() ? string.Join(", ", children.Select(node => node.getString()).Reverse().ToArray()) : ""; }
        public List<INode> getChildren() => children;
        public INode deepCopy()
        {
            LeftOrRight lr = new LeftOrRight();
            foreach (INode node in children)
                lr.appendChild(node.deepCopy());
            return lr;
        }
    }
}
