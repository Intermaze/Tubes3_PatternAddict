using Tubes3;
using System.IO; 

Database.Initialize(); 
Database.FixFingerprint();

// Converter.ImageToAscii("../Converter/test.jpg");
//
// // Boyer-Moore
// Console.WriteLine("Boyer-Moore: ");
// BoyerMoore bm = new BoyerMoore();
// string patternBM = "AAA";
// string[] databaseBM =
// {
//     "ABCBDABCCDABCBCABCB",
//     "AAAB",
//     "ACDABABCABAB",
//     "ABCDABABABCABAB",
//     "ABABABCABAB",
//     "CCCAZA"
// };
// List<(string, string, int)> closestMatchBM = bm.ProcessAllBoyerMoore(patternBM, databaseBM);
// foreach (var match in closestMatchBM)
// {
//     if (match.Item3 == 0)
//     {
//         Console.WriteLine(
//             $"MATCH: {match.Item1}, text: {match.Item2}, Character difference: {match.Item3}"
//         );
//     }
//     else
//     {
//         Console.WriteLine(
//             $"Closest match: {match.Item1}, text: {match.Item2}, Character difference: {match.Item3}"
//         );
//     }
// }
//
// Console.WriteLine("\n================================================\n");
//
// // Knuth-Morris-Pratt
// Console.WriteLine("KMP: ");
// KnuthMorrisPratt kmp = new KnuthMorrisPratt();
// string pattern = "???";
// int[] array_of_lps = new int[pattern.Length];
// kmp.generate_lps(pattern, pattern.Length, array_of_lps);
// string[] database =
// {
//     "A?AL?KJSDF",
//     "FAXSDFF???KFAFDB",
//     "ACDSFS?DFL?:';FABBCSADFAFBAB",
//     "AFCBCD?:>,SFLK{}:FDADFAJFBASFAFHJBASDFFAB",
//     "AFBAFFBAGJ;',.][BSHGJDFFDCABGHJAB",
//     "CCGH[];',.JCADFGJHGGJHA"
// };
// List<(string, string, int)> closestMatches = kmp.process_all(pattern, database, array_of_lps);
// foreach (var match in closestMatches)
// {
//     if (match.Item3 == 0)
//     {
//         Console.WriteLine(
//             $"MATCH: {match.Item1}, text: {match.Item2}, Character difference: {match.Item3}"
//         );
//     }
//     else
//     {
//         Console.WriteLine(
//             $"Closest match: {match.Item1}, text: {match.Item2}, Character difference: {match.Item3}"
//         );
//     }
// }
//
// Console.WriteLine("\n================================================\n");
//
// Console.WriteLine("Regex: ");
// RegularExpression r = new RegularExpression();
// string patternAlay = "H4Ll0, n4M4 54Y4 4d4l4h 1ndr4AA!";
// Console.WriteLine(patternAlay);
// Console.WriteLine(r.ConvertAlayToNormal(patternAlay));
// Console.WriteLine();
// patternAlay = "b1ntN6 Dw mrthn";
// Console.WriteLine(patternAlay);
// Console.WriteLine(r.ConvertAlayToNormal(patternAlay));
