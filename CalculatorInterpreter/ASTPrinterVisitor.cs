using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorInterpreter
{
    public class ASTPrinterVisitor : CalcInterASTBaseVisitor<int>
    {
        private static int m_clusterSerial = 0;
        private StreamWriter m_ostream;
        private string m_dotName;

        // Constructor
        public ASTPrinterVisitor(string dotFileName)
        {
            m_ostream = new StreamWriter(dotFileName);
            m_dotName = dotFileName;
        }

        private void ExtractSubgraphs(ASTElement node, int context, string[] contextNames)
        {
            if (node.GetChildrenContextNumber(context) != 0)
            {
                m_ostream.WriteLine("\tsubgraph cluster" + m_clusterSerial++ + "{");
                m_ostream.WriteLine("\t\tnode [style=filled,color=white];");
                m_ostream.WriteLine("\t\tstyle=filled;");
                m_ostream.WriteLine("\t\tcolor=lightgrey;");
                m_ostream.Write("\t\t");

                foreach (ASTElement ln in node.GetChildrenContext(context))
                {
                    m_ostream.Write(ln.M_GraphVizName + ";");
                }

                m_ostream.WriteLine("\n\t\tlabel=" + contextNames[context] + ";");
                m_ostream.WriteLine("\t}");
            }
        }

        public override int VisitCompileUnit(CCompileUnit node)
        {
            m_ostream.WriteLine("digraph {");

            ExtractSubgraphs(node, CCompileUnit.CT_COMPILEUNIT_COMMANDSTATEMENT, CCompileUnit.ContextNames);
            ExtractSubgraphs(node, CCompileUnit.CT_COMPILEUNIT_MATHSTATEMENT, CCompileUnit.ContextNames);

            base.VisitCompileUnit(node);

            m_ostream.WriteLine("}");
            m_ostream.Close();

            // Prepare the process to run
            ProcessStartInfo start = new ProcessStartInfo();
            // Enter in the command line arguments, everything you would enter after the executable name itself
            start.Arguments = "-Tpng " + m_dotName + " -o" + m_dotName + ".png";
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

        public override int VisitFindRoot(CFindRoot node)
        {
            ExtractSubgraphs(node, CFindRoot.CT_FNAME, CFindRoot.ContextNames);
            ExtractSubgraphs(node, CFindRoot.CT_FUNCTION, CFindRoot.ContextNames);
            base.VisitFindRoot(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitMaxExtreme(CMaxExtreme node)
        {
            ExtractSubgraphs(node, CMaxExtreme.CT_FNAME, CMaxExtreme.ContextNames);
            ExtractSubgraphs(node, CMaxExtreme.CT_FUNCTION, CMaxExtreme.ContextNames);

            base.VisitMaxExtreme(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitMinExtreme(CMinExtreme node)
        {
            ExtractSubgraphs(node, CMinExtreme.CT_FNAME, CMinExtreme.ContextNames);
            ExtractSubgraphs(node, CMinExtreme.CT_FUNCTION, CMinExtreme.ContextNames);

            base.VisitMinExtreme(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitTanLine(CTanLine node)
        {
            ExtractSubgraphs(node, CTanLine.CT_FNAME, CTanLine.ContextNames);
            ExtractSubgraphs(node, CTanLine.CT_FUNCTION, CTanLine.ContextNames);
            ExtractSubgraphs(node, CTanLine.CT_POINT, CTanLine.ContextNames);

            base.VisitTanLine(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitFunction(CFunction node)
        {
            ExtractSubgraphs(node, CFunction.CT_FNAME, CFunction.ContextNames);
            ExtractSubgraphs(node, CFunction.CT_BODY, CFunction.ContextNames);

            base.VisitFunction(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitFcall(CFCall node)
        {
            ExtractSubgraphs(node, CFCall.CT_FNAME, CFCall.ContextNames);
            ExtractSubgraphs(node, CFCall.CT_ARGS, CFCall.ContextNames);

            base.VisitFcall(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitFactorial(CFactorial node)
        {
            ExtractSubgraphs(node, CFactorial.CT_EXPRESSION, CFactorial.ContextNames);

            base.VisitFactorial(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitAbsolute(CAbsolute node)
        {
            ExtractSubgraphs(node, CAbsolute.CT_EXPRESSION, CAbsolute.ContextNames);

            base.VisitAbsolute(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitPower(CPower node)
        {
            ExtractSubgraphs(node, CPower.CT_LEFT, CPower.ContextNames);
            ExtractSubgraphs(node, CPower.CT_RIGHT, CPower.ContextNames);

            base.VisitPower(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitMultiplication(CMultiplication node)
        {
            ExtractSubgraphs(node, CMultiplication.CT_LEFT, CMultiplication.ContextNames);
            ExtractSubgraphs(node, CMultiplication.CT_RIGHT, CMultiplication.ContextNames);

            base.VisitMultiplication(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitDivision(CDivision node)
        {
            ExtractSubgraphs(node, CDivision.CT_LEFT, CDivision.ContextNames);
            ExtractSubgraphs(node, CDivision.CT_RIGHT, CDivision.ContextNames);

            base.VisitDivision(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitAddition(CAddition node)
        {
            ExtractSubgraphs(node, CAddition.CT_LEFT, CAddition.ContextNames);
            ExtractSubgraphs(node, CAddition.CT_RIGHT, CAddition.ContextNames);

            base.VisitAddition(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitSubtraction(CSubtraction node)
        {
            ExtractSubgraphs(node, CSubtraction.CT_LEFT, CSubtraction.ContextNames);
            ExtractSubgraphs(node, CSubtraction.CT_RIGHT, CSubtraction.ContextNames);

            base.VisitSubtraction(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitExprPlus(CExprPlus node)
        {
            ExtractSubgraphs(node, CExprPlus.CT_EXPRESSION, CExprPlus.ContextNames);

            base.VisitExprPlus(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitExprMinus(CExprMinus node)
        {
            ExtractSubgraphs(node, CExprMinus.CT_EXPRESSION, CExprMinus.ContextNames);

            base.VisitExprMinus(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitVariable(CVariable node)
        {
            ExtractSubgraphs(node, CVariable.CT_FNAME, CVariable.ContextNames);
            ExtractSubgraphs(node, CVariable.CT_BODY, CVariable.ContextNames);

            base.VisitVariable(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitPair(CPair node)
        {
            ExtractSubgraphs(node, CPair.CT_LEFT, CPair.ContextNames);
            ExtractSubgraphs(node, CPair.CT_RIGHT, CPair.ContextNames);

            base.VisitPair(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitValueArray(CValueArray node)
        {
            ExtractSubgraphs(node, CValueArray.CT_BODY, CValueArray.ContextNames);

            base.VisitValueArray(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitValuePlus(CValuePlus node)
        {
            ExtractSubgraphs(node, CValuePlus.CT_VALUE, CValuePlus.ContextNames);

            base.VisitValuePlus(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitValueMinus(CValueMinus node)
        {
            ExtractSubgraphs(node, CValueMinus.CT_VALUE, CValueMinus.ContextNames);

            base.VisitValueMinus(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitIDENTIFIER(CIdentifier node)
        {
            base.VisitIDENTIFIER(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }

        public override int VisitNUMBER(CNumber node)
        {
            base.VisitNUMBER(node);

            m_ostream.WriteLine("{0}->{1}", currentParent.M_GraphVizName, node.M_GraphVizName);

            return 0;
        }
    }
}
