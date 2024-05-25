using Tubes3;
using System.IO; 

Database.Initialize(); 

// Database.CompareFingerprintKMP();

//** masukkan path **/

Console.Write("Masukkan path berkas citra: "); 
string path = Console.ReadLine(); 

string wantToCompare = Converter.ImageToAsciiStraight(path);

Database.CompareFingerprintKMP();
Database.CompareFingerprintBM();