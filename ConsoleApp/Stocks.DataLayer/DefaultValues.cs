using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stocks.Shared;

namespace Stocks.DataLayer
{
    public static class DefaultValues
    {
        public static IEnumerable<ChangingQualityModifiersDto> DefaultQualityModifiers = new List<ChangingQualityModifiersDto>
        {
            new ChangingQualityModifiersDto
            {
                Id = 0,
                ItemId = 1,
                ExpiresIn = 10,
                QualityModifier = 2,
            },
            new ChangingQualityModifiersDto
            {
                Id = 0,
                ItemId = 1,
                ExpiresIn = 5,
                QualityModifier = 3,
            },
            new ChangingQualityModifiersDto
            {
                Id = 0,
                ItemId = 1,
                ExpiresIn = 0,
                QualityModifier = 0,
            },
        };

        public static IEnumerable<ItemDto> DefaultItems = new List<ItemDto>
        {
            new ItemDto
            {
                Id = 0,
                Name = "Aged Brie",
                QualityModifier = 1
            },
            new ItemDto
            {
                Id = 1,
                Name = "Christmas Crackers",
                QualityModifier = 1
            },
            new ItemDto
            {
                Id = 2,
                Name = "Soap",
                QualityModifier = 0
            },
            new ItemDto
            {
                Id = 3,
                Name = "Frozen Item",
                QualityModifier = -1
            },
            new ItemDto
            {
                Id = 4,
                Name = "Fresh Item",
                QualityModifier = -2
            },
        };

        public static IEnumerable<StockDto> DefaultStocks = new List<StockDto>();
    }
}