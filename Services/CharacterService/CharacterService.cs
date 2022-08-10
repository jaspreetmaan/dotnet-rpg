using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Controllers.Services.CharacterService
{


  public class CharacterService : ICharacterService
  {
     private static List<Character> chars = new List<Character>{
            new Character(),
            new Character{id=1 ,Name = "jass" }
         };

    public async Task< List<Character>> AddCharacter(Character newCharacter)
    {
      return chars;
    }

    public async Task < List<Character>> GetAllCharacters()
    {
     return chars;
    }

    public async Task < Character > GetCharacterById(int id)
    {
      return chars.FirstOrDefault(d => d.id == id);
    }
  }
}