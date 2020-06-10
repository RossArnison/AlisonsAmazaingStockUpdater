using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stocks.BuisnessLayer;
using Stocks.DataLayer;
using Stocks.Shared;

namespace ConsoleApp
{
    class Program
    {
        private static readonly string[] positives = new[] { "y", "ye", "yes" };

        static void Main(string[] args)
        {
            var stockManager = new StockManager();
            Console.WriteLine("Please input the current products separated by commas. Enter a blank line to finish.");

            var inputs = GetStockInput();
            stockManager.ParseInput(inputs);
            Console.WriteLine("Input Accepted!");

            DayProgressionLoop(stockManager);

            Console.WriteLine("Press any key to continues...");
            Console.ReadKey(true);
        }

        private static IEnumerable<string> GetStockInput()
        {
            var inputList = new List<string>();
            while (true)
            {
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    break;

                inputList.Add(input);
            }

            return inputList;
        }

        private static void DayProgressionLoop(StockManager stockManager)
        {
            Console.WriteLine($"{Environment.NewLine}Do you want to progress the day?");
            var input = Console.ReadLine();

            if (positives.Contains(input.ToLower()))
            {
                stockManager.ProgressDay();
                Console.WriteLine(stockManager.DisplayStocks());
                DayProgressionLoop(stockManager);
            }
        }
    }
}
