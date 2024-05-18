using System;
 
class StringMatching {


    void process_all(string pattern_string, string[] database, int[] lps){
        foreach(var data in database){
            if(KMPSearch(pattern_string, data, lps) == false){
                Console.Write(data + " is not found");
            }
            Console.WriteLine("");
        }
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
        Console.Write("Suffix array: "); 
        foreach(var item in least_prefix_suffix){
            Console.Write(item + " ");
        }

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
                Console.Write(pattern_string + " Found pattern " + "at index " + (idx_second - idx_first));
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
 

    void generate_lps(string pattern, int length, int[] ans){
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

    // Driver program to test above function
    public static void Main()
    {
        string pattern = "ABABCABAB";
        int[] array_of_lps = new int[pattern.Length];
        new StringMatching().generate_lps(pattern, pattern.Length, array_of_lps);
        string[] database = { "ABABDABACDABABCABAB", "ABAB", "ACDABABCABAB", "ABCDABABABCABAB", "ABABABCABAB" };
        new StringMatching().process_all(pattern, database, array_of_lps);
    }
}
