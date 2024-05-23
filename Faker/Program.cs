using Bogus;
using Tubes3; 

string[] list_of_religion = {"Islam", "Christian", "Buddha"};
string[] list_of_gender = {"Perempuan", "Lelaki"};
string[] list_of_job = {"Dokter", "Programmer", "Guru", "Tentara", "Dosen", "Pedagang", "Kontraktor", "Arsitektur"};
string[] list_of_marital_status = {"Menikah", "Belum Menikah"};
var testBiodata = new Faker<Biodata>()
    .StrictMode(true)
    .RuleFor(o => o.jenis_kelamin, f => f.PickRandom(list_of_gender))
    .RuleFor(o => o.nama, f => f.Name.FullName())
    .RuleFor(o => o.tanggal_lahir, f => f.Date.BetweenDateOnly(new System.DateOnly(1970, 1, 1), new System.DateOnly(2000, 12, 31)).ToString())
    .RuleFor(o => o.tempat_lahir, f => f.Address.FullAddress())
    .RuleFor(o => o.alamat, f => f.Address.FullAddress())
    .RuleFor(o => o.golongan_darah, f => f.PickRandom("A", "O", "B", "AB"))
    .RuleFor(o => o.agama, f => f.PickRandom(list_of_religion))
    .RuleFor(o => o.pekerjaan, f => f.PickRandom(list_of_job))
    .RuleFor(o => o.jenis_kelamin, f => f.PickRandom(list_of_gender))
    .RuleFor(o => o.status_perkawinan, f => f.PickRandom(list_of_marital_status))
    .RuleFor(o => o.kewarganegaraan, f => f.Address.Country());


int i = 0; 
Tubes3.Database.Initialize();

var data = new Dictionary<char, char>{
    {'a', '4'}, 
    {'e', '3'},
    {'i', '1'}, 
    {'o', '0'},
    };

foreach (var filepath in Directory.GetFiles(Path.Join("..", "Data"))){
    var filename = Path.GetFileNameWithoutExtension(filepath);
    var biodata = testBiodata.Generate();

    var alay_name = biodata.nama;
    foreach(var pair in data){
        alay_name = alay_name.Replace(pair.Key, pair.Value);
    }

    Tubes3.Database.InsertBiodata(biodata);
    Tubes3.Database.InsertFingerprint(alay_name, Converter.ImageToAscii(filepath));

    Console.Write(i++);
    Console.Write(": ");
    Console.Write(biodata.nama);
    Console.Write(" | ");
    Console.WriteLine(alay_name);
}

var bro = testBiodata.Generate(); 



Console.WriteLine(bro.jenis_kelamin);
Console.WriteLine(bro.nama);
Console.WriteLine(bro.tanggal_lahir);
Console.WriteLine(bro.tempat_lahir);
Console.WriteLine(bro.alamat);
Console.WriteLine(bro.golongan_darah);
Console.WriteLine(bro.agama);
Console.WriteLine(bro.pekerjaan);
Console.WriteLine(bro.status_perkawinan);
Console.WriteLine(bro.kewarganegaraan);

