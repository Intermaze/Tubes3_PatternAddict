using System;
using System.Text.RegularExpressions;

namespace Tubes3
{
    public class RegularExpression
    {
        private (string, string)[] alayToNormalMap = new (string, string)[]
        {
            // Alay to normal character mappings
            // (@"13", "b"),
            // (@"17", "d"),
            (@"0", "o"),
            (@"1", "i"), // 1 can also be l
            (@"2", "z"),
            (@"3", "e"),
            (@"4", "a"),
            (@"5", "s"),
            (@"6", "g"), // 6 can also be b
            (@"7", "j"),
            (@"9", "g")
        };

        public string ConvertAlayToNormal(string alayText)
        {
            string result = alayText;
            string[] words = result.Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                foreach (var (bahasaAlay, bahasaNormal) in alayToNormalMap)
                {
                    words[i] = Regex.Replace(words[i], bahasaAlay, bahasaNormal, RegexOptions.IgnoreCase);
                }

                words[i] = words[i].ToLower();
                words[i] = Regex.Replace(words[i], "[aeiou]", string.Empty, RegexOptions.IgnoreCase);
            }
            result = string.Join(" ", words);
            return result;
        }


        private string GenerateRegexPattern(string normal)
        {
            normal = Regex.Replace(normal, "[aeiou]", string.Empty, RegexOptions.IgnoreCase).ToLower();
            string pattern = string.Join("[aeiou0-9]*", normal.ToCharArray());
            return pattern;
        }

        public bool IsMatch(string normal, string abnormal)
        {
            string preprocessedAbnormal = ConvertAlayToNormal(abnormal);
            string pattern = GenerateRegexPattern(normal);
            return Regex.IsMatch(preprocessedAbnormal, pattern, RegexOptions.IgnoreCase);
        }

        public static void main(){
            string abnormal = "Anton";
            string normal = "Toni";
            Console.WriteLine(Regex.IsMatch(normal, abnormal));
        }
    }
}
