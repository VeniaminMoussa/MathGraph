using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace CalculatorInterpreter
{
    public class STPrinterVisitor : GrammarBaseVisitor<int>
    {
        private Stack<string> m_labels = new Stack<string>();
        private StreamWriter outFile = new StreamWriter(path: "test.dot");
        private static int ms_serialCounter = 0;

        public override int VisitCompileUnit(GrammarParser.CompileUnitContext context)
        {
            string s = "CompileUnit_" + ms_serialCounter++;

            outFile.WriteLine("digraph G{");
            m_labels.Push(s);
            base.VisitCompileUnit(context);
            m_labels.Pop();
            outFile.WriteLine("}");
            outFile.Close();

            // Prepare the process dot to run
            ProcessStartInfo start = new ProcessStartInfo();
            // Enter in the command line arguments, everything you would enter after the executable name itself
            start.Arguments = " -Tpng " +
                              Path.GetFileName(path: "test.dot") + " -o " +
                              Path.GetFileNameWithoutExtension("test") + ".png";
            // Enter the executable to run, including the complete path
            start.FileName = "dot";
            // Do you want to show a console window?
            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;
            int exitCode;

            // Run the external process & wait for it to finish
            using (Process proc = Process.Start(start))
            {
                proc.WaitForExit();

                // Retrieve the app's exit code
                exitCode = proc.ExitCode;
            }
            return 0;
        }

        public override int VisitCommandStatement(GrammarParser.CommandStatementContext context)
        {
            string s = "CommandStatement_" + ms_serialCounter++;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitCommandStatement(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitMathStatement(GrammarParser.MathStatementContext context)
        {
            string s = "MathStatement_" + ms_serialCounter++;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitMathStatement(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitCommand_FindRoot(GrammarParser.Command_FindRootContext context)
        {
            int serial = ms_serialCounter++;
            string s = "Command_FindRoot_" + serial;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitChildren(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitCommand_MinExtreme(GrammarParser.Command_MinExtremeContext context)
        {
            int serial = ms_serialCounter++;
            string s = "Command_MinExtreme_" + serial;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitChildren(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitCommand_MaxExtreme(GrammarParser.Command_MaxExtremeContext context)
        {
            int serial = ms_serialCounter++;
            string s = "Command_MaxExtreme_" + serial;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitChildren(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitCommand_TanLine(GrammarParser.Command_TanLineContext context)
        {
            int serial = ms_serialCounter++;
            string s = "Command_TanLine_" + serial;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitChildren(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitFunction(GrammarParser.FunctionContext context)
        {
            string s = "Function_" + ms_serialCounter++;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitFunction(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitExpression(GrammarParser.ExpressionContext context)
        {
            string s = "Expression_" + ms_serialCounter++;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitExpression(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitExpr_Fcall(GrammarParser.Expr_FcallContext context)
        {
            string s = "FCall_" + ms_serialCounter++;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitExpr_Fcall(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitArgs(GrammarParser.ArgsContext context)
        {
            string s = "Args_" + ms_serialCounter++;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitArgs(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitExpr_Power(GrammarParser.Expr_PowerContext context)
        {
            string s = "Expr_Power_" + ms_serialCounter++;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitExpr_Power(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitExpr_Parenthesis(GrammarParser.Expr_ParenthesisContext context)
        {
            string s = "Expr_Parenthesis_" + ms_serialCounter++;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitExpr_Parenthesis(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitExpr_Abs(GrammarParser.Expr_AbsContext context)
        {
            string s = "FAbs_" + ms_serialCounter++;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitExpr_Abs(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitExpr_Factorial(GrammarParser.Expr_FactorialContext context)
        {
            string s = "Expr_Factorial_" + ms_serialCounter++;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitExpr_Factorial(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitExpr_MultiplicationOrDivision(GrammarParser.Expr_MultiplicationOrDivisionContext context)
        {
            string s = "";
            switch (context.op.Type)
            {
                case GrammarParser.MULT:
                    s = "Expr_Multiplication_" + ms_serialCounter++;
                    // Preorder action
                    outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);
                    break;
                case GrammarParser.DIV:
                    s = "Expr_Division_" + ms_serialCounter++;
                    // Preorder action
                    outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);
                    break;
                default:
                    break;
            }

            m_labels.Push(s);

            base.VisitExpr_MultiplicationOrDivision(context);

            m_labels.Pop();

            return 0;
        }

        public override int VisitExpr_AdditionOrSubtraction(GrammarParser.Expr_AdditionOrSubtractionContext context)
        {
            string s = "";
            switch (context.op.Type)
            {
                case GrammarParser.PLUS:
                    s = "Expr_Addition_" + ms_serialCounter++;
                    // Preorder action
                    outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);
                    break;
                case GrammarParser.MINUS:
                    s = "Expr_Substraction_" + ms_serialCounter++;
                    // Preorder action
                    outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);
                    break;
                default:
                    break;
            }
            m_labels.Push(s);

            base.VisitExpr_AdditionOrSubtraction(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitExpr_PlusOrMinus(GrammarParser.Expr_PlusOrMinusContext context)
        {
            string s = "";
            switch (context.op.Type)
            {
                case GrammarParser.PLUS:
                    s = "Expr_Plus_" + ms_serialCounter++;
                    // Preorder action
                    outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);
                    break;
                case GrammarParser.MINUS:
                    s = "Expr_Minus_" + ms_serialCounter++;
                    // Preorder action
                    outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);
                    break;
                default:
                    break;
            }
            m_labels.Push(s);

            base.VisitExpr_PlusOrMinus(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitVariable(GrammarParser.VariableContext context)
        {
            string s = "Variable_" + ms_serialCounter++;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitVariable(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitObject(GrammarParser.ObjectContext context)
        {
            int serial = ms_serialCounter++;
            string s = "Object_" + serial;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitObject(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitPairList(GrammarParser.PairListContext context)
        {
            string s = "PairList_" + ms_serialCounter++;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitPairList(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitPair(GrammarParser.PairContext context)
        {
            string s = "Pair_" + ms_serialCounter++;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitPair(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitValue(GrammarParser.ValueContext context)
        {
            string s = "Value_" + ms_serialCounter++;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitValue(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitValue_PlusOrMinus(GrammarParser.Value_PlusOrMinusContext context)
        {
            string s = "";
            switch (context.op.Type)
            {
                case GrammarParser.PLUS:
                    s = "Value_Plus_" + ms_serialCounter++;
                    // Preorder action
                    outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);
                    break;
                case GrammarParser.MINUS:
                    s = "Value_Minus_" + ms_serialCounter++;
                    // Preorder action
                    outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);
                    break;
                default:
                    break;
            }
            m_labels.Push(s);

            base.VisitValue_PlusOrMinus(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitValue_Array(GrammarParser.Value_ArrayContext context)
        {
            string s = "Value_Array_" + ms_serialCounter++;
            outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);

            m_labels.Push(s);

            base.VisitValue_Array(context);

            m_labels.Pop();
            return 0;
        }

        public override int VisitTerminal(ITerminalNode node)
        {
            int serial = ms_serialCounter++;
            string s = "";
            switch (node.Symbol.Type)
            {
                case GrammarParser.NUMBER:
                    s = "Number_" + serial;
                    // Preorder action
                    outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);
                    break;
                case GrammarParser.IDENTIFIER:
                    s = "Identifier_" + serial;
                    // Preorder action
                    outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);
                    break;
                case GrammarParser.CLEAR:
                    s = "Clear_" + serial;
                    // Preorder action
                    outFile.WriteLine("\"{0}\"->\"{1}\";", m_labels.Peek(), s);
                    break;
                default:
                    break;
            }

            return base.VisitTerminal(node);
        }
    }
}
