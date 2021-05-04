using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PropositionalFormulaProver.Prover.Node;

namespace PropositionalFormulaProver.Prover
{
    public class Proof
    {
        To currentEquation;
        To terminalTo;
        List<Proof> children = new List<Proof>();
        public bool isTautology = false;

        public Proof(To currentEquation)
        {
            isTautology = false;

            this.currentEquation = currentEquation;

            List<To> resolutions = getResolution();
            if (resolutions.Any())
                foreach (To to in resolutions)
                    children.Add(new Proof(to));

            if (children.Any())
                isTautology = children.Select(proof => proof.isTautology).Count() == children.Count();
            else
                isTautology = checkProvability();
        }

        public string getProof()
        {
            if (!isTautology)
                return "unsolvable";
            if (!children.Any())
                return currentEquation.getString();
            string strRet = @"\infer{" + currentEquation.getString() + "}{";
            if (children.Count() == 1)
                strRet += children[0].getProof();
            else if(children.Count() == 2)
                strRet += children[0].getProof() + " & " + children[1].getProof();
            strRet += "}";
            
            return strRet;
        }

        private bool checkProvability()
        {
            foreach (INode nodeLeft in currentEquation.getLeft().getChildren())
                foreach(INode nodeRight in currentEquation.getRight().getChildren())
                    if(nodeLeft is Atom && nodeRight is Atom)
                        if (((Atom)nodeLeft).getString().Equals(((Atom)nodeRight).getString()))
                        {
                            if(currentEquation.getLeft().getChildren().Count() != 1 ||
                                currentEquation.getRight().getChildren().Count() != 1)
                            {
                                LeftOrRight left = new LeftOrRight();
                                LeftOrRight right = new LeftOrRight();
                                left.appendChild(nodeLeft.deepCopy());
                                right.appendChild(nodeRight.deepCopy());
                                terminalTo = new To(left, right);
                                children.Add(new Proof(terminalTo));
                            }
                            return true;
                        }
            return false;
        }

        private List<To> getResolution()
        {
            List<INode> left = currentEquation.getLeft().getChildren();
            int leftCount = left.Count();
            for(int i = 0; i < leftCount; i++)
            {
                if (left[i] is Conjunction)
                    return getResolutionByLeftConjunction(i);
                else if (left[i] is Disjunction)
                    return getResolutionByLeftDisjunction(i);
                else if (left[i] is Supset)
                    return getResolutionByLeftSupset(i);
                else if (left[i] is Negation)
                    return getResolutionByLeftNegation(i);
            }

            List<INode> right = currentEquation.getRight().getChildren();
            int rightCount = right.Count();
            for(int i = 0; i < rightCount; i++)
            {
                if (right[i] is Conjunction)
                    return getResolutionByRightConjunction(i);
                else if (right[i] is Disjunction)
                    return getResolutionByRightDisjunction(i);
                else if (right[i] is Supset)
                    return getResolutionByRightSupset(i);
                else if (right[i] is Negation)
                    return getResolutionByRightNegation(i);
            }

            return new List<To>();
        }

        private List<To> getResolutionByLeftConjunction(int leftIndex)
        {
            List<INode> nextNodes = new List<INode>();
            Conjunction conjunction = (Conjunction)currentEquation.getLeft().getChildren()[leftIndex];
            nextNodes.Add(conjunction.getLeft());
            nextNodes.Add(conjunction.getRight());

            // Get the modified proof tree.
            To nextEquation = (To)currentEquation.deepCopy();
            nextEquation.getLeft().getChildren().RemoveAt(leftIndex);
            nextEquation.getLeft().getChildren().InsertRange(leftIndex, nextNodes);

            return new List<To>() { nextEquation };
        }

