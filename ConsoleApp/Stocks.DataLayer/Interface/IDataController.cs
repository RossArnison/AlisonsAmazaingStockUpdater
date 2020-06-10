using Stocks.Shared;
using Stocks.Shared.TestHelpers.Interface;
using System.Collections.Generic;

namespace Stocks.Interface.DataLayer
{
    public interface IDataController
    {
        IFile File { get; set; }

        IEnumerable<ItemDto> GetItems();
        IEnumerable<ChangingQualityModifiersDto> GetQualityModifiers();
        IEnumerable<StockDto> GetStocks();
        void SetItems(IEnumerable<ItemDto> value);
        void SetQualityModifiers(IEnumerable<ChangingQualityModifiersDto> value);
        void SetStocks(IEnumerable<StockDto> value);
    }
}