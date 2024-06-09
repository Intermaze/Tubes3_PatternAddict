using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Data.Sqlite;

namespace Tubes3
{
    public class Database
    {
        private static string ddl =
            @"
            CREATE TABLE IF NOT EXISTS biodata (
                nama VARCHAR(100),
                tempat_lahir VARCHAR(50),
                tanggal_lahir DATE,
                jenis_kelamin VARCHAR(16) CHECK(jenis_kelamin IN ('Laki-Laki','Perempuan', NULL)),
                golongan_darah VARCHAR(5),
                alamat VARCHAR(255),
                agama VARCHAR(50),
                status_perkawinan VARCHAR(16) CHECK(status_perkawinan IN ('Belum Menikah', 'Menikah', 'Cerai')),
                pekerjaan VARCHAR(100),
                kewarganegaraan VARCHAR(50)
            );

            CREATE TABLE IF NOT EXISTS sidik_jari (
                    nama VARCHAR(100),
                    berkas_citra TEXT
            );
            ";
        private static SqliteConnection connection = null;

        public static void Initialize()
        {
            ExecuteNonQuery(ddl);
        }

        private static void OpenConnection()
        {
            if (connection == null)
            {
                connection = new SqliteConnection("Data Source=../data.db");
            }
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
        }

        private static void ExecuteNonQuery(string commandText, params (string, object)[] parameters)
        {
            OpenConnection();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Item1, param.Item2);
                }
                command.ExecuteNonQuery();
            }
        }

        public static void InsertBiodata(Biodata bio)
        {
            ExecuteNonQuery(@"
                INSERT INTO biodata (
                    nama, 
                    tempat_lahir, 
                    tanggal_lahir, 
                    jenis_kelamin, 
                    golongan_darah, 
                    alamat, 
                    agama, 
                    status_perkawinan, 
                    pekerjaan, 
                    kewarganegaraan
                ) VALUES (
                    $name, 
                    $birthDate, 
                    $birthPlace, 
                    $gender, 
                    $bloodType, 
                    $address, 
                    $religion, 
                    $maritalStatus, 
                    $job, 
                    $nationality
                );",
                ("$name", bio.nama),
                ("$birthDate", bio.tanggal_lahir),
                ("$birthPlace", bio.tempat_lahir),
                ("$gender", bio.jenis_kelamin),
                ("$bloodType", bio.golongan_darah),
                ("$address", bio.alamat),
                ("$religion", bio.agama),
                ("$maritalStatus", bio.status_perkawinan),
                ("$job", bio.pekerjaan),
                ("$nationality", bio.kewarganegaraan)
            );
        }

        public static void InsertFingerprint(string name, string fingerprint)
        {
            ExecuteNonQuery(@"
                INSERT INTO sidik_jari (nama, berkas_citra)
                VALUES ($name, $fingerprint);",
                ("$name", name),
                ("$fingerprint", fingerprint)
            );
        }

        public static void ChangeFingerprintName(string from, string to)
        {
            ExecuteNonQuery(@"
                UPDATE sidik_jari 
                SET nama = $to
                WHERE nama = $from;",
                ("$from", from),
                ("$to", to)
            );
        }

        public static void ChangeBiodataName(string from, string to)
        {
            ExecuteNonQuery(@"
                UPDATE biodata
                SET nama = $to
                WHERE nama = $from;",
                ("$from", from),
                ("$to", to)
            );
        }

        private static List<T> GetData<T>(string commandText, Func<SqliteDataReader, T> parseFunc)
        {
            OpenConnection();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                using (var reader = command.ExecuteReader())
                {
                    var result = new List<T>();
                    while (reader.Read())
                    {
                        result.Add(parseFunc(reader));
                    }
                    return result;
                }
            }
        }

        private static List<Fingerprint> GetFingerprints()
        {
            return GetData("SELECT * FROM sidik_jari;", ParseFingerprint);
        }

        private static List<Biodata> GetBiodata()
        {
            return GetData("SELECT * FROM biodata;", ParseBiodata);
        }

        public static Fingerprint ParseFingerprint(SqliteDataReader r)
        {
            return new Fingerprint
            {
                nama = (string)r["nama"],
                berkas_citra = (string)r["berkas_citra"]
            };
        }

        public static Biodata ParseBiodata(SqliteDataReader r)
        {
            return new Biodata
            {
                nama = (string)r["nama"],
                alamat = (string)r["alamat"],
                golongan_darah = (string)r["golongan_darah"],
                status_perkawinan = (string)r["status_perkawinan"],
                kewarganegaraan = (string)r["kewarganegaraan"],
                tanggal_lahir = (string)r["tanggal_lahir"],
                tempat_lahir = (string)r["tempat_lahir"],
                pekerjaan = (string)r["pekerjaan"],
                jenis_kelamin = (string)r["jenis_kelamin"],
                agama = (string)r["agama"]
            };
        }

        public static string FixFingerprint()
        {
            var listFingerprint = GetFingerprints();
            var listBiodata = GetBiodata();
            var listBiodataString = new List<string>();

            foreach (var bio in listBiodata)
            {
                listBiodataString.Add(bio.nama);
            }

            KnuthMorrisPratt kmp = new KnuthMorrisPratt();
            foreach (var fingerprint in listFingerprint)
            {
                var result = kmp.ProcessAll(fingerprint.nama, listBiodataString);
                foreach (var item in result)
                {
                    // Console.WriteLine("Pattern: " + fingerprint.nama + " | Closest Match: " + item.Item1 + " | Text: " + item.Item2 + " | distance: " + item.Item3);
                    return item.Item1;
                }
            }
            return null;
        }

        // public static bool PatternExists(string pattern, List<string> textList, int maxDistance)
        // {
        //     foreach (var text in textList)
        //     {
        //         if (Util.CalculateLevenshteinDistance(pattern, text) <= maxDistance)
        //         {
        //             return true;
        //         }
        //     }
        //     return false;

        // }


        private static (Biodata, string, float) CompareFingerprint(string image, string imageBin, Func<string, List<string>, List<(string, string, int)>> matchFunc)
        {
            var listFingerprint = GetFingerprints();
            List<Biodata> listBiodata = GetBiodata();

            // var listBiodataString = new List<string>();
            // foreach (var bio in listBiodata)
            // {
            //     listBiodataString.Add(bio.nama);
            // }

            var listFingerprintASCII = new List<Fingerprint>();
            foreach (var item in listFingerprint)
            {
                listFingerprintASCII.Add(new Fingerprint
                {
                    nama = item.nama,
                    berkas_citra = Converter.ImageToAsciiStraight(item.berkas_citra)
                });
            }

            var listFingerprintString = new List<string>();
            foreach (var fingerprint in listFingerprintASCII)
            {
                listFingerprintString.Add(fingerprint.berkas_citra);
            }
            
            RegularExpression regex = new RegularExpression();
            List<String> listNamaNormal = new List<string>();
            foreach (var biodata in listBiodata){
                var namaNormal = regex.ConvertAlayToNormal(biodata.nama);
                listNamaNormal.Add(namaNormal);
            }
            
            string path = null;
            Biodata ans = null;

            var result = matchFunc(image, listFingerprintString);
            foreach (var fingerprint in listFingerprintASCII)
            {
                if (fingerprint.berkas_citra == result[0].Item1)
                {
                    var resultNama = matchFunc(fingerprint.nama, listNamaNormal);
                    string foundNama = resultNama[0].Item2;
                    int foundNamaIdx = listNamaNormal.FindIndex(x => x.Equals(foundNama));
                    ans = listBiodata[foundNamaIdx];    

                    Console.WriteLine(resultNama[0]);
                    // Console.WriteLine(resultNama[1]);
                    // Console.WriteLine(resultNama[2]);

                    Console.WriteLine("Nama fingerprint: " + fingerprint.nama);
                    Console.WriteLine("Nama biodata: " + ans.nama);
                    ans.nama = fingerprint.nama;

                    foreach (var fingerpath in listFingerprint)
                    {
                        if (fingerpath.nama == fingerprint.nama)
                        {
                            path = fingerpath.berkas_citra;
                            break;
                        }
                    }
                    break;
                }
            }

            float percentage;
            string patternBin = Converter.ImageToBin(path);
            percentage = (float)(30 - Util.CalculateHammingDistance(patternBin, imageBin)) / 30 * 100;

            if (ans != null && path != null)
            {
                // Console.WriteLine("Path: " + path);
                // Console.WriteLine("Hasil: ");
                // Console.WriteLine("Nama: " + ans.nama);
                // Console.WriteLine("Alamat: " + ans.alamat);
                // Console.WriteLine("pekerjaan: " + ans.pekerjaan);
                // Console.WriteLine("tanggal_lahir: " + ans.tanggal_lahir);
                // Console.WriteLine("tempat_lahir: " + ans.tempat_lahir);
                // Console.WriteLine("kewarganegaraan: " + ans.kewarganegaraan);
                // Console.WriteLine("agama: " + ans.agama);
                return (ans, path, percentage);
            }
            return (null, null, 0);
        }

        public static (Biodata, string, float) CompareFingerprintKMP(string image, string imageBin)
        {
            KnuthMorrisPratt kmp = new KnuthMorrisPratt();
            int[] lcs = new int[image.Length];
            return CompareFingerprint(image, imageBin, kmp.ProcessAll);
        }

        public static (Biodata, string, float) CompareFingerprintBM(string image, string imageBin)
        {
            BoyerMoore bm = new BoyerMoore();
            return CompareFingerprint(image, imageBin, bm.ProcessAll);
        }
    }
}
