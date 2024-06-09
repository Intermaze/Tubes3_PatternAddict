using Bogus;
using Tubes3; 

string[] list_of_religion = {
    "Islam", "Christianity", "Buddhism", "Hinduism", "Judaism"
};
string[] list_of_gender = {"Perempuan", "Lelaki"};
string[] list_of_job = {
    "Dokter", "Programmer", "Guru", "Tentara", "Dosen", "Pedagang", "Kontraktor", "Arsitektur",
    "Perawat", "Akuntan", "Pengacara", "Jurnalis", "Insinyur", "Desainer", "Pengusaha", "Penulis",
    "Fotografer", "Polisi", "Chef", "Ahli Gizi", "Psikolog", "Pilot", "Pramugari", "Manajer Proyek",
    "Pustakawan", "Petani", "Montir", "Penata Rambut", "Konsultan", "Seniman", "Musisi", "Aktor",
    "Peneliti", "Ekonom", "Terapis", "Arkeolog", "Astronom", "Penjaga Pantai", "Pemadam Kebakaran",
    "Biolog", "Dentist", "Electrician", "Florist", "Geologist", "Horticulturist", "IT Specialist",
    "Journalist", "Kindergarten Teacher", "Librarian", "Marine Biologist", "Nanny", "Optician",
    "Pharmacist", "Quantity Surveyor", "Radiologist", "Statistician", "Translator", "Veterinarian",
    "Web Developer", "Yoga Instructor", "Zoologist", "Banker", "Civil Engineer", "Data Scientist",
    "Event Planner", "Fashion Designer", "Game Developer", "Hotel Manager", "Interpreter", "Jeweler",
    "Logistics Manager", "Market Research Analyst", "Network Administrator", "Occupational Therapist",
    "Personal Trainer", "Quality Control Inspector", "Real Estate Agent", "Social Worker", "Tour Guide",
    "UX Designer", "Voice Actor", "Warehouse Manager", "X-ray Technician", "Youth Counselor", "Zookeeper",
    "Air Traffic Controller", "Biomedical Engineer", "Chiropractor", "Dietitian", "Environmental Scientist",
    "Financial Analyst", "Geneticist", "Hydrologist", "Industrial Designer", "Judge", "Landscape Architect",
    "Microbiologist", "Neuroscientist", "Oceanographer", "Paralegal", "Robotics Engineer", "Sociologist",
    "Tax Advisor", "Urban Planner", "Videographer", "Wind Turbine Technician", "Archivist", "Baker",
    "Ceramic Artist", "Diver", "Exhibit Designer", "Film Director", "Glassblower", "Herbalist",
    "Insurance Agent", "Jewelry Designer", "Knitter", "Lab Technician", "Makeup Artist", "Nutritionist",
    "Osteopath", "Photographer", "Quality Assurance Tester", "Research Scientist", "Sound Engineer",
    "Technical Writer", "Underwriter", "Voice Coach", "Writer", "Yoga Therapist", "3D Animator"
};
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

bool IsFirstCharacterOfWord(string str, int index) {
    if (index == 0) {
        return true;
    }
    return str[index - 1] == ' ';
}
Random random = new Random();
foreach (var filepath in Directory.GetFiles(Path.Join("..", "Data"))){
    var filename = Path.GetFileNameWithoutExtension(filepath);
    var biodata = testBiodata.Generate();

    var fingerprint_nama = biodata.nama;
    foreach(var pair in data){
        // alay_name = alay_name.Replace(pair.Key, pair.Value);
        biodata.nama = biodata.nama.Replace(pair.Key, pair.Value);
        // alay_name = alay_name.Remove(new Random().Next(0, alay_name.Length), 1);
    }

    int charToRemove = random.Next(1, 4);
    for (int j = 0; j < charToRemove; j++) {
        if (biodata.nama.Length > 0) {
            int indexToRemove;
            do {
                indexToRemove = random.Next(0, biodata.nama.Length);
            } while (biodata.nama[indexToRemove] == ' ' || IsFirstCharacterOfWord(biodata.nama, indexToRemove));

            biodata.nama = biodata.nama.Remove(indexToRemove, 1);
        }
    }

    Tubes3.Database.InsertBiodata(biodata);
    Tubes3.Database.InsertFingerprint(fingerprint_nama, filepath);

    Console.Write(i++);
    Console.Write(": ");
    Console.Write(biodata.nama);
    Console.Write(" | ");
    Console.WriteLine(fingerprint_nama);
}

var bro = testBiodata.Generate(); 


