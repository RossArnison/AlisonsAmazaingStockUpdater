using Stocks.Shared;
using Stocks.Shared.TestHelpers.Interface;
using System.Collections.Generic;

namespace Stocks.BuisnessLayer
{
    public interface IStockManager
    {
        string DisplayStocks();
        IEnumerable<ItemDto> GetItems();
        IEnumerable<StockDto> GetStocks();
        void ParseInput(IEnumerable<string> inputs);
        void ProgressDay();
        void SetIFile(IFile value);
        void SetStocks(IEnumerable<StockDto> stocks);
    }
}