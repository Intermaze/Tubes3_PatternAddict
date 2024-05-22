using System;
using System.Text.RegularExpressions;

class RegularExpression
{
	private (string, string)[] alayToNormalMap = new (string, string)[]{
            (@"4", "a"),
            (@"3", "e"),
            (@"1", "i"),
            (@"0", "o"),
            (@"5", "s"),
            (@"7", "t"),
        };

    public string ConvertAlayToNormal(string alayText)
    {
        string result = alayText;
        foreach (var (bahasaAlay, bahasaNormal) in alayToNormalMap)
        {
            result = Regex.Replace(result, bahasaAlay, bahasaNormal, RegexOptions.IgnoreCase);
        }

        return result;
    }
}
