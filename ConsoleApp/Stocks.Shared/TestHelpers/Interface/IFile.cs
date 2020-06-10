using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Shared.TestHelpers.Interface
{
    public interface IFile
    {
        string ReadAllText(string filePath);
        void WriteAllText(string filePath, string serialisedJson);
    }
}
