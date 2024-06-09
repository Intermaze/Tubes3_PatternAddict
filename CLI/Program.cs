using System.IO;
using Tubes3;

Database.Initialize();
while (true)
{
    Biodata ans;
    String pathAns;
    Console.Write("Masukkan path berkas citra: ");
    string basepath = "..\\Data";
    string path = Console.ReadLine();

    string wantToCompare = Converter.ImageToAsciiStraight(Path.Join(basepath, path));

    Console.WriteLine("Pilih Algoritma: ");
    Console.WriteLine("1. BM ");
    Console.WriteLine("2. KMP ");
    Console.Write("> ");
    string choose = Console.ReadLine();
    if (choose == "KMP")
    {
        (ans, pathAns) = Database.CompareFingerprintKMP(wantToCompare);
    }
    else if (choose == "BM")
    {
        (ans, pathAns) = Database.CompareFingerprintBM(wantToCompare);
    }
    else if (choose == "exit")
    {
        break;
    }
    Console.WriteLine("\n\n");
}
