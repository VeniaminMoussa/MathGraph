using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CalculatorInterpreter
{
    public class CalcInterType : CNodeType<CalcInterType.NodeType>
    {
        public CalcInterType(NodeType m_nodeType) : base(m_nodeType) { }

        public enum NodeType
        {
            NT_NA,
            NT_COMPILEUNIT,
            NT_MATHSTATEMENT,
            NT_COMMANDSTATEMENT,
            NT_FUNCTION,
            NT_VARIABLE,
            NT_FINDROOT,
            NT_MAXEXTREME,
            NT_MINEXTREME,
            NT_TANLINE,

            NT_FCALL,
            NT_ADDTION,
            NT_SUBTRACTION,
            NT_DIVISION,
            NT_MULTIPLICATION,
            NT_POWER,
            NT_ABSOLUTE,
            NT_FACTORIAL,
            NT_PAIR,
            NT_ARRAY,
            NT_VALUE,

            NT_PLUS,
            NT_MINUS,

            NT_IDENTIFIER,
            NT_NUMBER,
            NT_CLEAR
        }

        public override NodeType Default()
        {
            return NodeType.NT_NA;
        }

        public override NodeType NA()
        {
            return NodeType.NT_NA;
        }

        public override NodeType Map(int type)
        {
            return (NodeType)type;
        }

        public override int Map(NodeType type)
        {
            return (int)type;
        }
    }

    public abstract class CalcInterASTElement : ASTVisitableElement
    {
        private CalcInterType m_nodeType;

        protected CalcInterASTElement(int context, CalcInterType.NodeType Type) : base(context)
        {
            this.m_nodeType = new CalcInterType(Type);
        }
    }

    //Depends on grammar...
    public class CCompileUnit : CalcInterASTElement
    {
        public const int CT_COMPILEUNIT_MATHSTATEMENT = 0, CT_COMPILEUNIT_COMMANDSTATEMENT = 1;

        public static readonly string[] ContextNames = {
            "COMPILEUNIT_MATHSTATEMENTCONTEXT", "COMPILEUNIT_COMMANDSTATEMENTCONTEXT"
        };

        public CCompileUnit() : base(context: 2, CalcInterType.NodeType.NT_COMPILEUNIT)
        {
            M_GraphVizName = "CCompileUnit" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitCompileUnit(node: this);
            }

            return default(T);
        }
    }

    public class CFindRoot : CalcInterASTElement
    {
        public const int CT_FNAME = 0;
        public const int CT_FUNCTION = 1;

        public static readonly string[] ContextNames = {
             "FINDROOT_FNAMECONTEXT", "FINDROOT_FUNCTIONCONTEXT"
         };

        public CFindRoot() : base(context: 2, CalcInterType.NodeType.NT_FINDROOT)
        {
            M_GraphVizName = "CFindRoot" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitFindRoot(node: this);
            }

            return default(T);
        }
    }

    public class CMaxExtreme : CalcInterASTElement
    {
        public const int CT_FNAME = 0;
        public const int CT_FUNCTION = 1;

        public static readonly string[] ContextNames = {
             "MAXEXTREME_FNAMECONTEXT", "MAXEXTREME_FUNCTIONCONTEXT"
         };

        public CMaxExtreme() : base(context: 2, CalcInterType.NodeType.NT_MAXEXTREME)
        {
            M_GraphVizName = "CMaxExtreme" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitMaxExtreme(node: this);
            }

            return default(T);
        }
    }

    public class CMinExtreme : CalcInterASTElement
    {
        public const int CT_FNAME = 0;
        public const int CT_FUNCTION = 1;

        public static readonly string[] ContextNames = {
             "MINEXTREME_FNAMECONTEXT", "MINEXTREME_FUNCTIONCONTEXT"
         };

        public CMinExtreme() : base(context: 2, CalcInterType.NodeType.NT_MINEXTREME)
        {
            M_GraphVizName = "CMinExtreme" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitMinExtreme(node: this);
            }

            return default(T);
        }
    }

    public class CTanLine : CalcInterASTElement
    {
        public const int CT_FNAME = 0;
        public const int CT_FUNCTION = 1;
        public const int CT_POINT = 2;

        public static readonly string[] ContextNames = {
             "TANLINE_FNAMECONTEXT", "TANLINE_FUNCTIONCONTEXT","TANLINE_POINTCONTEXT"
         };

        public CTanLine() : base(context: 3, CalcInterType.NodeType.NT_TANLINE)
        {
            M_GraphVizName = "CTanLine" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitTanLine(node: this);
            }

            return default(T);
        }
    }

    public class CFunction : CalcInterASTElement
    {
        public const int CT_FNAME = 0;
        public const int CT_BODY = 1;

        public static readonly string[] ContextNames = {
             "FUNCTION_NAMECONTEXT","FUNCTION_BODYCONTEXT"
         };

        public CFunction() : base(context: 2, CalcInterType.NodeType.NT_FUNCTION)
        {
            M_GraphVizName = "CFunction" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitFunction(node: this);
            }

            return default(T);
        }
    }

    public class CFCall : CalcInterASTElement
    {
        public const int CT_FNAME = 0;
        public const int CT_ARGS = 1;

        public static readonly string[] ContextNames = {
             "FCALL_NAMECONTEXT","FCALL_ARGSCONTEXT"
         };

        public CFCall() : base(context: 2, CalcInterType.NodeType.NT_FCALL)
        {
            M_GraphVizName = "CFCall" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitFcall(node: this);
            }

            return default(T);

        }
    }

    public class CAddition : CalcInterASTElement
    {
        public const int CT_LEFT = 0;
        public const int CT_RIGHT = 1;

        public static readonly string[] ContextNames = {
            "ADDITION_LEFTCONTEXT","ADDITION_RIGHTCONTEXT"
         };

        public CAddition() : base(context: 2, CalcInterType.NodeType.NT_ADDTION)
        {
            M_GraphVizName = "CAddition" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitAddition(node: this);
            }

            return default(T);
        }
    }

    public class CSubtraction : CalcInterASTElement
    {
        public const int CT_LEFT = 0;
        public const int CT_RIGHT = 1;

        public static readonly string[] ContextNames = {
            "SUBTRACTION_LEFTCONTEXT","SUBTRACTION_RIGHTCONTEXT"
         };

        public CSubtraction() : base(context: 2, CalcInterType.NodeType.NT_SUBTRACTION)
        {
            M_GraphVizName = "CSubtraction" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitSubtraction(node: this);
            }

            return default(T);
        }
    }

    public class CMultiplication : CalcInterASTElement
    {
        public const int CT_LEFT = 0;
        public const int CT_RIGHT = 1;

        public static readonly string[] ContextNames = {
             "MULTIPLICATION_LEFTCONTEXT","MULTIPLICATION_RIGHTCONTEXT"
         };

        public CMultiplication() : base(context: 2, CalcInterType.NodeType.NT_MULTIPLICATION)
        {
            M_GraphVizName = "CMultiplication" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitMultiplication(node: this);
            }

            return default(T);
        }
    }

    public class CDivision : CalcInterASTElement
    {
        public const int CT_LEFT = 0;
        public const int CT_RIGHT = 1;

        public static readonly string[] ContextNames = {
             "DIVISION_LEFTCONTEXT","DIVISION_RIGHTCONTEXT"
         };

        public CDivision() : base(context: 2, CalcInterType.NodeType.NT_DIVISION)
        {
            M_GraphVizName = "CDivision" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitDivision(node: this);
            }

            return default(T);
        }
    }

    public class CPower : CalcInterASTElement
    {
        public const int CT_LEFT = 0;
        public const int CT_RIGHT = 1;

        public static readonly string[] ContextNames = {
             "POWER_BASE","POWER_EXPONENT"
         };

        public CPower() : base(context: 2, CalcInterType.NodeType.NT_POWER)
        {
            M_GraphVizName = "CPower" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitPower(node: this);
            }

            return default(T);
        }
    }

    public class CAbsolute : CalcInterASTElement
    {
        public const int CT_EXPRESSION = 0;

        public static readonly string[] ContextNames = {
            "ABSOLUTE_EXPRESSIONCONTEXT"
         };

        public CAbsolute() : base(context: 1, CalcInterType.NodeType.NT_ABSOLUTE)
        {
            M_GraphVizName = "CAbsolute" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitAbsolute(node: this);
            }

            return default(T);
        }
    }

    public class CFactorial : CalcInterASTElement
    {
        public const int CT_EXPRESSION = 0;

        public static readonly string[] ContextNames = {
            "FACTORIAL_EXPRESSIONCONTEXT"
         };

        public CFactorial() : base(context: 1, CalcInterType.NodeType.NT_FACTORIAL)
        {
            M_GraphVizName = "CFactorial" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitFactorial(node: this);
            }

            return default(T);
        }
    }

    public class CExprPlus : CalcInterASTElement
    {
        public const int CT_EXPRESSION = 0;

        public static readonly string[] ContextNames = {
             "PLUS_EXPRESSIONCONTEXT"
         };

        public CExprPlus() : base(context: 1, CalcInterType.NodeType.NT_PLUS)
        {
            M_GraphVizName = "CExprPlus" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitExprPlus(node: this);
            }

            return default(T);
        }
    }

    public class CExprMinus : CalcInterASTElement
    {
        public const int CT_EXPRESSION = 0;

        public static readonly string[] ContextNames = {
             "MINUS_EXPRESSIONCONTEXT"
         };

        public CExprMinus() : base(context: 1, CalcInterType.NodeType.NT_MINUS)
        {
            M_GraphVizName = "CExprMinus" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitExprMinus(node: this);
            }

            return default(T);
        }
    }

    public class CVariable : CalcInterASTElement
    {
        public const int CT_FNAME = 0;
        public const int CT_BODY = 1;

        public static readonly string[] ContextNames = {
            "VARIABLE_NAMECONTEXT", "VARIABLE_BODYCONTEXT"
        };

        public CVariable() : base(context: 2, CalcInterType.NodeType.NT_VARIABLE)
        {
            M_GraphVizName = "CVariable" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitVariable(node: this);
            }

            return default(T);

        }
    }

    public class CPair : CalcInterASTElement
    {
        public const int CT_LEFT = 0;
        public const int CT_RIGHT = 1;

        public static readonly string[] ContextNames = {
            "PAIR_LEFTCONTEXT", "PAIR_RIGHTCONTEXT"
        };

        public CPair() : base(context: 2, CalcInterType.NodeType.NT_PAIR)
        {
            M_GraphVizName = "CPair" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitPair(node: this);
            }

            return default(T);

        }
    }

    public class CValueArray : CalcInterASTElement
    {
        public const int CT_BODY = 0;
        private string left_close;
        public string Left_close => left_close;
        private string right_close;
        public string Right_close => right_close;
        public string close;

        public static readonly string[] ContextNames = {
            "ARRAY_BODYCONTEXT"
        };

        public CValueArray() : base(context: 1, CalcInterType.NodeType.NT_ARRAY)
        {
            M_GraphVizName = "CValueArray" + M_GraphVizName;
        }

        public void setLeft(String left)
        {
            this.left_close = left;
            close = left_close + right_close;
        }

        public void setRight(String right)
        {
            this.right_close = right;
            close = left_close + right_close;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitValueArray(node: this);
            }

            return default(T);

        }
    }

    public class CValuePlus : CalcInterASTElement
    {
        public const int CT_VALUE = 0;

        public static readonly string[] ContextNames = {
             "PLUS_VALUECONTEXT"
         };

        public CValuePlus() : base(context: 1, CalcInterType.NodeType.NT_PLUS)
        {
            M_GraphVizName = "CValuePlus" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitValuePlus(node: this);
            }

            return default(T);
        }
    }

    public class CValueMinus : CalcInterASTElement
    {
        public const int CT_VALUE = 0;

        public static readonly string[] ContextNames = {
             "MINUS_VALUECONTEXT"
         };

        public CValueMinus() : base(context: 1, CalcInterType.NodeType.NT_MINUS)
        {
            M_GraphVizName = "CValueMinus" + M_GraphVizName;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitValueMinus(node: this);
            }

            return default(T);
        }
    }

    public class CNumber : CalcInterASTElement
    {
        public CNumber(string Number) : base(context: 0, CalcInterType.NodeType.NT_NUMBER)
        {
            M_Name = Number;
            M_GraphVizName = "CNumber" + M_GraphVizName + "_" + Number;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T>? visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitNUMBER(node: this);
            }

            return default(T);
        }
    }

    public class CIdentifier : CalcInterASTElement
    {
        public CIdentifier(string label) : base(context: 0, CalcInterType.NodeType.NT_IDENTIFIER)
        {
            M_Name = label;
            M_GraphVizName = "CIdentifier" + M_GraphVizName + "_" + label;
        }

        public override T Accept<T>(ASTBaseVisitor<T> visitor)
        {
            CalcInterASTBaseVisitor<T> visitor_ = visitor as CalcInterASTBaseVisitor<T>;

            if (visitor_ != null)
            {
                return visitor_.VisitIDENTIFIER(node: this);
            }

            return default(T);
        }
    }
}
