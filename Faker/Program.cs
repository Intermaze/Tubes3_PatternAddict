using Bogus;
using Tubes3; 

Console.WriteLine("Hello, World!");

var testBiodata = new Faker<User>(); 
    .StringMode(true)
    .RuleFor(o => o.)

