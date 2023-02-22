using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAppMathGraph.Models
{
    public class InputData
    {
        [Required]
        public string Function { get; set; }

        [Required]
        public string RangeMax { get; set; }

        [Required]
        public string RangeMin { get; set; }

        [Required]
        public string Step { get; set; }
    }

    public class OutputData
    {
        public Table table { get; set; }
        public List<Function> functions { get; set; }

        public OutputData()
        {
            this.setTableValue(new Table());
            this.setFunctionValue(new List<Function>());
        }

        public void setTableValue(Table table)
        {
            this.table = table;
        }

        public Table getTableValue()
        {
            return this.table;
        }

        public void setFunctionValue(List<Function> functions)
        {
            this.functions = functions;
        }

        public List<Function> getFunctionValue()
        {
            return this.functions;
        }
    }

    public class Function
    {
        public string name { get; set; }
        public string functionBody { get; set; }
        public string function { get; set; }
        public List<double> roots { get; set; }
        public List<double> localMaxExtremes { get; set; }
        public List<double> localMinExtremes { get; set; }

        public Function()
        {
            this.setName("");
            this.setFunctionBody("");
            this.setFunction("");
            this.setRoots(new List<double>());
            this.setLocalMaxExtremes(new List<double>());
            this.setLocalMinExtremes(new List<double>());
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public string getName()
        {
            return this.name;
        }

        public void setFunctionBody(string functionBody)
        {
            this.functionBody = functionBody;
        }

        public string getFunctionBody()
        {
            return this.functionBody;
        }

        public void setFunction(string function)
        {
            this.function = function;
        }

        public string getFunction()
        {
            return this.function;
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

    }

    public class Table
    {
        public List<ColHeaders> cols { get; set; }

        public List<RowData> rows { get; set; }

        public Table()
        {
            this.cols = new List<ColHeaders>();
            this.rows = new List<RowData>();
        }

        public void addRow(params double[] values)
        {
            this.rows.Add(new RowData(values));
        }

        public void addHeaderCol()
        {
            this.cols.Add(new ColHeaders(this.cols.Count.ToString()));
        }

        public void addHeaderCol(string label)
        {
            this.cols.Add(new ColHeaders(this.cols.Count.ToString(), label));
        }

        public void setHeaderColLabel(string id, string label)
        {
            for (int i = 0; i < this.cols.Count; i++)
            {
                if (this.cols.ElementAt(i).getId().CompareTo(id) == 0)
                {
                    this.cols.ElementAt(i).setLabel(label);
                }
            }
        }
    }

    public class ColHeaders
    {
        public string id { get; set; }

        public string label { get; set; }

        public string type { get; set; }

        public ColHeaders(string id, string label)
        {
            this.setId(id);
            this.setLabel(label);
            this.setType();
        }

        public ColHeaders(string id)
        {
            this.setId(id);
            this.setType();
        }

        public void setId(string id)
        {
            this.id = id;
        }

        public void setLabel(string label)
        {
            this.label = label;
        }

        public void setType()
        {
            this.type = "number";
        }

        public string getId()
        {
            return this.id;
        }

        public string getLabel()
        {
            return this.label;
        }

        public string getType()
        {
            return this.type;
        }
    }

    public class RowData
    {
        public Value[] c { get; set; }

        public List<Value> rowData;

        public RowData(params double[] values)
        {
            this.rowData = new List<Value>();

            foreach (double value in values)
            {
                this.rowData.Add(new Value(value));
            }

            this.c = this.rowData.ToArray();
        }

        public void setCellAt(double x, int index)
        {
            if (c[index] != null)
            {
                c[index].setValue(x);
            }
            else
            {
                c[index] = new Value(x);
            }
            return;
        }

        public double getCellAt(int index)
        {
            if (c[index] != null)
            {
                return c[index].getValue();
            }
            else
            {
                return 0;
            }
        }
    }

    public class Value
    {
        public double v { get; set; }

        public Value(double v)
        {
            this.setValue(v);
        }

        public void setValue(double v)
        {
            this.v = v;
        }

        public double getValue()
        {
            return this.v;
        }
    }
}