        private List<To> getResolutionByLeftDisjunction(int leftIndex)
        {
            Disjunction disjunction = (Disjunction)currentEquation.getLeft().getChildren()[leftIndex];
            INode nextLeft = disjunction.getLeft();
            INode nextRight = disjunction.getRight();

            // In this case we have two children.
            To nextLeftEquation = (To)currentEquation.deepCopy();
            To nextRightEquation = (To)currentEquation.deepCopy();

            nextLeftEquation.getLeft().getChildren().RemoveAt(leftIndex);
            nextRightEquation.getLeft().getChildren().RemoveAt(leftIndex);

            nextLeftEquation.getLeft().getChildren().Insert(leftIndex, nextLeft);
            nextRightEquation.getLeft().getChildren().Insert(leftIndex, nextRight);

            return new List<To>() { nextLeftEquation, nextRightEquation };
        }

        private List<To> getResolutionByLeftSupset(int leftIndex)
        {
            Supset supset = (Supset)currentEquation.getLeft().getChildren()[leftIndex];
            INode nextLeft = supset.getLeft();
            INode nextRight = supset.getRight();

            // In this case we have two children.
            To nextLeftEquation = (To)currentEquation.deepCopy();
            nextLeftEquation.getLeft().getChildren().RemoveAt(leftIndex);
            nextLeftEquation.getRight().getChildren().Add(nextLeft);

            To nextRightEquation = (To)currentEquation.deepCopy();
            nextRightEquation.getLeft().getChildren().RemoveAt(leftIndex);
            nextRightEquation.getLeft().getChildren().Insert(leftIndex, nextRight);

            return new List<To>() { nextLeftEquation, nextRightEquation };
        }

        private List<To> getResolutionByLeftNegation(int leftIndex)
        {
            Negation negation = (Negation)currentEquation.getLeft().getChildren()[leftIndex];
            INode nextNode = negation.getChild();

            To nextEquation = (To)currentEquation.deepCopy();
            nextEquation.getLeft().getChildren().RemoveAt(leftIndex);
            nextEquation.getRight().getChildren().Add(nextNode);

            return new List<To>() { nextEquation };
        }

        private List<To> getResolutionByRightConjunction(int rightIndex)
        {
            Conjunction conjunction = (Conjunction)currentEquation.getRight().getChildren()[rightIndex];
            INode nextLeftNode = conjunction.getLeft();
            INode nextRightNode = conjunction.getRight();

            To nextLeftEquation = (To)currentEquation.deepCopy();
            nextLeftEquation.getRight().getChildren().RemoveAt(rightIndex);
            nextLeftEquation.getRight().getChildren().Insert(rightIndex, nextLeftNode);

            To nextRightEquation = (To)currentEquation.deepCopy();
            nextRightEquation.getRight().getChildren().RemoveAt(rightIndex);
            nextRightEquation.getRight().getChildren().Insert(rightIndex, nextLeftNode);

            return new List<To>() { nextLeftEquation, nextRightEquation };
        }

        private List<To> getResolutionByRightDisjunction(int rightIndex)
        {
            Disjunction disjunction = (Disjunction)currentEquation.getRight().getChildren()[rightIndex];
            To nextEquation = (To)currentEquation.deepCopy();
            nextEquation.getRight().getChildren().RemoveAt(rightIndex);
            nextEquation.getRight().getChildren().InsertRange(rightIndex, new List<INode> { disjunction.getLeft(), disjunction.getRight() });

            return new List<To>() { nextEquation };
        }

        private List<To> getResolutionByRightSupset(int rightIndex)
        {
            Supset supset = (Supset)currentEquation.getRight().getChildren()[rightIndex];
            To nextEquation = (To)currentEquation.deepCopy();
            nextEquation.getLeft().getChildren().Insert(0, supset.getLeft());
            nextEquation.getRight().getChildren().RemoveAt(rightIndex);
            nextEquation.getRight().getChildren().Insert(rightIndex, supset.getRight());

            return new List<To>() { nextEquation };
        }

        private List<To> getResolutionByRightNegation(int rightIndex)
        {
            Negation negation = (Negation)currentEquation.getRight().getChildren()[rightIndex];
            To nextEquation = (To)currentEquation.deepCopy();
            nextEquation.getRight().getChildren().RemoveAt(rightIndex);
            nextEquation.getLeft().getChildren().Insert(0, negation.getChild());

            return new List<To>() { nextEquation };
        }
    }
}
