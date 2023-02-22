using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorInterpreter
{
    public class ASTEvalVisitor : CalcInterASTBaseVisitor<Expression>
    {
        public Library lib;
        private StringBuilder m_repository;
        private Stack<int> m_function;

        public ASTEvalVisitor(Library library)
        {
            lib = library;
            m_repository = new StringBuilder();
            m_function = new Stack<int>();
        }

        private static Expression<Func<double, double>> FuncToExpression<T>(Func<double, double> f)
        {
            return x => f(x);
        } 
        
        public override Expression VisitCompileUnit(CCompileUnit node)
        {
            // Visit Function Definitions and emmit code to distinct functions
            foreach (ASTVisitableElement child in node.GetChildrenContext(CCompileUnit.CT_COMPILEUNIT_MATHSTATEMENT))
            {
                Visit(child);
            }

            // Visit Statements Context and emmit code to main functions
            foreach (ASTVisitableElement child in node.GetChildrenContext(CCompileUnit.CT_COMPILEUNIT_COMMANDSTATEMENT))
            {
                Visit(child);
            }

            return default;
        }

        public override Expression VisitFindRoot(CFindRoot node)
        {
            if (node.GetChildrenContextNumber(CFindRoot.CT_FNAME) != 0) //if Function's Name exists
            {
                CIdentifier? id = node.GetChild(CFindRoot.CT_FNAME, 0) as CIdentifier;
                lib.getMathFunction(id.M_Name).findRoots();
            }
            else if (node.GetChildrenContextNumber(CFindRoot.CT_FUNCTION) != 0)
            {
                foreach (ASTVisitableElement child in node.GetChildrenContext(CFindRoot.CT_FUNCTION))
                {
                    Visit(child);
                }
                lib.getLastMathFunction().findRoots();
            }

            return default;
        }

        public override Expression VisitMaxExtreme(CMaxExtreme node)
        {
            if (node.GetChildrenContextNumber(CMaxExtreme.CT_FNAME) != 0) //if Function's Name exists
            {
                CIdentifier? id = node.GetChild(CMaxExtreme.CT_FNAME, 0) as CIdentifier;
                lib.getMathFunction(id.M_Name).calculateMaxLocalExtremes();
            }
            else if (node.GetChildrenContextNumber(CMaxExtreme.CT_FUNCTION) != 0)
            {
                foreach (ASTVisitableElement child in node.GetChildrenContext(CMaxExtreme.CT_FUNCTION))
                {
                    Visit(child);
                }
                lib.getLastMathFunction().calculateMaxLocalExtremes();
            }

            return default;
        }

        public override Expression VisitMinExtreme(CMinExtreme node)
        {
            if (node.GetChildrenContextNumber(CMinExtreme.CT_FNAME) != 0) //if Function's Name exists
            {
                CIdentifier? id = node.GetChild(CMinExtreme.CT_FNAME, 0) as CIdentifier;
                lib.getMathFunction(id.M_Name).calculateMinLocalExtremes();
            }
            else if (node.GetChildrenContextNumber(CMinExtreme.CT_FUNCTION) != 0)
            {
                foreach (ASTVisitableElement child in node.GetChildrenContext(CMinExtreme.CT_FUNCTION))
                {
                    Visit(child);
                }
                lib.getLastMathFunction().calculateMinLocalExtremes();
            }

            return default;
        }

        public override Expression VisitTanLine(CTanLine node)
        {
            Expression pointExpression = null;

            if (node.GetChildrenContextNumber(CTanLine.CT_FNAME) != 0) //if Function's Name exists
            {
                CIdentifier? id = node.GetChild(CTanLine.CT_FNAME, 0) as CIdentifier;

                foreach (ASTVisitableElement child in node.GetChildrenContext(CTanLine.CT_POINT))
                {
                    pointExpression = Visit(child);
                }

                double point = Convert.ToDouble(pointExpression.ToString().Replace(".", ","));

                lib.getMathFunction(id.M_Name).createTanLineFunctionCall(point);
            }
            else if (node.GetChildrenContextNumber(CTanLine.CT_FUNCTION) != 0)
            {
                foreach (ASTVisitableElement child in node.GetChildrenContext(CTanLine.CT_FUNCTION))
                {
                    Visit(child);
                }

                foreach (ASTVisitableElement child in node.GetChildrenContext(CTanLine.CT_POINT))
                {
                    pointExpression = Visit(child);
                }

                double point = Convert.ToDouble(pointExpression.ToString().Replace(".", ","));

                lib.getLastMathFunction().createTanLineFunctionCall(point);
            }

            return default;
        }

        public override Expression VisitFunction(CFunction node)
        {
            m_function.Push(1);
            m_repository.Clear();

            Expression function = null;

            //Add the new Function to Library
            if (node.GetChildrenContextNumber(CFunction.CT_FNAME) != 0) //if Function's Name exists
            {
                CIdentifier? id = node.GetChild(CFunction.CT_FNAME, 0) as CIdentifier;
                lib.addMathFunction(id.M_Name);
            }
            else
            {
                lib.addMathFunction("");
            }

            // Visit Statements Context and emmit code to main functions
            foreach (ASTVisitableElement child in node.GetChildrenContext(CFunction.CT_BODY))
            {
                function = Visit(child);
            }

            lib.getLastMathFunction().setFunctionBody(m_repository.ToString().Trim());

            Expression<Func<double, double>> lambda = Expression.Lambda<Func<double, double>>(function, lib.getLastMathFunction().getMathExpressionParameter());

            lib.getLastMathFunction().setFunctionCall(lambda.Compile());

            lib.getLastMathFunction().calculateSetOfValues();

            m_function.Pop();

            return function;
        }

        public override Expression VisitFcall(CFCall node)
        {
            Expression firstArg = null, secondArg = null, tmp = null;
            CIdentifier? id = node.GetChild(CFCall.CT_FNAME, 0) as CIdentifier;

            m_repository.Append(id.M_Name + "(");

            int i = 1, last = node.GetChildrenContextNumber(CFCall.CT_ARGS);

            foreach (ASTVisitableElement child in node.GetChildrenContext(CFCall.CT_ARGS))
            {
                firstArg = secondArg = Visit(child);

                if (last == 2 && i == 1)
                {
                    tmp = firstArg;
                    m_repository.Append(", ");
                }
                else if (last == 2 && i == 2)
                {
                    firstArg = tmp;
                }

                i++;
            }

            m_repository.Append(")");

            if (last == 1)
            {
                return lib.mathMethodCallExpressions.findMethodCallExpression(id.M_Name, firstArg, lib.getLastMathFunction().getMathExpressionParameter());
            }
            else if (last == 2)
            {
                return lib.mathMethodCallExpressions.findMethodCallExpression(id.M_Name, firstArg, secondArg, lib.getLastMathFunction().getMathExpressionParameter());
            }

            return default;
        }

        public override Expression VisitFactorial(CFactorial node)
        {
            Expression number = null;

            m_repository.Append("(");

            foreach (ASTVisitableElement child in node.GetChildrenContext(CFactorial.CT_EXPRESSION))
            {
                number = Visit(child);
            }

            m_repository.Append("!)");

            return Expression.Call(typeof(SpecialFunctions).GetMethod("Gamma"), Expression.Add(number, Expression.Constant(1.0)));
        }

        public override Expression VisitAbsolute(CAbsolute node)
        {
            Expression number = null;

            m_repository.Append("|");

            foreach (ASTVisitableElement child in node.GetChildrenContext(CAbsolute.CT_EXPRESSION))
            {
                number = Visit(child);
            }

            m_repository.Append("|");

            var logCall = typeof(Math).GetMethods().First(x => x.Name == "Abs" && x.ReturnParameter.ParameterType.Name.CompareTo("Double") == 0);
            return Expression.Call(logCall, number);
        }

        public override Expression VisitPower(CPower node)
        {
            Expression left = null, right = null;

            foreach (ASTVisitableElement child in node.GetChildrenContext(CPower.CT_LEFT))
            {
                left = Visit(child);
            }

            m_repository.Append("^");

            foreach (ASTVisitableElement child in node.GetChildrenContext(CPower.CT_RIGHT))
            {
                right = Visit(child);
            }

            return Expression.Power(left, right);
        }

        public override Expression VisitMultiplication(CMultiplication node)
        {
            Expression left = null, right = null;

            m_repository.Append("(");

            foreach (ASTVisitableElement child in node.GetChildrenContext(CMultiplication.CT_LEFT))
            {
                left = Visit(child);
            }

            m_repository.Append("*");

            foreach (ASTVisitableElement child in node.GetChildrenContext(CMultiplication.CT_RIGHT))
            {
                right = Visit(child);
            }

            m_repository.Append(")");

            return Expression.Multiply(left, right);
        }

        public override Expression VisitDivision(CDivision node)
        {
            Expression left = null, right = null;

            m_repository.Append("(");

            foreach (ASTVisitableElement child in node.GetChildrenContext(CDivision.CT_LEFT))
            {
                left = Visit(child);
            }

            m_repository.Append("/");

            foreach (ASTVisitableElement child in node.GetChildrenContext(CDivision.CT_RIGHT))
            {
                right = Visit(child);
            }

            m_repository.Append(")");

            return Expression.Divide(left, right);
        }

        public override Expression VisitAddition(CAddition node)
        {
            Expression left = null, right = null;

            foreach (ASTVisitableElement child in node.GetChildrenContext(CAddition.CT_LEFT))
            {
                left = Visit(child);
            }

            m_repository.Append("+");

            foreach (ASTVisitableElement child in node.GetChildrenContext(CAddition.CT_RIGHT))
            {
                right = Visit(child);
            }

            return Expression.Add(left, right);
        }

        public override Expression VisitSubtraction(CSubtraction node)
        {
            Expression left = null, right = null;

            foreach (ASTVisitableElement child in node.GetChildrenContext(CSubtraction.CT_LEFT))
            {
                left = Visit(child);
            }

            m_repository.Append("-");

            foreach (ASTVisitableElement child in node.GetChildrenContext(CSubtraction.CT_RIGHT))
            {
                right = Visit(child);
            }

            return Expression.Subtract(left, right);
        }

        public override Expression VisitExprPlus(CExprPlus node)
        {
            Expression number = null;

            m_repository.Append("(+");

            foreach (ASTVisitableElement child in node.GetChildrenContext(CExprPlus.CT_EXPRESSION))
            {
                number = Visit(child);
            }

            m_repository.Append(")");

            return Expression.UnaryPlus(number);
        }

        public override Expression VisitExprMinus(CExprMinus node)
        {
            Expression number = null;

            m_repository.Append("(-");

            foreach (ASTVisitableElement child in node.GetChildrenContext(CExprMinus.CT_EXPRESSION))
            {
                number = Visit(child);
            }

            m_repository.Append(")");

            return Expression.Negate(number);
        }

        public override Expression VisitVariable(CVariable node)
        {
            CIdentifier? id = node.GetChild(CVariable.CT_FNAME, 0) as CIdentifier;

            lib.addMathParameter(id.M_Name);

            foreach (ASTVisitableElement child in node.GetChildrenContext(CVariable.CT_BODY))
            {
                Visit(child);
            }

            lib.getLastMathParameter().fixRange();

            foreach (var mathFunction in lib.mathFunctions)
            {
                mathFunction.updateMathParameter(lib.getLastMathParameter());
            }

            return default;
        }

        public override Expression VisitPair(CPair node)
        {
            CIdentifier? id = node.GetChild(CPair.CT_LEFT, 0) as CIdentifier;
            lib.getLastMathParameter().addParameter(id.M_Name);

            foreach (ASTVisitableElement child in node.GetChildrenContext(CPair.CT_RIGHT))
            {
                Expression result = Visit(child);

                double number = Convert.ToDouble(result.ToString().Replace(".", ","));
                lib.getLastMathParameter().addParameter(number);
            }

            return default;
        }

        public override Expression VisitValueArray(CValueArray node)
        {
            foreach (ASTVisitableElement child in node.GetChildrenContext(CValueArray.CT_BODY))
            {
                Expression result = Visit(child);

                double number = Convert.ToDouble(result.ToString().Replace(".", ","));
                lib.getLastMathParameter().addParameter(number);
            }

            lib.getLastMathParameter().addParameter("NA");

            return Expression.Constant(0.0, typeof(double));
        }

        public override Expression VisitValuePlus(CValuePlus node)
        {
            Expression result = null;

            foreach (ASTVisitableElement child in node.GetChildrenContext(CValuePlus.CT_VALUE))
            {
                result = Visit(child);
            }

            return Expression.UnaryPlus(result);
        }

        public override Expression VisitValueMinus(CValueMinus node)
        {
            Expression result = null;

            foreach (ASTVisitableElement child in node.GetChildrenContext(CValueMinus.CT_VALUE))
            {
                result = Visit(child);
            }

            return Expression.Negate(result);
        }

        public override Expression VisitIDENTIFIER(CIdentifier node)
        {
            if (node.M_Name.ToLower().CompareTo("pi") == 0)
            {
                m_repository.Append(node.M_Name);
                return Expression.Constant(Math.PI, typeof(double));
            }
            else if (node.M_Name.ToLower().CompareTo("e") == 0)
            {
                m_repository.Append(node.M_Name);
                return Expression.Constant(Math.E, typeof(double));
            }
            else
            {
                if (lib.hasParameter(node.M_Name)) //if Parameter exists in Library Parameters
                {
                    if (lib.getLastMathFunction().getMathParameter().var_name.CompareTo(node.M_Name) == 0) //if a Function's Parameter does exists
                    {
                        m_repository.Append(node.M_Name);
                        return lib.getLastMathFunction().getMathExpressionParameter();
                    }
                    else //if a Function's Parameter does exists
                    {
                        m_repository.Append(node.M_Name);
                        lib.getLastMathFunction().setMathParameter(lib.getMathParameter(node.M_Name));
                        return lib.getLastMathFunction().getMathExpressionParameter();
                    }
                }
                else //if Parameter doesn't exists in Library Parameters
                {
                    foreach (var mathFunction in lib.mathFunctions)
                    {
                        if (mathFunction.name.CompareTo(node.M_Name) == 0) //if Parameter is a Function's Name
                        {
                            m_repository.Append("(" + mathFunction.functionBody + ")");
                            Expression<Func<double, double>> ExprFunc = FuncToExpression<double>(mathFunction.functionCall);
                            if (lib.getLastMathFunction().getMathParameter().var_name.CompareTo("Default") == 0) //if the Function's Parameter doesn't exists
                            {
                                lib.getLastMathFunction().setMathParameter(mathFunction.getMathParameter());
                            }
                            return Expression.Invoke(ExprFunc, mathFunction.getMathExpressionParameter());
                        }
                    }

                    if (lib.getLastMathFunction().getMathParameter().var_name.CompareTo(node.M_Name) == 0) //if Parameter exists in Function's Parameter
                    {
                        m_repository.Append(node.M_Name);
                        return lib.getLastMathFunction().getMathExpressionParameter();
                    }
                    else //if Parameter doesn't exists in Function's Parameter
                    {
                        m_repository.Append(node.M_Name);
                        lib.getLastMathFunction().setMathParameter(new MathParameter(node.M_Name));
                        return lib.getLastMathFunction().getMathExpressionParameter();
                    }
                }
            }
        }

        public override Expression VisitNUMBER(CNumber node)
        {
            if (m_function.Count != 0)
            {
                m_repository.Append(node.M_Name);
            }

            double number = Convert.ToDouble(node.M_Name.Replace(".", ","));
            return Expression.Constant(number, typeof(double));
        }
    }
}
