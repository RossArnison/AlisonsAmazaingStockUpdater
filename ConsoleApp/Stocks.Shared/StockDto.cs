using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Shared
{
    public class StockDto
    {
        public int Id { get; set; }
        public int? ItemId { get; set; }
        public DateTime? SellByDates { get; set; }
        public int Quality { get; set; }
    }
}
