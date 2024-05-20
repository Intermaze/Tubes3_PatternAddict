using System;
using System.Collections.Generic;


class KnuthMorrisPratt {

    public List<(string, string, int)> process_all(string pattern_string, string[] database, int[] lps){
        List<(string, string, int)> result = new List<(string, string, int)>();

        foreach (var data in database){
            bool patternFound = KMPSearch(pattern_string, data, lps);
            if (!patternFound){
                (string, int) closestMatch = FindClosestMatch(pattern_string, data);
                if(closestMatch.Item1 != ""){
                    //kalau gada yang sama, cari yang terdekat dengan hamming distance
                    // dan masukan ke dalam result beserta distance-nya 
                    int distanceEachChar = CalculateCharDifference(pattern_string, closestMatch.Item1);
                    //untuk sekarang buat distancenya hammingDistance + perbedaan distance dari tiap karakter 
                    result.Add((closestMatch.Item1, data, distanceEachChar + closestMatch.Item2));
                }else{
                    Console.WriteLine($"{data} is not found. {pattern_string} is too long to compare");
                }
            }
            else{
                //kalau ketemu masukin ke dalam list dengan perbedaan karakter sebesar 0 
                result.Add((pattern_string, data, 0));
            }
        }

        result = result.OrderBy(tuple => tuple.Item2).ToList();
        return result;
    }

    bool KMPSearch(string pattern_string, string string_to_compare, int[] least_prefix_suffix){
        /*
            inisiasi panjang masing-masing array
        */
        int first_length = pattern_string.Length;
        int second_length = string_to_compare.Length;

        /*
            buat sebuah array yang menampung suffix daripattern_stringg
        */
        int idx_first = 0; // index for handlingpattern_stringg

        /*
            fungsi untuk membuat suffix_array daripattern_string
        */ 
        // generate_lps(pattern_string, first_length, suffix_array);
        // Console.Write("Suffix array: "); 

        /*
            looping untuk melakukan perbandingan pada  kedua string
        */
        int idx_second = 0; 
        while (idx_second < second_length) {
            /*
                melakukan perbandingan pada 
            */
            if (pattern_string[idx_first] == string_to_compare[idx_second]) {
                idx_first++;
                idx_second++;
            }
            /*
                kalo idx_first sampai dengan panjangnya dari length dari pattern_string maka pattern ditemukan 
            */
            if (idx_first == first_length){
                // Console.Write(pattern_string + " Found pattern " + "at index " + (idx_second - idx_first));
                idx_first = least_prefix_suffix[idx_first - 1];
                return true;
            }

            else if (idx_second < second_length && pattern_string[idx_first] != string_to_compare[idx_second]) {
                if (idx_first != 0)
                    idx_first = least_prefix_suffix[idx_first - 1];
                else
                    idx_second = idx_second + 1;
            }
        }
        return false;
    }
 
    /**
        basically  yang ini ngitung kayak huruf yang beda dari awal sampai akhir 
        jadi misal 
        pattern AAA
        dibandingin sama CDE 
        nah ini bedanya 3 karena beda semua 
    */
    int CalculateHammingDistance(string pattern, string text) {
        int minLength = Math.Min(pattern.Length, text.Length);
        int differenceCount = 0;

        for (int i = 0; i < minLength; i++) {
            if (pattern[i] != text[i]) {
                differenceCount++;
            }
        }
        // Count the extra characters in the longer string as differences
        differenceCount += Math.Abs(pattern.Length - text.Length);

        return differenceCount;
    }

    /**
        kalau yang ini bandinginnya jarak alfabet
        jadi misal pattern A, tapi teksnya B 
        nah ini jaraknya satu karena dari alfabet A -> B emang distance-nya satu 
        kalau pattern A, tapi teksnya C 
        distance-nya ya dua.
    */
    int CalculateCharDifference(string pattern, string text) {
        int minLength = Math.Min(pattern.Length, text.Length);
        int differenceCount = 0;

        for (int i = 0; i < minLength; i++) {
            differenceCount += CalculateCharDistance(pattern[i], text[i]);
        }
        differenceCount += Math.Abs(pattern.Length - text.Length);

        return differenceCount;
    }

    int CalculateCharDistance(char a, char b) {
        int distance = Math.Abs(a - b);

        // Consider circular distance
        int circularDistance = Math.Min(distance, 128 - Math.Abs(a - b));
        
        return circularDistance;
    }

    (string, int) FindClosestMatch(string pattern, string text) {
        int patternLength = pattern.Length;
        int textLength = text.Length;
        int minDifference = int.MaxValue;
        string closestMatch = "";

        for (int i = 0; i <= textLength - patternLength; i++) {
            string substring = text.Substring(i, patternLength);
            int difference = CalculateHammingDistance(pattern, substring);

            if (difference < minDifference) {
                minDifference = difference;
                closestMatch = substring;
            }
        }
        return (closestMatch, minDifference);
    }

    public void generate_lps(string pattern, int length, int[] ans){
        int len = 0; 
        int idx = 1; 

        ans[0] = 0; // substring idx = 0 selalu 0 lps-nya 

        while(idx < length){
            //kalo pattern-nya sama 
            if(pattern[idx] == pattern[len]){
                len++; 
                ans[idx] = len; 
                idx++;
            }else{
                if(len != 0){
                    len = ans[len-1];
                }else{
                    ans[idx++] = 0; 
                }
            }
        }
    }
}
