using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Tubes3; 

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
            using (Database.connection = new SqliteConnection("Data Source=../data.db"))
            {
                Database.connection.Open();
                var command = Database.connection.CreateCommand();
                command.CommandText = Database.ddl;
                command.ExecuteNonQuery();
            }
        }

        public static void InsertBiodata(Biodata bio)
        {
            Database.connection.Open(); 
            var command = Database.connection.CreateCommand(); 
            command.CommandText = 
            @"
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
                );
            ";

            command.Parameters.AddWithValue("$name", bio.nama); 
            command.Parameters.AddWithValue("$birthDate", bio.tanggal_lahir); 
            command.Parameters.AddWithValue("$birthPlace", bio.tempat_lahir); 
            command.Parameters.AddWithValue("$gender", bio.jenis_kelamin); 
            command.Parameters.AddWithValue("$bloodType", bio.golongan_darah); 
            command.Parameters.AddWithValue("$address", bio.alamat); 
            command.Parameters.AddWithValue("$religion", bio.agama); 
            command.Parameters.AddWithValue("$maritalStatus", bio.status_perkawinan); 
            command.Parameters.AddWithValue("$job", bio.pekerjaan); 
            command.Parameters.AddWithValue("$nationality", bio.kewarganegaraan);
            
            command.ExecuteNonQuery();
        }
        public static void InsertFingerprint(string name,  string fingerprint)
        {
            Database.connection.Open();
            var command = Database.connection.CreateCommand();
            command.CommandText =
                @"
						INSERT INTO sidik_jari (nama, berkas_citra)
						VALUES ($name, $fingerprint); 
						";

            command.Parameters.AddWithValue("$name", name);
            command.Parameters.AddWithValue("$fingerprint", fingerprint);

            command.ExecuteNonQuery();
        }

        public static void ChangeFingerprintName(string from, string to){
            Database.connection.Open();
            var command = Database.connection.CreateCommand();
            command.CommandText =
                @"
						UPDATE sidik_jari 
                        SET nama = $to
                        WHERE nama = $from;
						";

            command.Parameters.AddWithValue("$from", from);
            command.Parameters.AddWithValue("$to", to);

            command.ExecuteNonQuery();

        }

        public static void ChangeBiodataName(string from, string to){
            Database.connection.Open();
            var command = Database.connection.CreateCommand();
            command.CommandText =
                @"
						UPDATE biodata
                        SET nama = $to
                        WHERE nama = $from;
						";

            command.Parameters.AddWithValue("$from", from);
            command.Parameters.AddWithValue("$to", to);

            command.ExecuteNonQuery();

        }

        public static Fingerprint ParseFingerprint(SqliteDataReader r){
            var f = new Fingerprint(); 
            f.nama = (string)r["nama"];
            f.berkas_citra = (string)r["berkas_citra"]; 

            return f;
        }

        public static Biodata ParseBiodata(SqliteDataReader r){
            var b = new Biodata(); 

            b.nama = (string)r["nama"];
            b.alamat = (string)r["alamat"]; 
            b.golongan_darah = (string)r["golongan_darah"]; 
            b.status_perkawinan = (string)r["status_perkawinan"]; 
            b.kewarganegaraan = (string)r["kewarganegaraan"]; 
            b.tanggal_lahir = (string)r["tanggal_lahir"]; 
            b.tempat_lahir = (string)r["tempat_lahir"]; 
            b.pekerjaan = (string)r["pekerjaan"]; 
            b.jenis_kelamin = (string)r["jenis_kelamin"];
            b.agama = (string)r["agama"];

            return b; 
        }

        public static void FixFingerprint(){
            Database.connection.Open(); 
            var commandFingerprint = Database.connection.CreateCommand();
            commandFingerprint.CommandText = @"SELECT * FROM sidik_jari;";
            var readerFingerprint = commandFingerprint.ExecuteReader();
            List<Fingerprint> listFingerprint= new List<Fingerprint>();
            while(readerFingerprint.Read()){
                // Console.WriteLine(readerFingerprint["nama"]);
                Fingerprint fingerprint = ParseFingerprint(readerFingerprint);
                listFingerprint.Add(fingerprint);
            }

            var commandBiodata = Database.connection.CreateCommand(); 
            commandBiodata.CommandText = @"SELECT * FROM biodata;";
            var readerBiodata = commandBiodata.ExecuteReader(); 
            List<Biodata> listBiodata = new List<Biodata>(); 

            while(readerBiodata.Read()){
                Biodata biodata = ParseBiodata(readerBiodata); 
                listBiodata.Add(biodata);
            }

            List<string> listBiodataString = new List<string>(); 
            foreach(var bio in listBiodata){
                listBiodataString.Add(bio.nama);
            }

            List<string> listFingerprintString = new List<string>(); 
            foreach (var fingerprint in listFingerprint){
                listFingerprintString.Add(fingerprint.berkas_citra);
            }
            
            KnuthMorrisPratt kmp = new KnuthMorrisPratt();  
            foreach (var fingerprint in listFingerprint){
                /*MEMBANDINGKAN NAMANYA SAJA*/
                int[] lcsTemp = new int[fingerprint.nama.Length];
                kmp.generate_lps(fingerprint.nama, fingerprint.nama.Length, lcsTemp);
                List<(string, string, int)> result = kmp.process_all(fingerprint.nama, listBiodataString, lcsTemp);
                foreach(var item in result){
                    Console.WriteLine("Pattern: " + fingerprint.nama +  " | Closest Match: " + item.Item1 + " | Text: " + item.Item2 + " | distance: " + item.Item3);
                    break;
                }

                /*CHANGE USERNAME*/
                // int[] lcsTemp = new int[fingerprint.nama.Length]; 
                // kmp.generate_lps(fingerprint.nama, fingerprint.nama.Length, lcsTemp);
                // List<(string, string, int)> result = kmp.process_all(fingerprint.nama, listBiodataString, lcsTemp); 
                // ChangeFingerprintName(fingerprint.nama, result[0].Item2); 
                // Console.WriteLine("Berhasil mengganti: " + fingerprint.nama + " -> " + result[0].Item2);
            }
        }

        public static (Biodata, string) CompareFingerprintKMP(string image){
            //melakukan koneksi ke database
            Database.connection.Open(); 
            
            //Command untuk mengambil dari sidik jari
            var commandFingerprint = Database.connection.CreateCommand();
            commandFingerprint.CommandText = @"SELECT * FROM sidik_jari;";
            var readerFingerprint = commandFingerprint.ExecuteReader();
            
            //Menambahkan ke list of Fingerprint
            List<Fingerprint> listFingerprint= new List<Fingerprint>();
            while(readerFingerprint.Read()){
                Fingerprint fingerprint = ParseFingerprint(readerFingerprint);
                listFingerprint.Add(fingerprint);
            }

            //Command untuk mengambil dari biodata
            var commandBiodata = Database.connection.CreateCommand(); 
            commandBiodata.CommandText = @"SELECT * FROM biodata;";
            var readerBiodata = commandBiodata.ExecuteReader(); 
            
            //Menambahkan ke list of Biodata
            List<Biodata> listBiodata = new List<Biodata>(); 
            while(readerBiodata.Read()){
                Biodata biodata = ParseBiodata(readerBiodata); 
                listBiodata.Add(biodata);
            }

            //Mengubah list of Biodata menjadi list of String 
            List<string> listBiodataString = new List<string>(); 
            foreach(var bio in listBiodata){
                listBiodataString.Add(bio.nama);
            }

            List<Fingerprint> listFingerprintASCII = new List<Fingerprint>(); 
            foreach(var item in listFingerprint){
                Fingerprint temp = new Fingerprint(){
                    nama = item.nama,
                    berkas_citra = Converter.ImageToAsciiStraight(item.berkas_citra)
                }; 
                listFingerprintASCII.Add(temp);
            } 

            //Mengubah list of Fingerprint menjadi list of String
            List<string> listFingerprintString = new List<string>(); 
            foreach (var fingerprint in listFingerprintASCII){
                listFingerprintString.Add(fingerprint.berkas_citra);
            }

            RegularExpression regex = new RegularExpression();
            KnuthMorrisPratt kmp = new KnuthMorrisPratt();  

            string path = null;
            Biodata ans= null; 
            int[] lcs = new int[image.Length]; 
            kmp.generate_lps(image, image.Length, lcs); 
            List<(string, string, int)> result = kmp.process_all(image, listFingerprintString, lcs); //string dari fingerprint
            foreach(var fingerprint in  listFingerprintASCII){
                if(fingerprint.berkas_citra == result[0].Item1){
                    foreach(var biodata in listBiodata){
                        if(regex.IsMatch(fingerprint.nama, biodata.nama)){
                            Console.WriteLine("Nama alay: " + biodata.nama);
                            ans = biodata;
                            ans.nama = fingerprint.nama;
                            break;
                        }
                    } 
                    foreach(var fingerpath in listFingerprint){
                        if(fingerpath.nama == fingerprint.nama){
                            path = fingerpath.berkas_citra; 
                            break;
                        }
                    }
                    break;
                }
            }
            if(ans != null && path != null){
                Console.WriteLine("Path: " + path);
                Console.WriteLine("Hasil: "); 
                Console.WriteLine("Nama: " + ans.nama);
                Console.WriteLine("Alamat: " + ans.alamat);
                Console.WriteLine("pekerjaan: " + ans.pekerjaan);
                Console.WriteLine("tanggal_lahir: " + ans.tanggal_lahir);
                Console.WriteLine("tempat_lahir: " + ans.tempat_lahir);
                Console.WriteLine("kewarganegaraan: " + ans.kewarganegaraan);
                Console.WriteLine("agama: " + ans.agama);
                return (ans, path);
            }
            return (null, null);
        }

        public static (Biodata, string) CompareFingerprintBM(string image){
            //melakukan koneksi ke database
            Database.connection.Open(); 
            
            //Command untuk mengambil dari sidik jari
            var commandFingerprint = Database.connection.CreateCommand();
            commandFingerprint.CommandText = @"SELECT * FROM sidik_jari;";
            var readerFingerprint = commandFingerprint.ExecuteReader();
            
            //Menambahkan ke list of Fingerprint
            List<Fingerprint> listFingerprint= new List<Fingerprint>();
            while(readerFingerprint.Read()){
                Fingerprint fingerprint = ParseFingerprint(readerFingerprint);
                listFingerprint.Add(fingerprint);
            }

            //Command untuk mengambil dari biodata
            var commandBiodata = Database.connection.CreateCommand(); 
            commandBiodata.CommandText = @"SELECT * FROM biodata;";
            var readerBiodata = commandBiodata.ExecuteReader(); 
            
            //Menambahkan ke list of Biodata
            List<Biodata> listBiodata = new List<Biodata>(); 
            while(readerBiodata.Read()){
                Biodata biodata = ParseBiodata(readerBiodata); 
                listBiodata.Add(biodata);
            }

            //Mengubah list of Biodata menjadi list of String 
            List<string> listBiodataString = new List<string>(); 
            foreach(var bio in listBiodata){
                listBiodataString.Add(bio.nama);
            }

            List<Fingerprint> listFingerprintASCII = new List<Fingerprint>(); 
            foreach(var item in listFingerprint){
                Fingerprint temp = new Fingerprint(){
                    nama = item.nama,
                    berkas_citra = Converter.ImageToAsciiStraight(item.berkas_citra)
                }; 
                listFingerprintASCII.Add(temp);
            } 

            //Mengubah list of Fingerprint menjadi list of String
            List<string> listFingerprintString = new List<string>(); 
            foreach (var fingerprint in listFingerprintASCII){
                listFingerprintString.Add(fingerprint.berkas_citra);
            }

            RegularExpression regex = new RegularExpression();
            BoyerMoore kmp = new BoyerMoore();  


            string path = null; 
            Biodata ans= null; 
            BoyerMoore bm = new BoyerMoore();
            List<(string, string, int)> result = bm.ProcessAllBoyerMoore(image, listFingerprintString); //string dari fingerprint
            foreach(var fingerprint in  listFingerprintASCII){
                if(fingerprint.berkas_citra == result[0].Item1){
                    foreach(var biodata in listBiodata){
                        if(regex.IsMatch(fingerprint.nama, biodata.nama)){
                            Console.WriteLine("Nama alay: " + biodata.nama);
                            ans = biodata;
                            ans.nama = fingerprint.nama;
                            break;
                        }
                    } 
                    foreach(var fingerpath in listFingerprint){
                        if(fingerpath.nama == fingerprint.nama){
                            path = fingerpath.berkas_citra; 
                            break;
                        }
                    }
                    break;
                }
            }
            if(ans != null){
                Console.WriteLine("path: " + path);
                Console.WriteLine("Hasil: "); 
                Console.WriteLine("Nama: " + ans.nama);
                Console.WriteLine("Alamat: " + ans.alamat);
                Console.WriteLine("pekerjaan: " + ans.pekerjaan);
                Console.WriteLine("tanggal_lahir: " + ans.tanggal_lahir);
                Console.WriteLine("tempat_lahir: " + ans.tempat_lahir);
                Console.WriteLine("kewarganegaraan: " + ans.kewarganegaraan);
                Console.WriteLine("agama: " + ans.agama);
                return (ans, path);
            }
            return (null, null);
        }

    }
}
