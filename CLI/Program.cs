using System.IO;
using Tubes3;

Database.Initialize();
Database.FixFingerprint();

// Database.CompareFingerprintKMP();

//** masukkan path **/

Console.Write("Masukkan path berkas citra: ");
string path = Console.ReadLine();

string wantToCompare = Converter.ImageToAsciiStraight(path);

Database.CompareFingerprintKMP();
Database.CompareFingerprintBM();

