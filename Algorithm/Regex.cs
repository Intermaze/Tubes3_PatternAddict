using System;
using System.Text.RegularExpressions;

class RegularExpression
{
	private (string, string)[] alayToNormalMap = new (string, string)[]{
            // Dalam spesifikasi, bahasa alay yang non-karakter berupa angka
            // Ada non-karakter alay lain selain angka, tetapi tidak dihandle disini (silahkan ditambahkan jika perlu)
            (@"13", "b"),
            (@"17", "d"),
            (@"0", "o"),
            (@"1", "i"), //bisa 1/l
            (@"2", "z"),
            (@"3", "e"),
            (@"4", "a"),
            (@"5", "s"),
            (@"6", "g"), //bisa 6/b
            (@"7", "j"),
            (@"9", "g"), 
        };

    public string ConvertAlayToNormal(string alayText)
    {
        string result = alayText;
        foreach (var (bahasaAlay, bahasaNormal) in alayToNormalMap)
        {
            result = Regex.Replace(result, bahasaAlay, bahasaNormal);
        }

        // Konversi menjadi Capitalized Case
        // Konversi semua huruf besar menjadi kecilnya
        result = Regex.Replace(result, @"[A-Z]", (Match match) => match.ToString().ToLower());

        // Konversi semua huruf di awal kata menjadi huruf besar
        result = Regex.Replace(result, @"\b\w", (Match match)=> match.ToString().ToUpper());

        return result;
    }

    // TODO: test dan masukkin ke main
    // Fungsi untuk konversi bahasa alay ke normal, lalu lakukan Algo.FindClosestMatch untuk mencari nama yang paling dekat di database

    // public List<(string, string, int)> CompareAlayWithDatabase(string alay, string[] database){
    //     List<(string, string, int)> result = new List<(string, string, int)>();
    //     string pattern = ConvertAlayToNormal(alay);

    //     foreach (var data in database){
    //         if (pattern.Equals(data)){
    //             result.Add((pattern, data, 0));
    //         }
    //         else{
    //             (string, int) closestMatch = FindClosestMatch(pattern, data);
    //             if (!string.IsNullOrEmpty(closestMatch.Item1)){
    //                 int distanceEachChar = Algo.CalculateCharDifference(pattern, closestMatch.Item1);
    //                 result.Add((closestMatch.Item1, data, distanceEachChar + closestMatch.Item2));
    //             }
    //             else{
    //                 Console.WriteLine($"{data} is not found. {pattern} is too long to compare");
    //             }
    //         }
    //     }

    //     result = result.OrderBy(tuple => tuple.Item3).ToList();
    //     return result;
    // }
}
