using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Misc;
using PropositionalFormulaProver.Grammar;
using PropositionalFormulaProver.Prover.Node;

namespace PropositionalFormulaProver.Prover
{
    public class FormulaListener: FormulaBaseListener
    {
        Stack<INode> stack = new Stack<INode>();
        bool isLeftFinished = false;
        public override void ExitAtom([NotNull] FormulaParser.AtomContext context)
        {
            Console.WriteLine("ExitAtom start, stack count = " + stack.Count());
            base.ExitAtom(context);
            stack.Push(new Atom(context.GetText()));
        }
        public override void ExitNegation([NotNull] FormulaParser.NegationContext context)
        {
            Console.WriteLine("ExitNegation start, stack count = " + stack.Count());
            base.ExitNegation(context);
            Negation negation = new Negation(stack.Pop());
            stack.Push(negation);
        }
        public override void ExitTo([NotNull] FormulaParser.ToContext context)
        {
            Console.WriteLine("ExitTo start, stack count = " + stack.Count());
            base.EnterTo(context);
            LeftOrRight right = (LeftOrRight)stack.Pop();
            LeftOrRight left = (LeftOrRight)stack.Pop();
            To to = new To(left, right);
            stack.Push(to);
        }
        public override void ExitSupset([NotNull] FormulaParser.SupsetContext context)
        {
            Console.WriteLine("ExitSupset start, stack count = " + stack.Count());
            base.ExitSupset(context);
            Supset supset = new Supset(stack.Pop(), stack.Pop());
            stack.Push(supset);
        }
        public override void ExitRightOrLeft([NotNull] FormulaParser.RightOrLeftContext context)
        {
            Console.WriteLine("ExitRightOrLeft start, stack count = " + stack.Count());
            base.ExitRightOrLeft(context);
            LeftOrRight lr = new LeftOrRight();

            if (isLeftFinished)
            {
                int stackCount = stack.Count();
                for (int i = 0; i < stackCount - 1; i++)
                    lr.appendChild(stack.Pop());
            }
            else
            {
                while (stack.Any())
                    lr.appendChild(stack.Pop());
                isLeftFinished = true;
            }
            
            stack.Push(lr);
        }
        public override void ExitDisjunction([NotNull] FormulaParser.DisjunctionContext context)
        {
            Console.WriteLine("ExitDisjunction start, stack count = " + stack.Count());
            base.ExitDisjunction(context);
            Disjunction disjunction = new Disjunction(stack.Pop(), stack.Pop());
            stack.Push(disjunction);
        }
        public override void ExitConjunction([NotNull] FormulaParser.ConjunctionContext context)
        {
            Console.WriteLine("ExitConjunction start, stack count = " + stack.Count());
            base.ExitConjunction(context);
            Conjunction disjunction = new Conjunction(stack.Pop(), stack.Pop());
            stack.Push(disjunction);
        }
        public INode getTree() => stack.Pop();
    }
}
