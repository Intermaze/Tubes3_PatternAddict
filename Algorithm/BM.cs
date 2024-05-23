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
                (string, int) closestMatch = Algo.FindClosestMatch(pattern, data);
                if (!string.IsNullOrEmpty(closestMatch.Item1)){
                    int distanceEachChar = Algo.CalculateCharDifference(pattern, closestMatch.Item1);
                    result.Add((closestMatch.Item1, data, distanceEachChar + closestMatch.Item2));
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

    private int BoyerMooreSearch(string pattern, string text){
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

    private int[] BuildBadCharacterTable(string pattern){
        int[] badChar = new int[256];
        int m = pattern.Length;

        for (int i = 0; i < 256; i++)
            badChar[i] = -1;

        for (int i = 0; i < m; i++)
            badChar[(int)pattern[i]] = i;

        return badChar;
    }

    private int[] BuildGoodSuffixTable(string pattern){
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
}
