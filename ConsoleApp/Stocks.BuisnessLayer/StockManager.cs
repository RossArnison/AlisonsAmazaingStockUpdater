using System;
using System.Collections.Generic;
using System.Linq;
using Stocks.DataLayer;
using Stocks.Interface.DataLayer;
using Stocks.Shared;
using Stocks.Shared.TestHelpers.Interface;

namespace Stocks.BuisnessLayer
{
    public class StockManager : IStockManager
    {
        private IDataController _controller;
        private DateTime _today;

        public StockManager()
        {
            _controller = new DataController();
            _today = DateTime.Today;
        }

        public void ParseInput(IEnumerable<string> inputs)
        {
            var items = _controller.GetItems();

            var stocks = new List<StockDto>();

            for (int i = 0; i < inputs.Count(); i++)
            {
                var input = inputs.ElementAt(i);
                var sections = input.Split(',');

                var name = sections[0].Trim();

                var dto = new StockDto { Id = i };

                if (items.Select(s => s.Name).Contains(name))
                {
                    var daysTillOff = int.Parse(sections[1].Trim());
                    var currentQuality = int.Parse(sections[2].Trim());

                    dto.ItemId = items.First(s => s.Name == name).Id;
                    dto.SellByDates = _today.AddDays(daysTillOff);
                    dto.Quality = currentQuality;
                }

                stocks.Add(dto);
            }

            _controller.SetStocks(stocks);
        }

        public string DisplayStocks()
        {
            var stocks = _controller.GetStocks();
            var items = _controller.GetItems();

            var displayStocks = stocks.Select(s =>
            {
                if (!s.ItemId.HasValue)
                {
                    return "NO SUCH ITEM";
                }

                var name = items.First(i => i.Id == s.ItemId.Value).Name;
                var daysTillOff = s.SellByDates - _today;

                var displayDays = daysTillOff.HasValue
                        ? daysTillOff.Value.Days.ToString()
                        : "0";

                return $"{name} {displayDays} {s.Quality}";
            });

            return string.Join(Environment.NewLine, displayStocks);
        }

        public void ProgressDay()
        {
            _today = _today.AddDays(1);

            var stocks = _controller.GetStocks();
            var items = _controller.GetItems();
            var qualityModifiers = _controller.GetQualityModifiers();

            for (int i = 0; i < stocks.Count(); i++)
            {
                var stock = stocks.ElementAt(i);

                if (!stock.ItemId.HasValue || !stock.SellByDates.HasValue)
                    continue;

                var item = items.First(it => it.Id == stock.ItemId);

                var qualityMultiplier = item.QualityModifier;
                var expiresInDays = (stock.SellByDates - _today).Value.Days;

                if (expiresInDays < 0 && item.QualityModifier < 0)
                {
                    qualityMultiplier *= 2;
                }

                if (qualityModifiers.Any(q => q.ItemId == stock.ItemId && q.ExpiresIn >= expiresInDays))
                {
                    qualityMultiplier = qualityModifiers
                        .Where(q => q.ItemId == stock.ItemId && q.ExpiresIn >= expiresInDays)
                        .OrderBy(q => q.ExpiresIn)
                        .First()
                        .QualityModifier;

                    if (qualityMultiplier == 0)
                    {
                        stock.Quality = 0;
                    }
                }

                stock.Quality += qualityMultiplier;

                if (stock.Quality < 0)
                {
                    stock.Quality = 0;
                }

                if (stock.Quality > 50)
                {
                    stock.Quality = 50;
                }
            }

            _controller.SetStocks(stocks);
        }

        public IEnumerable<StockDto> GetStocks() => _controller.GetStocks();
        public void SetStocks(IEnumerable<StockDto> stocks) => _controller.SetStocks(stocks);
        public IEnumerable<ItemDto> GetItems() => _controller.GetItems();
        public void SetIFile(IFile value) => _controller.File = value;
    }
}
