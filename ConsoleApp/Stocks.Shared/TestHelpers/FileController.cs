using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stocks.Shared.TestHelpers.Interface;

namespace Stocks.Shared.TestHelpers
{
    public class FileController : IFile
    {
        public string ReadAllText(string filePath) => File.ReadAllText(filePath);

        public void WriteAllText(string filePath, string contents) => File.WriteAllText(filePath, contents);
    }
}
