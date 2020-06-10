using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stocks.Shared;
using Stocks.Shared.TestHelpers;
using Stocks.Shared.TestHelpers.Interface;

namespace Stocks.DataLayer
{
    public class DataController
    {
        private const string TableFolder = "DataFiles";

        private const string StocksTable = "Stocks.json";
        private const string ItemsInStockTable = "ItemsInStock.json";
        private const string QualityModifiersTable = "ChangingQualityModifiers.json";
        
        public DataController()
        {
            File = new FileController();
        }

        public IFile File { get; set; }
        
        public IEnumerable<StockDto> GetStocks()
        {
            return DeserializeDataObject<StockDto>(StocksTable);
        }

        public void SetStocks(IEnumerable<StockDto> value)
        {
            SetNewJsonValue(value, StocksTable);
        }

        public IEnumerable<ItemDto> GetItems()
        {
            return DeserializeDataObject<ItemDto>(ItemsInStockTable);
        }

        public void SetItems(IEnumerable<ItemDto> value)
        {
            SetNewJsonValue(value, ItemsInStockTable);
        }

        public IEnumerable<ChangingQualityModifiersDto> GetQualityModifiers()
        {
            return DeserializeDataObject<ChangingQualityModifiersDto>(QualityModifiersTable);
        }

        public void SetQualityModifiers(IEnumerable<ChangingQualityModifiersDto> value)
        {
            SetNewJsonValue(value, QualityModifiersTable);
        }

        private IEnumerable<T> DeserializeDataObject<T>(string filename)
        {
            var filePath = LocationFileName(filename);
            var jsonString = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(jsonString.Replace(Environment.NewLine, string.Empty).Replace("{", string.Empty).Replace("}", string.Empty)))
            {
                SetDefaultValues();
                return DeserializeDataObject<T>(filename);
            }

            var jsonObject = JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            return jsonObject;
        }

        private void SetNewJsonValue(object value, string filename)
        {
            var filePath = LocationFileName(filename);
            var serialisedJson = JsonConvert.SerializeObject(value);
            File.WriteAllText(filePath, serialisedJson);
        }

        private string LocationFileName(string filename)
        {
            var filePath = Path.Combine(TableFolder, filename);
            return filePath;
        }

        private void SetDefaultValues()
        {
            SetItems(DefaultValues.DefaultItems);
            SetQualityModifiers(DefaultValues.DefaultQualityModifiers);
            SetStocks(DefaultValues.DefaultStocks);
        }
    }
}
