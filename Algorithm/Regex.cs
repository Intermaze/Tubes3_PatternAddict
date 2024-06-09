using System;
using System.Text.RegularExpressions;

namespace Tubes3
{
    public class RegularExpression
    {
    private (string, string)[] alayToNormalMap = new (string, string)[]{
            (@"4", "a"),
            (@"3", "e"),
            (@"1", "i"),
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
            (@"7", "t"),
            (@"6", "g"), //bisa 6/b
            (@"7", "j"),
            (@"9", "g"), 
        };

    public string ConvertAlayToNormal(string alayText)
    {
        string result = alayText;
        foreach (var (bahasaAlay, bahasaNormal) in alayToNormalMap)
        {
            result = Regex.Replace(result, bahasaAlay, bahasaNormal, RegexOptions.IgnoreCase);
            result = Regex.Replace(result, bahasaAlay, bahasaNormal);
        }

        // Konversi menjadi Capitalized Case
        // Konversi semua huruf besar menjadi kecilnya
        result = Regex.Replace(result, @"[A-Z]", (Match match) => match.ToString().ToLower());

        // Konversi semua huruf di awal kata menjadi huruf besar
        result = Regex.Replace(result, @"\b\w", (Match match)=> match.ToString().ToUpper());

        return result;
    }

    }
}
