using System;
using System.ComponentModel.DataAnnotations;


namespace Tubes3
{
    public static class Util
    {
        public static int CalculateHammingDistance(string pattern, string text)
        {
            int minLength = Math.Min(pattern.Length, text.Length);
            int differenceCount = 0;

            for (int i = 0; i < minLength; i++)
            {
                if (pattern[i] != text[i])
                {
                    differenceCount++;
                }
            }
            // Count the extra characters in the longer string as differences
            differenceCount += Math.Abs(pattern.Length - text.Length);

            return differenceCount;
        }

        public static int CalculateCharDifference(string pattern, string text)
        {
            int minLength = Math.Min(pattern.Length, text.Length);
            int differenceCount = 0;
            differenceCount +=  (Math.Abs(pattern.Length - text.Length) * 10);
            for (int i = 0; i < minLength; i++)
            {
                differenceCount += CalculateCharDistance(pattern[i], text[i]);
            }
            // differenceCount += Math.Abs(pattern.Length - text.Length);

            return differenceCount;
        }


    public static int CalculateCharDistance(char a, char b)
    {
        var data = new Dictionary<char, char>
        {
            {'A', '4'}, {'a', '4'}, 
            {'e', '3'}, {'E', '3'}, 
            {'i', '1'}, {'I', '1'},
            {'o', '0'}, {'O', '0'},
            {'g', '6'}, {'G', 'g'},
            {'T', '7'}, {'t', '7'},
            {'S', '5'}, {'s', '5'}
        };

        // Direct replacement lookup
        if (data.TryGetValue(a, out char mappedA) && mappedA == b || 
            data.TryGetValue(b, out char mappedB) && mappedB == a)
        {
            return 0;
        }

        // Handling letter distances
        if (char.IsLetter(a) && char.IsLetter(b))
        {
            int distance = Math.Abs(char.ToLower(a) - char.ToLower(b));
            int circularDistance = Math.Min(distance, 26 - distance);
            return circularDistance;
        }

        // Handling letter vs. non-letter cases
        if (char.IsLetter(a))
        {
            int distance = Math.Abs(char.ToLower(a) - b);
            int circularDistance = Math.Min(distance, 128 - distance);
            return circularDistance;
        }
    
        if (char.IsLetter(b))
        {
            int distance = Math.Abs(char.ToLower(b) - a);
            int circularDistance = Math.Min(distance, 128 - distance);
            return circularDistance;
        }

        // If none of the above, return a high distance
        return 1000;
    }

        public static (string, int) FindClosestMatch(string pattern, string text)
        {
            int patternLength = pattern.Length;
            int textLength = text.Length;
            int minDifference = int.MaxValue;
            string closestMatch = "";

            for (int i = 0; i <= textLength - patternLength; i++)
            {
                string substring = text.Substring(i, patternLength);
                // int difference = CalculateHammingDistance(pattern, substring);
                // int difference = CalculateLevenshteinDistance(pattern, substring, patternLength, substring.Length);
                int difference = CalculateLevenshteinDistanceWithChar(pattern, substring, patternLength, substring.Length);

                if (difference < minDifference)
                {
                    minDifference = difference;
                    closestMatch = substring;
                }
            }

            return (closestMatch, minDifference);
        }

        public static int CalculateLevenshteinDistance(string str1, string str2, int m, int n)
        {
            // Create a matrix to store the distances
            int[,] distances = new int[m + 1, n + 1];

            // Initialize the first row and column of the matrix
            for (int i = 0; i <= m; i++)
            {
                distances[i, 0] = i;
            }

            for (int j = 0; j <= n; j++)
            {
                distances[0, j] = j;
            }

            // Calculate the distances for the remaining cells
            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    // If the characters are the same, no operation is needed
                    if (str1[i - 1] == str2[j - 1])
                    {
                        distances[i, j] = distances[i - 1, j - 1];
                    }
                    else
                    {
                    // Calculate the minimum of three operations: Insert, Remove, and Replace
                        distances[i, j] = 1 + Math.Min(
                        Math.Min(distances[i, j - 1], distances[i - 1, j]),
                        distances[i - 1, j - 1]
                    );
                    }
                }
            }

            // Return the distance between the two strings
            return distances[m, n];
        }

        //kombinasi dengan CalculateCharDifference kopas yang atas doang.
        //Cuman ketambahan calculateCharDistance
        public static int CalculateLevenshteinDistanceWithChar(string str1, string str2, int m, int n)
        {
            // Create a matrix to store the distances
            int[,] distances = new int[m + 1, n + 1];

            for (int i = 0; i <= m; i++)
            {
                distances[i, 0] = i;
            }

            for (int j = 0; j <= n; j++)
            {
                distances[0, j] = j;
            }

            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    // Calculate substitution cost with character differences
                    int cost = (str1[i - 1] == str2[j - 1]) ? 0 : CalculateCharDistance(str1[i - 1], str2[j - 1]);
                    // Calculate the minimum of three operations: Insert, Remove, and Replace
                    distances[i, j] = Math.Min(
                        Math.Min(distances[i, j - 1] + 1, distances[i - 1, j] + 1),
                        distances[i - 1, j - 1] + cost
                    );
                }
            }

            // Return the distance between the two strings
            return distances[m, n];
        }
    }

}
