using System;

class BoyerMoore
{

    public void ProcessAll(string pattern, string[] database)
    {
        foreach (var data in database)
        {
            if (BoyerMooreSearch(pattern, data) == -1)
            {
                string closestMatch = FindClosestMatch(pattern, data);
                if(closestMatch != ""){
                    int distance = CalculateCharDifference(pattern, closestMatch);
                    Console.WriteLine($"{data} is not found. Closest match is {closestMatch} with {distance} character differences.");
                }else{
                    Console.WriteLine($"{data} is not found. {pattern} is too long to compare");
                }
            }
            else
            {
                Console.WriteLine($"{pattern} found in {data}");
            }
        }
    }

    int BoyerMooreSearch(string pattern, string text)
    {
        int m = pattern.Length;
        int n = text.Length;

        int[] badChar = BuildBadCharacterTable(pattern);
        int[] goodSuffix = BuildGoodSuffixTable(pattern);

        int s = 0;  // s is shift of the pattern with respect to text
        while (s <= (n - m))
        {
            int j = m - 1;

            // Keep reducing index j of pattern while characters of
            // pattern and text are matching at this shift s
            while (j >= 0 && pattern[j] == text[s + j])
                j--;

            // If the pattern is present at the current shift, then index j will become -1 after the above loop
            if (j < 0)
            {
                Console.WriteLine($"Pattern occurs at shift = {s}");
                s += (s + m < n) ? m - badChar[text[s + m]] : 1;
                return s;
            }
            else
            {
                s += Math.Max(1, j - badChar[text[s + j]]);
            }
        }
        return -1;
    }

    int[] BuildBadCharacterTable(string pattern)
    {
        int[] badChar = new int[256];
        int m = pattern.Length;

        // Initialize all occurrences as -1
        for (int i = 0; i < 256; i++)
            badChar[i] = -1;

        // Fill the actual value of last occurrence
        for (int i = 0; i < m; i++)
            badChar[(int)pattern[i]] = i;

        return badChar;
    }

    int[] BuildGoodSuffixTable(string pattern)
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
                suffix[i] = i;
            else
                suffix[i] = m - suffix[i];
        }

        for (int i = 0; i < m - 1; i++)
        {
            goodSuffix[m - 1 - suffix[i]] = m - 1 - i;
        }

        return goodSuffix;
    }

    int CalculateCharDifference(string pattern, string text)
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

    int CalculateCharDistance(char a, char b)
    {
        return Math.Abs(a - b);
    }

    string FindClosestMatch(string pattern, string text)
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

        return closestMatch;
    }

    int CalculateHammingDistance(string pattern, string text)
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
        differenceCount += Math.Abs(pattern.Length - text.Length);

        return differenceCount;
    }
}
