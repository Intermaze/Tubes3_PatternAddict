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

        public static void init()
        {
            using (Database.connection = new SqliteConnection("Data Source=data.db"))
            {
                Database.connection.Open();
                var command = Database.connection.CreateCommand();
                command.CommandText = Database.ddl;
                command.ExecuteNonQuery();
            }
        }

        public static void insert_fingerprint(string name, string fingerprint)
        {
            var command = Database.connection.CreateCommand();
            command.CommandText =
                @"
						INSERT INTO sidik_jari (nama, berkas_citra)
						VALUES ($name, $fingerprint); 
						";

            command.Parameters.AddWithValue("$name:", name);
            command.Parameters.AddWithValue("$fingerprint", fingerprint);

            command.ExecuteNonQuery();
        }

        public static void load_fingerprint(string path)
        {
            int i = 0;
            foreach (var filename in Directory.GetFiles(path))
            {
                Console.Write(i++); Console.Write(": ");
                Console.Write(Converter.ImageToAscii(filename));
                Console.Write("|\n");
            }
            ;
        }
    }
}
