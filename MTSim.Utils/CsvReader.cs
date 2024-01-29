using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MTSim.Utils
{
    public sealed class CsvReader
    {
        private const char Delimiter = ';';

        public IReadOnlyCollection<string[]> ReadAll(string filePath)
        {
            return File.ReadAllLines(filePath)
                .Where(x => x.Length > 0)
                .Select(x => x.Split(Delimiter, StringSplitOptions.None))
                .ToArray();
        }
    }
}
