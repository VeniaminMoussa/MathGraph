using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CalculatorInterpreter.GrammarParser;

namespace CalculatorInterpreter
{
    public class ASTGenerator : GrammarBaseVisitor<double>
    {
        public Library library;
        private CCompileUnit m_root;//Represent the root node of the Abstract Syntax Tree
        private Stack<ValueTuple<CalcInterASTElement, int>> m_parents;

        public CCompileUnit M_Root => m_root;

        public ASTGenerator(Library lib)
        {
            library = lib;
            m_parents = new Stack<ValueTuple<CalcInterASTElement, int>>();
        }

        public override double VisitCompileUnit(GrammarParser.CompileUnitContext context)
        {
            CCompileUnit newNode = new CCompileUnit();
            m_root = newNode;

            if (context.children != null)
            {
                foreach (var statementContext in context.children)
                {
                    if (statementContext.GetType().Name.ToLower().Contains("mathstatementcontext"))
                    {
                        m_parents.Push((m_root, CCompileUnit.CT_COMPILEUNIT_MATHSTATEMENT));
                        base.Visit(statementContext);
                        m_parents.Pop();
                    }
                    else
                    {
                        m_parents.Push((m_root, CCompileUnit.CT_COMPILEUNIT_COMMANDSTATEMENT));
                        base.Visit(statementContext);
                        m_parents.Pop();
                    }
                }
            }

            return 0;
        }

        public override double VisitCommand_FindRoot(GrammarParser.Command_FindRootContext context)
        {
            CFindRoot newNode = new CFindRoot();
            ValueTuple<CalcInterASTElement, int> parent = m_parents.Peek();
            parent.Item1.AddChild(newNode, parent.Item2);

            if (context.IDENTIFIER() != null)
            {
                m_parents.Push((newNode, CFindRoot.CT_FNAME));
                Visit(tree: context.IDENTIFIER());
                m_parents.Pop();
            }
            else if (context.function() != null)
            {
                m_parents.Push((newNode, CFindRoot.CT_FUNCTION));
                Visit(tree: context.function());
                m_parents.Pop();
            }

            return 0;
        }

        public override double VisitCommand_TanLine(GrammarParser.Command_TanLineContext context)
        {
            CTanLine newNode = new CTanLine();
            ValueTuple<CalcInterASTElement, int> parent2 = m_parents.Peek();
            parent2.Item1.AddChild(newNode, parent2.Item2);

            if (context.IDENTIFIER() != null)
            {
                m_parents.Push((newNode, CTanLine.CT_FNAME));
                Visit(tree: context.IDENTIFIER());
                m_parents.Pop();
            }
            else if (context.function() != null)
            {
                m_parents.Push((newNode, CTanLine.CT_FUNCTION));
                Visit(tree: context.function());
                m_parents.Pop();
            }

            m_parents.Push((newNode, CTanLine.CT_POINT));
            Visit(tree: context.value());
            m_parents.Pop();
            
            return 0;
        }

        public override double VisitCommand_MinExtreme(GrammarParser.Command_MinExtremeContext context)
        {
            CMinExtreme newNode = new CMinExtreme();
            ValueTuple<CalcInterASTElement, int> parent = m_parents.Peek();
            parent.Item1.AddChild(newNode, parent.Item2);

            if (context.IDENTIFIER() != null)
            {
                m_parents.Push((newNode, CMinExtreme.CT_FNAME));
                Visit(tree: context.IDENTIFIER());
                m_parents.Pop();
            }
            else if (context.function() != null)
            {
                m_parents.Push((newNode, CMinExtreme.CT_FUNCTION));
                Visit(tree: context.function());
                m_parents.Pop();
            }

            return 0;
        }

        public override double VisitCommand_MaxExtreme(GrammarParser.Command_MaxExtremeContext context)
        {
            CMaxExtreme newNode = new CMaxExtreme();
            ValueTuple<CalcInterASTElement, int> parent = m_parents.Peek();
            parent.Item1.AddChild(newNode, parent.Item2);

            if (context.IDENTIFIER() != null)
            {
                m_parents.Push((newNode, CMaxExtreme.CT_FNAME));
                Visit(tree: context.IDENTIFIER());
                m_parents.Pop();
            }
            else if (context.function() != null)
            {
                m_parents.Push((newNode, CMaxExtreme.CT_FUNCTION));
                Visit(tree: context.function());
                m_parents.Pop();
            }

            return 0;
        }

        public override double VisitFunction(GrammarParser.FunctionContext context)
        {
            CFunction newNode = new CFunction();
            ValueTuple<CalcInterASTElement, int> parent = m_parents.Peek();
            parent.Item1.AddChild(newNode, parent.Item2);

            if (context.IDENTIFIER() != null)
            {
                m_parents.Push((newNode, CFunction.CT_FNAME));
                Visit(tree: context.IDENTIFIER());
                m_parents.Pop();
            }

            m_parents.Push((newNode, CFunction.CT_BODY));
            Visit(tree: context.expression());
            m_parents.Pop();

            return 0;
        }

