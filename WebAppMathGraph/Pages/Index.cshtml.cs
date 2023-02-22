using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections;
using System.Text.Json.Serialization;
using System.Text.Json;
using WebAppMathGraph.Models;
using Calculator = CalculatorInterpreter.Program;
using CalculatorInterpreter;

namespace WebAppMathGraph.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public InputData Input { get; set; }

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

            string input = "clear; " + Input.Function + ";" + " x " + "{ range: " + "[" + Input.RangeMin + "," + Input.RangeMax + "]" + ", step: " + Input.Step + "};";

            return GenerateData(input);
        }

        public IActionResult OnPostAdd()
        {
            //Handle validation errors that are passed to the server, like posting the form with no value.
            if (!ModelState.IsValid)
            {
                return default;
            }

            string input = Input.Function + ";" + " x " + "{ range: " + "[" + Input.RangeMin + "," + Input.RangeMax + "]" + ", step: " + Input.Step + "};";

            return GenerateData(input);
        }

        public IActionResult OnPostFixRange()
        {
            Input.Function = " ";

            //Handle validation errors that are passed to the server, like posting the form with no value.
            ModelState.ClearValidationState(nameof(Input.Function));
            if (!TryValidateModel(Input.Function, nameof(Input.Function)))
            {
                return default;
            }

            string input = " x " + "{ range: " + "[" + Input.RangeMin + "," + Input.RangeMax + "]" + ", step: " + Input.Step + "};";

            return GenerateData(input);
        }

        public IActionResult OnPostFindRoots()
        {
            string input = "FindRootsOf(" + Request.Form["RootsFunction"] + ");";

            return GenerateData(input);
        }

        public IActionResult OnPostFindMaximum()
        {
            string input = "FindMaxExtremeOf(" + Request.Form["MaxFunction"] + ");";

            return GenerateData(input);
        }

        public IActionResult OnPostFindMinimum()
        {
            string input = "FindMinExtremeOf(" + Request.Form["MinFunction"] + ");";

            return GenerateData(input);
        }

        public IActionResult OnPostFindTanLine()
        {
            string input = "TanLineOfAt(" + Request.Form["TanLineFunction"] + ", " + Request.Form["Point"] + ");";

            return GenerateData(input);
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