using System;

namespace Algorithm
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

            for (int i = 0; i < minLength; i++)
            {
                differenceCount += CalculateCharDistance(pattern[i], text[i]);
            }
            differenceCount += Math.Abs(pattern.Length - text.Length);

            return differenceCount;
        }

        public static int CalculateCharDistance(char a, char b)
        {
            int distance = Math.Abs(a - b);
            int circularDistance = Math.Min(distance, 128 - Math.Abs(a - b));
            return circularDistance;
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
                int difference = CalculateHammingDistance(pattern, substring);

                if (difference < minDifference)
                {
                    minDifference = difference;
                    closestMatch = substring;
                }
            }

            return (closestMatch, minDifference);
        }
    }
}
