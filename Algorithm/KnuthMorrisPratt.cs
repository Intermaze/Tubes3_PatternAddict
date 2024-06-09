using System;
using System.Collections.Generic;

namespace Tubes3
{
    public class KnuthMorrisPratt
    {
        public List<(string, string, int)> process_all(
            string pattern_string,
            List<string> database,
            int[] lps
        )
        {
            List<(string, string, int)> result = new List<(string, string, int)>();
            
            foreach (var data in database)
            {
                bool patternFound = KMPSearch(pattern_string, data, lps);
                if(patternFound) result.Add((pattern_string, data, 0));
            }

            if(result.Count() == 0){
                foreach (var data in database)
                {

                    (string, int) closestMatch = Util.FindClosestMatch(pattern_string, data);
                    if (closestMatch.Item1 != "")
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

        bool KMPSearch(string pattern_string, string string_to_compare, int[] least_prefix_suffix)
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

        public void generate_lps(string pattern, int length, int[] ans)
        {
            int len = 0;
            int idx = 1;

            ans[0] = 0; 

            while (idx < length)
            {
                if (pattern[idx] == pattern[len])
                {
                    len++;
                    ans[idx] = len;
                    idx++;
                }
                else
                {
                    if (len != 0) len = ans[len - 1];
                    else ans[idx++] = 0;
                }
            }
        }
    }
}
