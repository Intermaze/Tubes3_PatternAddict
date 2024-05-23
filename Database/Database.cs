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

    }
}
