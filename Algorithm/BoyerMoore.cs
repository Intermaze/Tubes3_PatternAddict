using System;
using System.Globalization;

namespace Tubes3
{
    public class BoyerMoore
    {
        /*
        hampir sama kayak KMP
        */
        public List<(string, string, int)> ProcessAllBoyerMoore(string pattern, List<string> database)
        {
            List<(string, string, int)> result = new List<(string, string, int)>();

            foreach (var data in database)
            {
                int patternIndex = BoyerMooreSearch(pattern, data);
                if (patternIndex != -1) result.Add((pattern, data, 0));

            }

            if(result.Count() == 0){
                foreach (var data in database){
                    (string, int) closestMatch = Util.FindClosestMatch(pattern, data);
                    if (!string.IsNullOrEmpty(closestMatch.Item1))
                    {
                        result.Add(
                            (closestMatch.Item1, data, closestMatch.Item2)
                        );
                    }
                }
            }

            result = result.OrderBy(tuple => tuple.Item3).ToList();
            return result;
        }

        private int BoyerMooreSearch(string pattern, string text)
        {
            int m = pattern.Length;
            int n = text.Length;

            int[] badChar = BuildBadCharacterTable(pattern);
            int[] goodSuffix = BuildGoodSuffixTable(pattern);

            int s = 0; // s is shift of the pattern with respect to text
            while (s <= (n - m))
            {
                int j = m - 1;
                while (j >= 0 && pattern[j] == text[s + j]) j--;
                if (j < 0)
                {
                    s += (s + m < n) ? m - badChar[text[s + m]] : 1;
                    return s;
                }
                else s += Math.Max(1, j - badChar[text[s + j]]);
            }
            return -1;
        }
        
        private int BoyerMooreSearchGoodchar(string pattern, string text)
        {
            int m = pattern.Length;
            int n = text.Length;

            int[] badChar = BuildBadCharacterTable(pattern);
            int[] goodSuffix = BuildGoodSuffixTable(pattern);

            int s = 0; // s is the shift of the pattern with respect to the text
            while (s <= (n - m))
            {
                int j = m - 1;
                while (j >= 0 && pattern[j] == text[s + j]) j--;
                if (j < 0) return s;
                else
                {
                    // Calculate the shift with both bad character and good suffix rules
                    s += Math.Max(1, Math.Max(j - badChar[text[s + j]], goodSuffix[j]));
                }
            }
            return -1;
        }
        private int BoyerMooreSearchLookingGlass(string pattern, string text)
        {
            int m = pattern.Length;
            int n = text.Length;

            int[] lookingGlass = BuildLookingGlass(pattern);

            int s = 0; 
            while (s <= (n - m))
            {
                int j = m - 1;
                while (j >= 0 && pattern[j] == text[s + j]) j--;
                if (j < 0)
                {
                    s += (s + m < n) ? lookingGlass[0] : 1;
                    return s;
                }
                else
                {
                    int lookingGlassShift = lookingGlass[j];
                    s += lookingGlassShift;
                }
            }
            return -1;
        }

        private int[] BuildBadCharacterTable(string pattern)
        {
            int[] badChar = new int[256];
            int m = pattern.Length;

            for (int i = 0; i < 256; i++)
                badChar[i] = -1;

            for (int i = 0; i < m; i++)
                badChar[(int)pattern[i]] = i;

            return badChar;
        }

        private int[] BuildGoodSuffixTable(string pattern)
        {
            int m = pattern.Length;
            int[] suffix = new int[m];
            int[] goodSuffix = new int[m];

            // Initialize all occurrences as -1
            for (int i = 0; i < m; i++)
                suffix[i] = -1;

            for (int i = m - 1, j = m; i >= 0; i--)
            {
                if (i == m - 1 || suffix[i + 1] == -1)
                    j = i + 1;
                suffix[i] = j;
                if (i > 0 && pattern[i - 1] == pattern[m - 1])
                    j = i;
            }

            for (int i = 0; i < m; i++)
            {
                if (suffix[i] == m)
                {
                    suffix[i] = i;
                }
                else
                {
                    suffix[i] = m - suffix[i];
                }
            }
            for (int i = 0; i < m - 1; i++)
            {
                goodSuffix[m - 1 - suffix[i]] = m - 1 - i;
            }

            return goodSuffix;
        }
    private int[] BuildLookingGlass(string pattern)
    {
        int[] lookingGlassTable = new int[pattern.Length];

        for(int i = 0; i < pattern.Length; i++)
        {
            lookingGlassTable[i] = pattern.Length;
        }

        for (int i = 0; i < pattern.Length - 1; i++)
        {
            lookingGlassTable[pattern.Length - 1 - i] = Math.Min(lookingGlassTable[pattern.Length - 1 - i], i);
        }

        return lookingGlassTable;
    }
    }

}
