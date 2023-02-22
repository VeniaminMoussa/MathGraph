using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorInterpreter
{
    public class Library
    {
        public MathMethodCallExpressions mathMethodCallExpressions;
        public List<MathParameter> mathParameters;
        public List<MathFunction> mathFunctions;
        public List<string> functionsTagNamesDictionary;
        public SortedDictionary<double, LinkedList<double>> functionsValuesDictionary;

        public Library()
        {
            mathParameters = new List<MathParameter>();
            mathFunctions = new List<MathFunction>();
            mathMethodCallExpressions = new MathMethodCallExpressions();
            functionsValuesDictionary = new SortedDictionary<double, LinkedList<double>>();
            functionsTagNamesDictionary = new List<string>();
        }

        public void clearLibrary()
        {
            this.functionsValuesDictionary.Clear();
            this.functionsTagNamesDictionary.Clear();
            this.mathFunctions.Clear();
            this.mathParameters.Clear();
        }

        public bool hasMathFunction(string funcname)
        {
            if (this.mathFunctions.Contains(new MathFunction(funcname)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool hasParameter(string varname)
        {
            if (this.mathParameters.Contains(new MathParameter(varname)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool removeMathFunction(string funcname)
        {
            if (hasMathFunction(funcname))
            {
                this.mathFunctions.Remove(new MathFunction(funcname));
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool removeMathParameter(string varname)
        {
            if (hasParameter(varname))
            {
                this.mathParameters.Remove(new MathParameter(varname));
                return true;
            }
            else
            {
                return false;
            }
        }

        public void addMathFunction(MathFunction mathFunction)
        {
            this.removeMathFunction(mathFunction.name);
            this.mathFunctions.Add(mathFunction);
        }

        public void addMathFunction(string funcname)
        {
            this.removeMathFunction(funcname);
            this.mathFunctions.Add(new MathFunction(funcname));
        }

        public void addMathParameter(string varname)
        {
            this.removeMathParameter(varname);
            this.mathParameters.Add(new MathParameter(varname));
        }

        public MathFunction getLastMathFunction()
        {
            if (mathFunctions.Count != 0)
                return mathFunctions.LastOrDefault();
            else
                throw (new InvalidRegistryException("Index of list Out Of Bounds"));
        }

        public MathFunction getMathFunctionAt(int index)
        {
            if (this.mathFunctions.Count == 0 || index < 0)
            {
                throw (new InvalidRegistryException("Index of list Out Of Bounds"));
            }
            else
            {
                return mathFunctions[index];
            }
        }

        public MathFunction getMathFunction(string funcname)
        {
            if (this.hasMathFunction(funcname))
                return this.getMathFunctionAt(this.mathFunctions.IndexOf(new MathFunction(funcname)));
            else
                throw (new InvalidRegistryException("Index of list Out Of Bounds"));
        }

        public MathParameter getLastMathParameter()
        {
            if (mathParameters.Count != 0)
                return mathParameters.LastOrDefault();
            else
                throw (new InvalidRegistryException("Index of list Out Of Bounds"));
        }

        public MathParameter getMathParameterAt(int index)
        {
            if (this.mathParameters.Count == 0 || index < 0)
            {
                throw (new InvalidRegistryException("Index of list Out Of Bounds"));
            }
            else
            {
                return mathParameters[index];
            }
        }

        public MathParameter getMathParameter(string varname)
        {
            if (this.hasParameter(varname))
                return this.getMathParameterAt(this.mathParameters.IndexOf(new MathParameter(varname)));
            else
                throw (new InvalidRegistryException("Index of list Out Of Bounds"));
        }

        public SortedDictionary<double, LinkedList<double>> createJoinedFunctionsData()
        {
            functionsValuesDictionary.Clear();
            functionsTagNamesDictionary.Clear();

            foreach (var mathParameter in mathParameters)
            {
                foreach (var mathParameterRange in mathParameter.var_range)
                {
                    if (!functionsValuesDictionary.ContainsKey(mathParameterRange))
                    {
                        functionsValuesDictionary.Add(mathParameterRange, new LinkedList<double>());
                    }
                }
            }

            foreach (var mathFunction in mathFunctions)
            {
                foreach (var mathFunctionDictionaryKeys in mathFunction.functionValueDictionary.Keys)
                {
                    if (!functionsValuesDictionary.ContainsKey(mathFunctionDictionaryKeys))
                    {
                        functionsValuesDictionary.Add(mathFunctionDictionaryKeys, new LinkedList<double>());
                    }
                }
            }

            foreach (var mathFunction in mathFunctions)
            {
                functionsTagNamesDictionary.Add(mathFunction.getFunction());

                if (mathFunction.tanLineFunctionCall != null)
                    functionsTagNamesDictionary.Add(mathFunction.getTanLineName());

                foreach (KeyValuePair<double, LinkedList<double>> point in functionsValuesDictionary)
                {
                    if (mathFunction.functionValueDictionary.ContainsKey(point.Key))
                    {
                        functionsValuesDictionary[point.Key].AddLast(mathFunction.functionValueDictionary[point.Key]);
                    }
                    else
                    {
                        functionsValuesDictionary[point.Key].AddLast(double.NaN);
                    }

                    if (mathFunction.tanLineFunctionCall == null)
                        continue;

                    if (mathFunction.tangentValueDictionary.ContainsKey(point.Key))
                    {
                        functionsValuesDictionary[point.Key].AddLast(mathFunction.tangentValueDictionary[point.Key]);
                    }
                    else
                    {
                        functionsValuesDictionary[point.Key].AddLast(double.NaN);
                    }
                }
            }

            return functionsValuesDictionary;
        }
    }

    public class MathMethodCallExpressions
    {
        public enum MethodCallExpressionType
        {
            NA = 0,
            SIN,
            COS,
            TAN,
            LOG,
            LN,
            DERIVATIVE,
            INTEGRAL
        };

        public static readonly List<string> methodCallExpressionList = new List<string>
        {
            "NA", "SIN", "COS", "TAN", "LOG", "LN", "DERIVATIVE", "INTEGRAL"
        };

        public int index;

        public MathMethodCallExpressions()
        {
            this.index = 0;
        }

        public bool hasMethodCallExpression(string fcallname)
        {
            if (!methodCallExpressionList.Contains(fcallname.ToUpper()))
            {
                throw (new InvalidRegistryException("Invalid Registry Parameter: \"" + fcallname + "\""));
            }
            else
            {
                return true;
            }
        }

        public int indexOfMethodCallExpression(string fcallname)
        {
            if (hasMethodCallExpression(fcallname))
            {
                return methodCallExpressionList.IndexOf(fcallname.ToUpper());
            }
            else
            {
                return -1;
            }
        }

        public Expression findMethodCallExpression(string fname, Expression firstArg, ParameterExpression parameter)
        {
            index = this.indexOfMethodCallExpression(fname);
            return this.getMethodCallExpression(firstArg, parameter);
        }

        public Expression findMethodCallExpression(string fname, Expression firstArg, Expression secondArg, ParameterExpression parameter)
        {
            index = this.indexOfMethodCallExpression(fname);
            return this.getMethodCallExpression(firstArg, secondArg, parameter);
        }

        public Expression getMethodCallExpression(Expression firstArg, ParameterExpression parameter)
        {
            Expression result = null;

            switch (index)
            {
                case (int)MethodCallExpressionType.NA: //NA
                    //Console.WriteLine("~~~~~~~NA~~~~~~~");
                    break;
                case (int)MethodCallExpressionType.SIN: //Sin
                    {
                        result = Expression.Call(typeof(Math).GetMethod("Sin"), firstArg);
                    }
                    break;
                case (int)MethodCallExpressionType.COS: //Cos
                    {
                        result = Expression.Call(typeof(Math).GetMethod("Cos"), firstArg);
                    }
                    break;
                case (int)MethodCallExpressionType.TAN: //Tan
                    {
                        result = Expression.Call(typeof(Math).GetMethod("Tan"), firstArg);
                    }
                    break;
                case (int)MethodCallExpressionType.LOG: //Log
                    {
                        result = Expression.Call(typeof(Math).GetMethod("Log10"), firstArg);
                    }
                    break;
                case (int)MethodCallExpressionType.LN: //Ln
                    {
                        var logCall = typeof(Math).GetMethods().First(x => x.Name == "Log" && x.GetParameters().Length == 1);
                        result = Expression.Call(logCall, firstArg);
                    }
                    break;
                case (int)MethodCallExpressionType.DERIVATIVE: //Derivative
                    {
                        Expression<Func<double, double>> lambda = Expression.Lambda<Func<double, double>>(firstArg, parameter);
                        result = Expression.Call(typeof(Differentiate).GetMethod("FirstDerivative"), lambda, parameter);
                    }
                    break;
                case (int)MethodCallExpressionType.INTEGRAL: //Integral
                    {
                        Expression<Func<double, double>> lambda = Expression.Lambda<Func<double, double>>(firstArg, parameter);

                        ConstantExpression order = Expression.Constant(-1.0, typeof(double));
                        ConstantExpression x0 = Expression.Constant(0.0, typeof(double));
                        ConstantExpression targetAbsoluteError = Expression.Constant(1E-10, typeof(double));

                        result = Expression.Call(typeof(DifferIntegrate).GetMethod("DoubleExponential"), lambda, parameter, order, x0, targetAbsoluteError);
                    }
                    break;
                default:
                    break;
            }

            return result;
        }

        public Expression getMethodCallExpression(Expression firstArg, Expression secondArg, ParameterExpression parameter)
        {
            Expression result = null;

            switch (index)
            {
                case (int)MethodCallExpressionType.NA: //NA
                    //Console.WriteLine("~~~~~~~NA~~~~~~~");
                    break;
                case (int)MethodCallExpressionType.LOG: //Range
                    {
                        var logCall = typeof(Math).GetMethods().First(x => x.Name == "Log" && x.GetParameters().Length == 2);
                        result = Expression.Call(logCall, secondArg, firstArg);
                    }
                    break;
                case (int)MethodCallExpressionType.DERIVATIVE: //Derivative
                    {
                        Expression<Func<double, double>> lambda = Expression.Lambda<Func<double, double>>(firstArg, parameter);
                        ConstantExpression secArg = (ConstantExpression)secondArg;
                        ConstantExpression order = Expression.Constant(Convert.ToInt32((double)secArg.Value), typeof(int));
                        result = Expression.Call(typeof(Differentiate).GetMethod("Derivative"), lambda, parameter, order);
                    }
                    break;
                default:
                    break;
            }

            return result;
        }
    }

    public class MathParameter
    {
        public enum ParameterType
        {
            NA = 0,
            RANGE,
            STEP
        };

        public static readonly List<string> ParamList = new List<string>
        {
            "NA" ,"RANGE", "STEP"
        };

        public string var_name;
        public ParameterExpression parameterExpression_name;
        public List<double> var_range;
        public double var_step;
        public int index;

        public MathParameter(string varName)
        {
            if (varName.CompareTo("Default") == 0)
            {
                this.setDefault();
            }
            else
            {
                this.setName(varName);
                this.setparameterExpressionName(varName);
                this.setRange(new List<double>());
                this.setStep(0);
                this.setIndex(0);
            }
        }

        public void setDefault()
        {
            this.setName("Default");
            this.setparameterExpressionName("Default");
            this.setRange(new List<double>());
            this.addToRange(-10);
            this.addToRange(10);
            this.setStep(1);
            this.fixRange();
            this.setIndex(0);
        }

        public void setName(String name)
        {
            this.var_name = name;
        }

        public void setparameterExpressionName(String name)
        {
            this.parameterExpression_name = Expression.Parameter(typeof(double), name);
        }

        public void setRange(List<double> range)
        {
            this.var_range = range;
        }

        public void setIndex(int index)
        {
            this.index = index;
        }

        public void setStep(double step)
        {
            this.var_step = step;
        }

        public bool existInParamList(string paramname)
        {
            if (!ParamList.Contains(paramname.ToUpper()))
            {
                throw (new InvalidRegistryException("Invalid Registry Parameter: \"" + paramname + "\""));
            }
            else
            {
                return true;
            }
        }

        public int indexOfParamList(string paramname)
        {
            if (existInParamList(paramname))
            {
                return ParamList.IndexOf(paramname.ToUpper());
            }
            else
            {
                return -1;
            }
        }

        public ParameterExpression getparameterExpressionName()
        {
            return this.parameterExpression_name;
        }

        public void addToRange(double num)
        {
            var_range.Add(num);
            var_range.Sort();
        }

        public void addToRange(double first, double last, double step)
        {
            if (step == 0)
                return;

            for (double i = first; i < last; i = i + step)
            {
                var_range.Add(Math.Round(i, 5));
            }
        }

        public void fixRange()
        {
            var_range.Sort();

            if (this.var_step == 0 || var_range.Count < 2)
                return;

            double first, last, size = var_range.Count;

            for (int i = 0; i < size - 1; i++)
            {
                first = var_range[i];
                last = var_range[(i + 1)];

                this.addToRange(first, last, this.var_step);
            }

            var_range = var_range.Distinct().ToList();
            var_range.Sort();
        }

        public double getFromRangeAt(int index)
        {
            return var_range[index];
        }

        public int addParameter(String param)
        {
            index = this.indexOfParamList(param);
            return index;
        }

        public void addParameter(Double param)
        {
            switch (index)
            {
                case (int)ParameterType.NA: //NA
                    //Console.WriteLine("~~~~~~~NA~~~~~~~");
                    break;
                case (int)ParameterType.RANGE: //Range
                    this.addToRange(param);
                    //Console.WriteLine("~~~~~~RANGE~~~~~~~");
                    break;
                case (int)ParameterType.STEP: //Step
                    this.setStep(param);
                    //Console.WriteLine("~~~~~~STEP~~~~~~~");
                    break;
                default:
                    break;
            }
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                MathParameter p = (MathParameter)obj;
                return (this.var_name == p.var_name);
            }
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }

    public class MathFunction
    {
        public string name;
        public string functionBody;
        public string function;
        public MathParameter mathParameter;
        public Func<double, double> functionCall;
        public string tanLineFunctionName;
        public Func<double, double> tanLineFunctionCall;
        public List<double> roots;
        public List<double> localMaxExtremes;
        public List<double> localMinExtremes;
        public SortedDictionary<double, double> functionValueDictionary;
        public SortedDictionary<double, double> tangentValueDictionary;

        public MathFunction(string name)
        {
            this.setName(name);
            this.setFunctionBody("");
            this.setTanLineFunctionName("");
            this.setRoots(new List<double>());
            this.setLocalMaxExtremes(new List<double>());
            this.setLocalMinExtremes(new List<double>());
            this.setMathParameter(new MathParameter("Default"));
            this.setFunctionCall(null);
            this.setTanLineFunctionCall(null);
            this.setfunctionValueDictionary(new SortedDictionary<double, double>());
            this.setTangentValueDictionary(new SortedDictionary<double, double>());
        }

        public void setName(string name)
        {
            this.name = name;

            this.setFunction(this.name + " = " + this.functionBody);
        }

        public void setFunctionBody(string functionBody)
        {
            this.functionBody = functionBody;

            if (this.name.Length == 0)
                this.setFunction(this.functionBody);
            else
                this.setFunction(this.name + " = " + this.functionBody);
        }

        private void setFunction(string function)
        {
            this.function = function;
        }

        public void setRoots(List<double> roots)
        {
            this.roots = roots;
        }

        public void setLocalMaxExtremes(List<double> localMaxExtremes)
        {
            this.localMaxExtremes = localMaxExtremes;
        }

        public void setLocalMinExtremes(List<double> localMinExtremes)
        {
            this.localMinExtremes = localMinExtremes;
        }

        public void setMathParameter(MathParameter mathParameter)
        {
            this.mathParameter = mathParameter;
        }

        public void setFunctionCall(Func<double, double> function)
        {
            this.functionCall = function;
        }

        public void setTanLineFunctionCall(Func<double, double> tanLineFunctionCall)
        {
            this.tanLineFunctionCall = tanLineFunctionCall;
        }

        public void setfunctionValueDictionary(SortedDictionary<double, double> functionValueDictionary)
        {
            this.functionValueDictionary = functionValueDictionary;
        }

        public void setTangentValueDictionary(SortedDictionary<double, double> tangentValueDictionary)
        {
            this.tangentValueDictionary = tangentValueDictionary;
        }

        private void setTanLineFunctionName(string point)
        {
            this.tanLineFunctionName = "Tangent Line of " + this.function + " at x0 = " + point;
        }

        public void findRoots()
        {
            if (this.functionCall == null)
                return;

            this.roots.Clear();
            double firstRoot;

            List<int> list = getMathParameter().var_range.ConvertAll(x => (int)x).Distinct().ToList();

            for (int i = 0; i < list.Count - 1; i++)
            {
                double min, max;

                try
                {
                    min = list[i];
                    max = list[i + 1];
                    firstRoot = FindRoots.OfFunction(this.functionCall, min, max);

                    this.roots.Add(firstRoot.Round(5));

                    if (!this.functionValueDictionary.ContainsKey(firstRoot.Round(5)))
                    {
                        this.functionValueDictionary.Add(firstRoot.Round(5), 0);
                    }

                }
                catch (Exception e){ }
            }

            this.roots = this.roots.Distinct().ToList();
        }

        public void calculateMaxLocalExtremes()
        {
            if (this.functionCall == null)
                return;

            this.localMaxExtremes.Clear();

            double firstRoot;
            Func<double, double> firstDerivativeFunc;
            Func<double, double> secondDerivativeFunc;
            List<int> list = getMathParameter().var_range.ConvertAll(x => (int)x).Distinct().ToList();

            for (int i = 0; i < list.Count - 1; i++)
            {
                double min, max;

                try
                {
                    min = list[i];
                    max = list[i + 1];

                    firstDerivativeFunc = Differentiate.FirstDerivativeFunc(this.functionCall);

                    firstRoot = FindRoots.OfFunction(firstDerivativeFunc, min, max);

                    secondDerivativeFunc = Differentiate.FirstDerivativeFunc(firstDerivativeFunc);

                    if (firstRoot <= max && firstRoot >= min)
                    {
                        if (secondDerivativeFunc(firstRoot.Round(5)) < 0)
                        {
                            this.localMaxExtremes.Add(firstRoot.Round(5));

                            if (!this.functionValueDictionary.ContainsKey(firstRoot.Round(5)))
                            {
                                this.functionValueDictionary.Add(firstRoot.Round(5), this.functionCall(firstRoot).Round(5));
                            }
                        }
                    }
                }
                catch (Exception e){ }
            }

            this.localMaxExtremes = this.localMaxExtremes.Distinct().ToList();
        }

        public void calculateMinLocalExtremes()
        {
            if (this.functionCall == null)
                return;

            this.localMinExtremes.Clear();

            double firstRoot;
            Func<double, double> firstDerivativeFunc;
            Func<double, double> secondDerivativeFunc;
            List<int> list = getMathParameter().var_range.ConvertAll(x => (int)x).Distinct().ToList();

            for (int i = 0; i < list.Count - 1; i++)
            {
                double min, max;

                try
                {
                    min = list[i];
                    max = list[i + 1];

                    firstDerivativeFunc = Differentiate.FirstDerivativeFunc(this.functionCall);

                    firstRoot = FindRoots.OfFunction(firstDerivativeFunc, min, max);

                    secondDerivativeFunc = Differentiate.FirstDerivativeFunc(firstDerivativeFunc);

                    if (firstRoot <= max && firstRoot >= min)
                    {
                        if (secondDerivativeFunc(firstRoot.Round(5)) > 0)
                        {
                            this.localMinExtremes.Add(firstRoot.Round(5));

                            if (!this.functionValueDictionary.ContainsKey(firstRoot.Round(5)))
                            {
                                this.functionValueDictionary.Add(firstRoot.Round(5), this.functionCall(firstRoot).Round(5));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                }
            }

            this.localMinExtremes = this.localMinExtremes.Distinct().ToList();
        }

        public void createTanLineFunctionCall(double x0)
        {
            if (this.functionCall == null)
                return;

            try
            {
                Func<double, double> derivativeFunc = Differentiate.FirstDerivativeFunc(this.functionCall);
                Func<double, double> func = x => derivativeFunc(x0) * (x - x0) + this.functionCall(x0);

                this.tanLineFunctionCall = func;
                this.setTanLineFunctionName(x0.ToString());
                this.calculateTanLineSetOfValues();
                if (!this.functionValueDictionary.ContainsKey(x0))
                {
                    this.functionValueDictionary.Add(x0, this.functionCall(x0));
                }
            }
            catch (Exception e) { }
        }

        public string getFunction()
        {
            return this.function;
        }

        public string getTanLineName()
        {
            return this.tanLineFunctionName;
        }

        public MathParameter getMathParameter()
        {
            return this.mathParameter;
        }

        public ParameterExpression getMathExpressionParameter()
        {
            if (this.mathParameter != null)
            {
                return this.mathParameter.getparameterExpressionName();
            }
            else
            {
                return null;
            }
        }

        public void calculateSetOfValues()
        {
            double x = 0;
            functionValueDictionary.Clear();

            for (int i = 0; i < this.mathParameter.var_range.Count; i++)
            {
                x = this.mathParameter.getFromRangeAt(i);

                if (this.functionCall(x).IsFinite())
                    this.functionValueDictionary.Add(x.Round(5), this.functionCall(x).Round(5));
            }

            for (int i = 0; i < this.roots.Count; i++)
            {
                x = this.roots[i];

                if (!this.functionValueDictionary.ContainsKey(x.Round(5)))
                {
                    this.functionValueDictionary.Add(x.Round(5), this.functionCall(x).Round(5));
                }
            }

            for (int i = 0; i < this.localMinExtremes.Count; i++)
            {
                x = this.localMinExtremes[i];

                if (!this.functionValueDictionary.ContainsKey(x.Round(5)))
                {
                    this.functionValueDictionary.Add(x.Round(5), this.functionCall(x).Round(5));
                }
            }

            for (int i = 0; i < this.localMaxExtremes.Count; i++)
            {
                x = this.localMaxExtremes[i];

                if (!this.functionValueDictionary.ContainsKey(x.Round(5)))
                {
                    this.functionValueDictionary.Add(x.Round(5), this.functionCall(x).Round(5));
                }
            }
        }

        public void calculateTanLineSetOfValues()
        {
            double x = 0;
            this.tangentValueDictionary.Clear();

            for (int i = 0; i < this.mathParameter.var_range.Count; i++)
            {
                x = this.mathParameter.getFromRangeAt(i);

                if (this.functionCall(x).IsFinite())
                    this.tangentValueDictionary.Add(x.Round(5), this.tanLineFunctionCall(x).Round(5));
            }
        }

        public void updateMathParameter(MathParameter mathParameter)
        {
            if (this.mathParameter == null)
            {
                this.mathParameter = mathParameter;
                return;
            }

            if (this.mathParameter.var_name != mathParameter.var_name)
                return;

            this.mathParameter.setStep(mathParameter.var_step);
            this.mathParameter.setRange(mathParameter.var_range);

            if (this.functionCall != null)
            {
                this.calculateSetOfValues();
            }
            if (this.tanLineFunctionCall != null)
            {
                this.calculateTanLineSetOfValues();
            }
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                MathFunction p = (MathFunction)obj;
                return (this.name == p.name);
            }
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }

    public class InvalidRegistryException : Exception
    {
        public InvalidRegistryException(string message) : base(message) { }
    }
}
