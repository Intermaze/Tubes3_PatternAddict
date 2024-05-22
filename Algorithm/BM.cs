using System;

class BoyerMoore
{

    /*
    hampir sama kayak KMP
    */
    public List<(string, string, int)> ProcessAllBoyerMoore(string pattern, string[] database){
        List<(string, string, int)> result = new List<(string, string, int)>();

        foreach (var data in database){
            int patternIndex = BoyerMooreSearch(pattern, data);
            if (patternIndex == -1){
                string closestMatch = FindClosestMatch(pattern, data);
                if (!string.IsNullOrEmpty(closestMatch)){
                    int distanceEachChar = CalculateCharDifference(pattern, closestMatch);
                    result.Add((closestMatch, data, distanceEachChar));
                }
                else{
                    Console.WriteLine($"{data} is not found. {pattern} is too long to compare");
                }
            }
            else{
                result.Add((pattern, data, 0));
            }
        }

        result = result.OrderBy(tuple => tuple.Item3).ToList();
        return result;
    }

    int BoyerMooreSearch(string pattern, string text){
        int m = pattern.Length;
        int n = text.Length;

        int[] badChar = BuildBadCharacterTable(pattern);
        int[] goodSuffix = BuildGoodSuffixTable(pattern);

        int s = 0;  // s is shift of the pattern with respect to text
        while (s <= (n - m)){
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

    int[] BuildBadCharacterTable(string pattern){
        int[] badChar = new int[256];
        int m = pattern.Length;

        for (int i = 0; i < 256; i++)
            badChar[i] = -1;

        for (int i = 0; i < m; i++)
            badChar[(int)pattern[i]] = i;

        return badChar;
    }

    int[] BuildGoodSuffixTable(string pattern){
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

        for (int i = 0; i < m; i++){
            if(suffix[i] == m){
                suffix[i] = i;

            }
            else{
                suffix[i] = m - suffix[i];
            }
        }
        for (int i = 0; i < m - 1; i++){
            goodSuffix[m - 1 - suffix[i]] = m - 1 - i;
        }

        return goodSuffix;
    }

    /*
    sama kayak yang di KMP, harusnya jadiin satu cuman bingung CS
    ribet woey.
    */
    int CalculateCharDifference(string pattern, string text){
        int minLength = Math.Min(pattern.Length, text.Length);
        int differenceCount = 0;

        for (int i = 0; i < minLength; i++)
        {
            differenceCount += CalculateCharDistance(pattern[i], text[i]);
        }
        differenceCount += Math.Abs(pattern.Length - text.Length);

        return differenceCount;
    }

    int CalculateCharDistance(char a, char b){
        return Math.Abs(a - b);
    }

    string FindClosestMatch(string pattern, string text){
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

    int CalculateHammingDistance(string pattern, string text){
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
