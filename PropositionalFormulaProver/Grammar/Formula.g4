grammar Formula;

/*
=======================
    Parser
=======================
*/
top
    : equation EOF
    ;

equation
    : rightOrLeft toOperator rightOrLeft    #To
    ;

rightOrLeft
    : logicalFormula (delimiter logicalFormula)+
    | logicalFormula?
    ;

logicalFormula
    : primitiveFormula                              # Atom          // A propositional variable itself is a formula.
    | '(' logicalFormula ')'                        # Parentheses   // If A is a formula, then (A) is a formula.
    | negOperator logicalFormula                    # Negation      // If A is a formula, then \neg A is a formula.
    | logicalFormula andOperator logicalFormula     # Conjunction   // If A and B are formulas, then A \land B is a formula.
    | logicalFormula orOperator logicalFormula      # Disjunction   // If A and B are formulas, then A \lor B is a formula.
    | logicalFormula supsetOperator logicalFormula  # Supset        // If A and B are formulas, then A -> B is a formula.
    ;

/* primitive formula */
primitiveFormula
    : PropositionalVariable
    ;

toOperator
    : ToSymbol
    ;

/* infix operators */
orOperator
    : OrSymbol
    ;

andOperator
    : AndSymbol
    ;

supsetOperator
    : SupsetSymbol
    ;

/* prefix operators */
negOperator
    : NegSymbol
    ;

/* other symbols */
delimiter
    : Delimiter
    ;

/*
=======================
    Lexer
=======================
*/

/* logical symbols */
ToSymbol
    : '\\to'
    ;

SupsetSymbol
    : '\\supset'
    ;

AndSymbol
    : '\\land'
    ;

OrSymbol
    : '\\lor'
    ;

NegSymbol
    : '\\neg'
    ;

PropositionalVariable
    : [a-zA-Z]
    | [a-zA-Z] '_{' [0-9]+ '}'
    | [a-zA-Z] '_' [0-9]+
    ;

/* other symbols */
Delimiter
    : ','
    ;

WS
    : [ \n\t\r]+ -> skip
    ;