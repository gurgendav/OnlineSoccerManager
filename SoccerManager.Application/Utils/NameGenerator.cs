using System.Text;
using SoccerManager.Application.Interfaces;

namespace SoccerManager.Application.Utils;

public class NameGenerator : INameGenerator
{
    private static readonly IReadOnlyList<string> FirstNameSyllables = new[]
    {
        "mon",
        "fay",
        "shi",
        "zag",
        "blarg",
        "rash",
        "izen",
        "dee"
    };
    
    private static readonly IReadOnlyList<string> LastNameSyllables = new[]
    {
        "malo",
        "zak",
        "dee",
        "abo",
        "wonk"
    };
    
    private static readonly IReadOnlyList<string> LastNameSuffixes = new[]
    {
        "son",
        "li",
        "ssen",
        "kor",
        "yan"
    };
    
    private static readonly IReadOnlyList<string> CountryNameSyllables = new[]
    {
        "vas",
        "loc",
        "rest",
        "lad",
        "vus",
        "rus",
        "ga"
    };
    
    public string GenerateFirstName()
    {
        return CreateNewName(FirstNameSyllables, maxCount: 3);
    }

    public string GenerateLastName()
    {
        var name = CreateNewName(LastNameSyllables, maxCount: 2);

        // Add a suffix with 50% chance
        if (Random.Shared.Next(maxValue: 100) < 50)
        {
            name += LastNameSuffixes[Random.Shared.Next(LastNameSuffixes.Count)];
        }

        return name;
    }

    public string GenerateCountryName()
    {
        return CreateNewName(CountryNameSyllables, maxCount: 3);
    }

    private string CreateNewName(IReadOnlyList<string> syllables, int maxCount)
    {
        var syllableCount = Random.Shared.Next(minValue: 2, maxCount);
        var sb = new StringBuilder(syllableCount);
        
        for (var i = 0; i < syllableCount; i++)
        {
            var index = Random.Shared.Next(syllables.Count);
            var syllable = syllables[index];

            if (i == 0)
            {
                syllable = syllable[index: 0].ToString().ToUpper() + syllable[1..];
            }
            
            sb.Append(syllable);
        }
        
        return sb.ToString();
    }
}