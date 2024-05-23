using System;
using System.Collections.Generic;

class KnuthMorrisPratt {

    public List<(string, string, int)> process_all(string pattern_string, string[] database, int[] lps){
        List<(string, string, int)> result = new List<(string, string, int)>();

        
        foreach (var data in database){
            bool patternFound = KMPSearch(pattern_string, data, lps);
            if (!patternFound){
                (string, int) closestMatch = Algo.FindClosestMatch(pattern_string, data);
                if(closestMatch.Item1 != ""){
                    //kalau gada yang sama, cari yang terdekat dengan hamming distance
                    // dan masukan ke dalam result beserta distance-nya 
                    int distanceEachChar = Algo.CalculateCharDifference(pattern_string, closestMatch.Item1);
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

        result = result.OrderBy(tuple => tuple.Item3).ToList();
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
