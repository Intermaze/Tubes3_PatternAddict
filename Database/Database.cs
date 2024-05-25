﻿using System;
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

        public static void CompareFingerprintKMP(){
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

            //Mengubah list of Fingerprint menjadi list of String
            List<string> listFingerprintString = new List<string>(); 
            foreach (var fingerprint in listFingerprint){
                listFingerprintString.Add(fingerprint.berkas_citra);
            }

            RegularExpression regex = new RegularExpression();
            KnuthMorrisPratt kmp = new KnuthMorrisPratt();  
            foreach (var fingerprint in listFingerprint){
                //ALGORITMA: menggunakan algoritma KMP
                int[] lcsTemp = new int[fingerprint.berkas_citra.Length];
                kmp.generate_lps(fingerprint.berkas_citra, fingerprint.berkas_citra.Length, lcsTemp);
                List<(string, string, int)> result = kmp.process_all(fingerprint.berkas_citra, listFingerprintString, lcsTemp);
                foreach(var sidikJari in listFingerprint){
                    if(sidikJari.berkas_citra == result[0].Item1){
                        /*BAGIAN: buat nyari nama yang sesuai dari nama alay yang ada di table sidik_jari*/

                        foreach(var biodata in listBiodata){
                            if(regex.IsMatch(sidikJari.nama, biodata.nama)){
                                Console.WriteLine("Pemilik nama Asli: " + biodata.nama);
                                break;
                            }
                        }
                        /*Didapat dari sidik jari yang ingin dicar dari fingerprint di dalam listFingerPrint*/
                        // Console.WriteLine("Pemilik nama Asli: " + resultNama[0].Item1);
                        Console.WriteLine("Pemilik nama Alay: " + sidikJari.nama);
                        Console.WriteLine("Sidik jari: " + sidikJari.berkas_citra + " \ndatabase: " + result[0].Item2);
                        Console.WriteLine();
                        break;
                    }
                }
            }
        }

        public static void CompareFingerprintBM(){
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

            //Mengubah list of Fingerprint menjadi list of String
            List<string> listFingerprintString = new List<string>(); 
            foreach (var fingerprint in listFingerprint){
                listFingerprintString.Add(fingerprint.berkas_citra);
            }

            RegularExpression regex = new RegularExpression();

            BoyerMoore kmp = new BoyerMoore();  
            foreach (var fingerprint in listFingerprint){
                //ALGORITMA: menggunakan algoritma KMP
                List<(string, string, int)> result = kmp.ProcessAllBoyerMoore(fingerprint.berkas_citra, listFingerprintString);
                foreach(var sidikJari in listFingerprint){
                    if(sidikJari.berkas_citra == result[0].Item1){
                        /*BAGIAN: buat nyari nama yang sesuai dari nama alay yang ada di table sidik_jari*/

                        foreach(var biodata in listBiodata){
                            if(regex.IsMatch(sidikJari.nama, biodata.nama)){
                                Console.WriteLine("Pemilik nama Asli: " + biodata.nama);
                                break;
                            }
                        }
                        /*Didapat dari sidik jari yang ingin dicar dari fingerprint di dalam listFingerPrint*/
                        // Console.WriteLine("Pemilik nama Asli: " + resultNama[0].Item1);
                        Console.WriteLine("Pemilik nama Alay: " + sidikJari.nama);
                        Console.WriteLine("Sidik jari: " + sidikJari.berkas_citra + " \ndatabase: " + result[0].Item2);
                        Console.WriteLine();
                        break;
                    }
                }
            }
        }

    }
}
