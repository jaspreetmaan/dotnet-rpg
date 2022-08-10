using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Controllers.Services.CharacterService;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactorController :ControllerBase
    {
    private readonly ICharacterService charService;
        public CharactorController(ICharacterService charService)
        {
      this.charService = charService;

        }



            [HttpGet("GetAll")]

         public async Task< ActionResult<List<Character>>> Get(){
            return Ok(await charService.GetAllCharacters());
         }
          [HttpGet("{id}") ]
         public async Task< ActionResult<Character> >GetSingle(int id){
            return Ok(await charService.GetCharacterById(id));
         }

         [HttpPost]
         public async Task< ActionResult<List<Character>>> AddCharacter(Character newCharacter){

            return Ok(await charService.AddCharacter(newCharacter));
         }
    }
}