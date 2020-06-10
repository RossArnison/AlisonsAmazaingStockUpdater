using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Shared
{
    public class ChangingQualityModifiersDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int ExpiresIn { get; set; }
        public int QualityModifier { get; set; }
    }
}
