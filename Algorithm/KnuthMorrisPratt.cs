using System;
using System.Collections.Generic;

namespace Tubes3
{
    public class KnuthMorrisPratt : IPatternMatchingAlgorithm
    {
        public List<(string, string, int)> ProcessAll(string pattern, List<string> database)
        {
            List<(string, string, int)> result = new List<(string, string, int)>();
            int[] lps = GenerateLPS(pattern);
            foreach (var data in database)
            {
                if (KMPSearch(pattern, data, lps))
                    result.Add((pattern, data, 0));
            }
            if (result.Count == 0)
            {
                foreach (var data in database)
                {
                    (string, int) closestMatch = Util.FindClosestMatch(pattern, data);
                    if (!string.IsNullOrEmpty(closestMatch.Item1))
                    {
                        result.Add((closestMatch.Item1, data, closestMatch.Item2));
                    }
                }
            }
            return result.OrderBy(tuple => tuple.Item3).ToList();
        }

        static bool KMPSearch(string pattern_string, string string_to_compare, int[] least_prefix_suffix)
        {
            int first_length = pattern_string.Length;
            int second_length = string_to_compare.Length;

            int idx_first = 0; 
            int idx_second = 0;
            while (idx_second < second_length)
            {
                if (pattern_string[idx_first] == string_to_compare[idx_second])
                {
                    idx_first++;
                    idx_second++;
                }
                if (idx_first == first_length)
                {
                    return true;
                }
                else if (
                    idx_second < second_length
                    && pattern_string[idx_first] != string_to_compare[idx_second]
                )
                {
                    if (idx_first != 0) idx_first = least_prefix_suffix[idx_first - 1];
                    else idx_second = idx_second + 1;
                }
            }
            return false;
        }

        private static int[] GenerateLPS(string pattern)
        {
            int m = pattern.Length;
            int[] lps = new int[m];
            int length = 0;
            lps[0] = 0;
            int i = 1;
            while (i < m)
            {
                if (pattern[i] == pattern[length])
                {
                    length++;
                    lps[i] = length;
                    i++;
                }
                else
                {
                    if (length != 0)
                    {
                        length = lps[length - 1];
                    }
                    else
                    {
                        lps[i] = 0;
                        i++;
                    }
                }
            }
            return lps;
        }

        static int[] GenerateLPS2(string pattern)
        {
            int m = pattern.Length;
            int[] lps = new int[m];
            lps[0] = 0;
            int i = 1;
            int j = 1;
            int t = 0;
            while (j < m)
            {
                while (t > 0 && pattern[j] != pattern[t])
                    t = lps[t - 1];
                t = t + 1;
                j = j + 1;
                if (pattern[j - 1] == pattern[t - 1])
                    lps[j - 1] = t;
                else
                    lps[j - 1] = t;
            }
            return lps;
        }
    }
}
