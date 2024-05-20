using System;

class MainProgram
{
    public static void Main()
    {
        // Boyer-Moore
        Console.WriteLine("Boyer-Moore: ");
        string patternBM = "AAA";
        string[] databaseBM = { "ABCBDABCCDABCBCABCB", "ACXB", "ACDABABCABAB", "ABCDABABABCABAB", "ABABABCABAB", "CCCAZA"};
        BoyerMoore bm = new BoyerMoore();
        bm.ProcessAll(patternBM, databaseBM);

        Console.WriteLine("\n================================================\n");

        // Knuth-Morris-Pratt
        Console.WriteLine("KMP: ");
        KnuthMorrisPratt kmp = new KnuthMorrisPratt();
        string pattern = "???";
        int[] array_of_lps = new int[pattern.Length];
        kmp.generate_lps(pattern, pattern.Length, array_of_lps);
        string[] database = { "A?ACL?KJSDF", "FAXSDFF???KFAFDB", "ACDSFS?DFL?:';FABBCSADFAFBAB", "AFCBCD?:>,SFLK{}:FDADFAJFBASFAFHJBASDFFAB", "AFBAFFBAGJ;',.][BSHGJDFFDCABGHJAB", "CCGH[];',.JCADFGJHGGJHA"};
        List<(string, string,  int)> closestMatches = kmp.process_all(pattern, database, array_of_lps);
        foreach(var match in closestMatches) {
            if(match.Item3 == 0){
                Console.WriteLine($"MATCH: {match.Item1}, text: {match.Item2}, Character difference: {match.Item3}");
            }else{
                Console.WriteLine($"Closest match: {match.Item1}, text: {match.Item2}, Character difference: {match.Item3}");
            }
        }
    }
}
