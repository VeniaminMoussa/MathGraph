using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Calculator = CalculatorInterpreter.Program;
using CalculatorInterpreter;
using System.Collections;
using System.Text.Json.Serialization;
using System.Text.Json;
using WebAppMathGraph.Models;

namespace WebAppMathGraph.Pages
{
    public class WorkSpaceModel : PageModel
    {
        [BindProperty]
        [Required]
        public string WorkSpace { get; set; } = ""!;

        public static Calculator? calculator;

        public void OnGet()
        {
            calculator = new Calculator();
        }

        public IActionResult OnPostLoad()
        {
            //Handle validation errors that are passed to the server, like posting the form with no value.
            if (!ModelState.IsValid)
            {
                return default;
            }

            return GenerateData(WorkSpace);
        }

        [NonHandler]
        public IActionResult GenerateData(string input)
        {
            Library lib = calculator.Begin(input);

            SortedDictionary<double, LinkedList<double>> joinedFunctionsData = lib.createJoinedFunctionsData();

            JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions();
            JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;

            OutputData dataObject = new OutputData();

            dataObject.table.addHeaderCol("X");
            string jsonString = "";

            if (joinedFunctionsData.Count == 0)
            {
                dataObject.table.addHeaderCol();
                dataObject.table.addRow(0, 0);

                jsonString = JsonSerializer.Serialize(dataObject, JsonSerializerOptions);

                Console.WriteLine(jsonString);

                return new JsonResult(jsonString);
            }

            for (int i = 0; i < lib.functionsTagNamesDictionary.Count; i++)
            {
                dataObject.table.addHeaderCol(lib.functionsTagNamesDictionary[i]);
            }

            IDictionaryEnumerator myEnumerator = joinedFunctionsData.GetEnumerator();

            while (myEnumerator.MoveNext())
            {
                LinkedList<double> list = (LinkedList<double>)myEnumerator.Value;

                list.AddFirst((double)myEnumerator.Key);

                dataObject.table.addRow(list.ToArray());
            }

            foreach (var mathFunction in lib.mathFunctions)
            {
                dataObject.functions.Add(new Function());
                dataObject.functions.Last().setName(mathFunction.name);
                dataObject.functions.Last().setFunctionBody(mathFunction.functionBody);
                dataObject.functions.Last().setFunction(mathFunction.function);
                dataObject.functions.Last().setRoots(mathFunction.roots);
                dataObject.functions.Last().setLocalMaxExtremes(mathFunction.localMaxExtremes);
                dataObject.functions.Last().setLocalMinExtremes(mathFunction.localMinExtremes);
            }

            jsonString = JsonSerializer.Serialize(dataObject, JsonSerializerOptions);

            Console.WriteLine(jsonString);

            return new JsonResult(jsonString);
        }
    }
}
