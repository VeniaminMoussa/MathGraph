using Antlr4.Runtime.Tree;
using Antlr4.Runtime;

namespace CalculatorInterpreter
{
    public class Program
    {
        public Library library;

        public Program()
        {
            library = new Library();
        }

        public Library Begin(string input)
        {
            AntlrInputStream antlrInputStream = new AntlrInputStream(input);

            GrammarLexer lexer = new GrammarLexer(antlrInputStream);

            CommonTokenStream tokens = new CommonTokenStream(lexer);

            GrammarParser parser = new GrammarParser(tokens);

            IParseTree tree = parser.compileUnit();// Begin Syntax Analysis

            Console.WriteLine(tree.ToStringTree());

            STPrinterVisitor stPrinter = new STPrinterVisitor();
            stPrinter.Visit(tree);

            ASTGenerator astGenerator = new ASTGenerator(library);

            astGenerator.Visit(tree);

            ASTPrinterVisitor astPrinterVisitor = new ASTPrinterVisitor("test.ast.dot");
            astPrinterVisitor.Visit(astGenerator.M_Root);

            ASTEvalVisitor cMathVisitor = new ASTEvalVisitor(library);
            cMathVisitor.Visit(astGenerator.M_Root);

            return library;
        }
    }
}