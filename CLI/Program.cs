using Tubes3;
using System.IO; 

Database.Initialize(); 
while(true){
    Console.Write("Masukkan path berkas citra: "); 
    string basepath = Path.Join("..", "Data");
    string path =  Console.ReadLine(); 

    string wantToCompare = Converter.ImageToAsciiStraight(Path.Join(basepath, path));

    Console.WriteLine(wantToCompare.Length);
    Console.WriteLine("Pilih Algoritma: ");
    Console.WriteLine("1. BM ");
    Console.WriteLine("2. KMP ");
    Console.Write("> ");
    string choose = Console.ReadLine();
    if(choose == "KMP"){
        Database.CompareFingerprintKMP(wantToCompare);
    }
    else if(choose == "BM"){
        Database.CompareFingerprintBM(wantToCompare);
    }
    else if(choose == "exit"){
        break;
    }
    Console.WriteLine("\n\n");
}