        public override double VisitExpr_Fcall(GrammarParser.Expr_FcallContext context)
        {
            CFCall newNode = new CFCall();
            ValueTuple<CalcInterASTElement, int> parent = m_parents.Peek();
            parent.Item1.AddChild(newNode, parent.Item2);

            m_parents.Push((newNode, CFCall.CT_FNAME));
            Visit(tree: context.IDENTIFIER());
            m_parents.Pop();

            if (context.args() != null)
            {
                m_parents.Push((newNode, CFCall.CT_ARGS));
                Visit(tree: context.args());
                m_parents.Pop();
            }

            return 0;
        }

        public override double VisitExpr_Factorial(GrammarParser.Expr_FactorialContext context)
        {
            CFactorial newNode = new CFactorial();
            ValueTuple<CalcInterASTElement, int> parent = m_parents.Peek();
            parent.Item1.AddChild(newNode, parent.Item2);

            m_parents.Push((newNode, CFactorial.CT_EXPRESSION));
            Visit(tree: context.expression());
            m_parents.Pop();

            return 0;
        }

        public override double VisitExpr_Abs(GrammarParser.Expr_AbsContext context)
        {
            CAbsolute newNode = new CAbsolute();
            ValueTuple<CalcInterASTElement, int> parent = m_parents.Peek();
            parent.Item1.AddChild(newNode, parent.Item2);

            m_parents.Push((newNode, CAbsolute.CT_EXPRESSION));
            Visit(tree: context.expression());
            m_parents.Pop();

            return 0;
        }

        public override double VisitExpr_Power(GrammarParser.Expr_PowerContext context)
        {
            CPower newNode = new CPower();
            ValueTuple<CalcInterASTElement, int> parent = m_parents.Peek();
            parent.Item1.AddChild(newNode, parent.Item2);

            m_parents.Push((newNode, CPower.CT_LEFT));
            Visit(tree: context.expression(0));
            m_parents.Pop();

            m_parents.Push((newNode, CPower.CT_RIGHT));
            Visit(tree: context.expression(1));
            m_parents.Pop();

            return 0;
        }

        public override double VisitExpr_MultiplicationOrDivision(GrammarParser.Expr_MultiplicationOrDivisionContext context)
        {
            switch (context.op.Type)
            {
                case GrammarLexer.MULT:
                    CMultiplication newNode1 = new CMultiplication();
                    ValueTuple<CalcInterASTElement, int> parent1 = m_parents.Peek();
                    parent1.Item1.AddChild(newNode1, parent1.Item2);

                    m_parents.Push((newNode1, CMultiplication.CT_LEFT));
                    Visit(tree: context.expression(i: 0));
                    m_parents.Pop();

                    m_parents.Push((newNode1, CMultiplication.CT_RIGHT));
                    Visit(tree: context.expression(i: 1));
                    m_parents.Pop();
                    break;
                case GrammarLexer.DIV:
                    CDivision newNode2 = new CDivision();
                    ValueTuple<CalcInterASTElement, int> parent2 = m_parents.Peek();
                    parent2.Item1.AddChild(newNode2, parent2.Item2);

                    m_parents.Push((newNode2, CDivision.CT_LEFT));
                    Visit(tree: context.expression(i: 0));
                    m_parents.Pop();

                    m_parents.Push((newNode2, CDivision.CT_RIGHT));
                    Visit(tree: context.expression(i: 1));
                    m_parents.Pop();
                    break;
                default:
                    break;
            }

            return 0;
        }

        public override double VisitExpr_AdditionOrSubtraction(GrammarParser.Expr_AdditionOrSubtractionContext context)
        {
            switch (context.op.Type)
            {
                case GrammarLexer.PLUS:
                    CAddition newNode1 = new CAddition();
                    ValueTuple<CalcInterASTElement, int> parent1 = m_parents.Peek();
                    parent1.Item1.AddChild(newNode1, parent1.Item2);

                    m_parents.Push((newNode1, CAddition.CT_LEFT));
                    Visit(tree: context.expression(i: 0));
                    m_parents.Pop();

                    m_parents.Push((newNode1, CAddition.CT_RIGHT));
                    Visit(tree: context.expression(i: 1));
                    m_parents.Pop();

                    break;
                case GrammarLexer.MINUS:
                    CSubtraction newNode2 = new CSubtraction();
                    ValueTuple<CalcInterASTElement, int> parent2 = m_parents.Peek();
                    parent2.Item1.AddChild(newNode2, parent2.Item2);

                    m_parents.Push((newNode2, CSubtraction.CT_LEFT));
                    Visit(tree: context.expression(i: 0));
                    m_parents.Pop();

                    m_parents.Push((newNode2, CSubtraction.CT_RIGHT));
                    Visit(tree: context.expression(i: 1));
                    m_parents.Pop();
                    break;
                default:
                    break;
            }

            return 0;
        }

