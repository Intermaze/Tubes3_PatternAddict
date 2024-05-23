using System;
using Microsoft.Data.Sqlite;

namespace Database
{
    class Database
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
		berkas_citra TEXT,
		nama VARCHAR(100)
);
";

        public static void init()
        {
            using (var connection = new SqliteConnection("Data Source=data.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = Database.ddl;
                command.ExecuteNonQuery();

                var command2 = connection.CreateCommand();
                command2.CommandText =
                    @"
									INSERT INTO biodata (nama) VALUES ('indra');
									";

                int result = command2.ExecuteNonQuery();
                Console.WriteLine(result);
            }
        }
    }
}
