using System.Collections.Generic;

namespace Tubes3
{
    public interface IPatternMatchingAlgorithm
    {
        List<(string, string, int)> ProcessAll(string pattern, List<string> database);
    }
}