        public override double VisitExpr_PlusOrMinus(GrammarParser.Expr_PlusOrMinusContext context)
        {
            switch (context.op.Type)
            {
                case GrammarLexer.PLUS:
                    CExprPlus newNode1 = new CExprPlus();
                    ValueTuple<CalcInterASTElement, int> parent1 = m_parents.Peek();
                    parent1.Item1.AddChild(newNode1, parent1.Item2);

                    m_parents.Push((newNode1, CExprPlus.CT_EXPRESSION));
                    Visit(tree: context.expression());
                    m_parents.Pop();
                    break;
                case GrammarLexer.MINUS:
                    CExprMinus newNode2 = new CExprMinus();
                    ValueTuple<CalcInterASTElement, int> parent2 = m_parents.Peek();
                    parent2.Item1.AddChild(newNode2, parent2.Item2);

                    m_parents.Push((newNode2, CExprMinus.CT_EXPRESSION));
                    Visit(tree: context.expression());
                    m_parents.Pop();
                    break;
                default:
                    break;
            }

            return 0;
        }

        public override double VisitVariable(GrammarParser.VariableContext context)
        {
            CVariable newNode = new CVariable();
            ValueTuple<CalcInterASTElement, int> parent = m_parents.Peek();
            parent.Item1.AddChild(newNode, parent.Item2);

            m_parents.Push((newNode, CVariable.CT_FNAME));
            Visit(tree: context.IDENTIFIER());
            m_parents.Pop();

            m_parents.Push((newNode, CVariable.CT_BODY));
            Visit(tree: context.@object());
            m_parents.Pop();

            return 0;
        }

        public override double VisitPair(GrammarParser.PairContext context)
        {
            CPair newNode = new CPair();
            ValueTuple<CalcInterASTElement, int> parent = m_parents.Peek();
            parent.Item1.AddChild(newNode, parent.Item2);

            m_parents.Push((newNode, CPair.CT_LEFT));
            Visit(tree: context.IDENTIFIER());
            m_parents.Pop();

            m_parents.Push((newNode, CPair.CT_RIGHT));
            Visit(tree: context.value());
            m_parents.Pop();

            return 0;
        }

        public override double VisitValue_Array(GrammarParser.Value_ArrayContext context)
        {
            CValueArray newNode = new CValueArray();

            switch (context.left_op.Type)
            {
                case GrammarLexer.LP:
                    newNode.setLeft("(");
                    break;
                case GrammarLexer.LSB:
                    newNode.setLeft("[");
                    break;
                default:
                    break;
            }

            switch (context.right_op.Type)
            {
                case GrammarLexer.RP:
                    newNode.setRight(")");
                    break;
                case GrammarLexer.RSB:
                    newNode.setRight("]");
                    break;
                default:
                    break;
            }

            ValueTuple<CalcInterASTElement, int> parent = m_parents.Peek();
            parent.Item1.AddChild(newNode, parent.Item2);

            for (int i = 0; i < context.ChildCount; i++)
            {
                m_parents.Push((newNode, CValueArray.CT_BODY));
                Visit(tree: context.GetChild(i));
                m_parents.Pop();
            }

            return 0;
        }

        public override double VisitValue_PlusOrMinus(GrammarParser.Value_PlusOrMinusContext context)
        {
            switch (context.op.Type)
            {
                case GrammarLexer.PLUS:
                    CValuePlus newNode1 = new CValuePlus();
                    ValueTuple<CalcInterASTElement, int> parent1 = m_parents.Peek();
                    parent1.Item1.AddChild(newNode1, parent1.Item2);

                    m_parents.Push((newNode1, CValuePlus.CT_VALUE));
                    Visit(tree: context.value());
                    m_parents.Pop();
                    break;
                case GrammarLexer.MINUS:
                    CValueMinus newNode2 = new CValueMinus();
                    ValueTuple<CalcInterASTElement, int> parent2 = m_parents.Peek();
                    parent2.Item1.AddChild(newNode2, parent2.Item2);

                    m_parents.Push((newNode2, CValueMinus.CT_VALUE));
                    Visit(tree: context.value());
                    m_parents.Pop();
                    break;
                default:
                    break;
            }

            return 0;
        }

        public override double VisitTerminal(ITerminalNode node)
        {
            switch (node.Symbol.Type)
            {
                case GrammarLexer.IDENTIFIER:
                    {
                        CalcInterASTElement newIdentifier = new CIdentifier(node.Symbol.Text);
                        ValueTuple<CalcInterASTElement, int> parent1 = m_parents.Peek();
                        parent1.Item1.AddChild(newIdentifier, parent1.Item2);
                    }
                    break;
                case GrammarLexer.NUMBER:
                    {
                        CNumber newNumber = new CNumber(node.Symbol.Text);
                        ValueTuple<CalcInterASTElement, int> parent2 = m_parents.Peek();
                        parent2.Item1.AddChild(newNumber, parent2.Item2);
                    }
                    break;
                case GrammarLexer.CLEAR:
                    {
                        m_root.ClearChildren();
                        library.clearLibrary();
                    }
                    break;
                default:
                    break;
            }

            return 0;
        }
    }
}
