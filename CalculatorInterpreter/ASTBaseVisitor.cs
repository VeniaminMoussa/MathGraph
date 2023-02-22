using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorInterpreter
{
    public abstract class ASTBaseVisitor<T>
    {
        protected ASTVisitableElement currentParent = null;

        public virtual T Visit(ASTVisitableElement node)
        {
            return node.Accept(visitor: this);
        }

        public virtual T VisitChildren(ASTVisitableElement node)
        {
            ASTVisitableElement oldParent = currentParent;
            currentParent = node;
            T result = default(T);
            foreach (ASTVisitableElement child in node.GetChildren())
            {
                result = AggregateResult(result, child.Accept(visitor: this));
            }
            currentParent = oldParent;
            return result;
        }

        public virtual T AggregateResult(T oldResult, T value)
        {
            return value;
        }
    }

    public abstract class CalcInterASTBaseVisitor<T> : ASTBaseVisitor<T>
    {
        public virtual T VisitCompileUnit(CCompileUnit node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitFindRoot(CFindRoot node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitMaxExtreme(CMaxExtreme node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitMinExtreme(CMinExtreme node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitTanLine(CTanLine node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitFunction(CFunction node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitVariable(CVariable node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitPair(CPair node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitValueArray(CValueArray node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitFcall(CFCall node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitAddition(CAddition node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitSubtraction(CSubtraction node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitMultiplication(CMultiplication node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitDivision(CDivision node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitAbsolute(CAbsolute node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitFactorial(CFactorial node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitPower(CPower node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitExprPlus(CExprPlus node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitExprMinus(CExprMinus node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitValuePlus(CValuePlus node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitValueMinus(CValueMinus node)
        {
            return VisitChildren(node);
        }

        public virtual T VisitIDENTIFIER(CIdentifier node)
        {
            return default(T);
        }

        public virtual T VisitNUMBER(CNumber node)
        {
            return default(T);
        }
    }
}
