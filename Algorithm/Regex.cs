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

            // Separate the alay text into words
            string[] words = result.Split(' ');

            // Convert each word from alay to normal form
            for (int i = 0; i < words.Length; i++)
            {
                // Convert each alay sequence to its normal counterpart
                foreach (var (bahasaAlay, bahasaNormal) in alayToNormalMap)
                {
                    words[i] = Regex.Replace(words[i], bahasaAlay, bahasaNormal, RegexOptions.IgnoreCase);
                }

                words[i] = words[i].ToLower();
                words[i] = Regex.Replace(words[i], "[aeiou]", string.Empty, RegexOptions.IgnoreCase);
            }

            // Reconstruct the result with preserved spaces and non-alay characters
            result = string.Join(" ", words);

            return result;
        }


        private string GenerateRegexPattern(string normal)
        {
            // Remove vowels from the normal string and normalize to lowercase
            normal = Regex.Replace(normal, "[aeiou]", string.Empty, RegexOptions.IgnoreCase).ToLower();
            
            // Create regex pattern to match the preprocessed normal string
            string pattern = string.Join("[aeiou0-9]*", normal.ToCharArray());
            return pattern;
        }

        public bool IsMatch(string normal, string abnormal)
        {
            // Preprocess the abnormal string (convert leetspeak to original and remove vowels)
            string preprocessedAbnormal = ConvertAlayToNormal(abnormal);
            
            // Generate the regex pattern from the normal string
            string pattern = GenerateRegexPattern(normal);
            
            // Check if the preprocessed abnormal string matches the pattern
            return Regex.IsMatch(preprocessedAbnormal, pattern, RegexOptions.IgnoreCase);
        }
    }
}
