using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using dotnet_rpg.Controllers.Services.CharacterService;
using dotnet_rpg.Dtos.Character;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharactorController :ControllerBase
    {
    private readonly ICharacterService charServ;
        public CharactorController(ICharacterService charService)
        {
            charServ = charService;

        }

         [HttpGet("GetAll")]

         public async Task< ActionResult <ServiceResponse <List<GetCharacterDto>>>> Get(){
            int id= int.Parse(User.Claims.FirstOrDefault(c=> c.Type == ClaimTypes.NameIdentifier).Value);
            return Ok(await charServ.GetAllCharacters(id));
         }
          [HttpGet("{id}") ]
         public async Task< ActionResult <ServiceResponse <GetCharacterDto>>>GetSingle(int id){
            return Ok(await charServ.GetCharacterById(id));
         }
            [HttpDelete("{id}") ]
         public async Task< ActionResult <ServiceResponse<List< GetCharacterDto>>>>Delete(int id){
            var response = await charServ.DeleteCharacter(id);
            if(response.Data == null)
            return NotFound(response);
            else
            return Ok(response);
         }
         [HttpPost]
         public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter){

            return Ok(await charServ.AddCharacter(newCharacter));
         }

         [HttpPut]
         public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> UpdateCharacter(UpdateCharacterDto updateCharacter){
            var response = await charServ.UpdateCharacter(updateCharacter);
            if(response.Data == null)
            return NotFound(response);
            else
            return Ok(response);
         }
    }
}