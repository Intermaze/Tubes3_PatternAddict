using System;
using System.IO;
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
            using (Database.connection = new SqliteConnection("Data Source=data.db"))
            {
                Database.connection.Open();
                var command = Database.connection.CreateCommand();
                command.CommandText = Database.ddl;
                command.ExecuteNonQuery();
            }
        }

        public static void InsertBiodata(
                    string name, 
                    string birthDate, 
                    string birthPlace, 
                    string gender, 
                    string bloodType, 
                    string address, 
                    string religion, 
                    string maritalStatus, 
                    string job, 
                    string nationality)
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

            command.Parameters.AddWithValue("$name", name); 
            command.Parameters.AddWithValue("$birthDate", birthDate); 
            command.Parameters.AddWithValue("$birthPlace", birthPlace); 
            command.Parameters.AddWithValue("$gender", gender); 
            command.Parameters.AddWithValue("$bloodType", bloodType); 
            command.Parameters.AddWithValue("$address", address); 
            command.Parameters.AddWithValue("$religion", religion); 
            command.Parameters.AddWithValue("$maritalStatus", maritalStatus); 
            command.Parameters.AddWithValue("$job", job); 
            command.Parameters.AddWithValue("$nationality", nationality);
            
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

        public static void LoadFingerprint(string path)
        {
            int i = 0;
            foreach (var filepath in Directory.GetFiles(path))
            {
                var filename = Path.GetFileNameWithoutExtension(filepath);

                InsertFingerprint(filename, Converter.ImageToAscii(filepath));
                Console.Write(i++);
                Console.Write(": ");
                Console.WriteLine(filename);
            }
            ;
        }
    }
}
