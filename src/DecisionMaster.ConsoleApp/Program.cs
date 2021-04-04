using DecisionMaster.UserInterface.Controllers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace DecisionMaster.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = File.ReadAllText(args[0]);
            var resultFileNameCSV = args[1];
            SolutionController controller = JsonConvert.DeserializeObject<SolutionController>(json);
            var titles = controller.GetAlternativesTitles();
            var grid = controller.GetSolutionsAsDataGrid();
            var line = string.Join(";", titles.ToArray());
            File.WriteAllLines(resultFileNameCSV, new[] { line });
            Console.WriteLine(line);
            foreach (var row in grid)
            {
                line = "";
                var first = true;
                foreach (var cell in row.Cells)
                {
                    var c = (DataGridViewTextBoxCell)cell;
                    if (!first)
                    {
                        line += ";";
                    }
                    line += $"{c.Value}";
                    first = false;
                }
                Console.WriteLine(line);
                File.AppendAllLines(resultFileNameCSV, new[] { line });
            }
        }
    }
}
