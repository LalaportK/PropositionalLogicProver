using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropositionalFormulaProver.Prover;

namespace PropositionalFormulaProver.Prover.Node
{
    public class Atom: INode
    {
        string atom;

        public Atom(string atom){ this.atom = atom; }
        public string getDebugString() => this.atom;
        public string getString() => this.atom;
        public INode deepCopy() => new Atom(atom);
    }
}
