using Antlr4.Runtime;
using PropositionalFormulaProver.Grammar;
using PropositionalFormulaProver.Prover;
using PropositionalFormulaProver.Prover.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PropositionalFormulaProver
{
    class Program
    {
        static void Main(string[] args)
        {
            string parsedString = @"\to ((P\supset Q)\supset P)\supset P";
            var inputStream = new AntlrInputStream(parsedString);
            var lexer = new FormulaLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new FormulaParser(commonTokenStream);
            var listener = new FormulaListener();
            parser.AddParseListener(listener);
            var graphContext = parser.equation();

            To to = (To)listener.getTree();
            Console.WriteLine(to.getDebugString());
            Console.WriteLine(to.getString());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Proof proof = new Proof(to);
            Console.WriteLine("Input: " + parsedString);
            Console.WriteLine("Proof: " + proof.getProof());
            Console.ReadKey();
        }
    }
}
