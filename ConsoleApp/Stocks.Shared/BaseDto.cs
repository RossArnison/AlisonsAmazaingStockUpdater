using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Shared
{
    public class BaseDto
    {
        public DateTime ExecutionTime { get; set; }
        public Exception Exception { get; set; }
    }
}
