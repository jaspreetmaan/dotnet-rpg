using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Dtos.Character;

namespace dotnet_rpg.Controllers.Services.CharacterService
{


  public class CharacterService : ICharacterService
  {
     private static List<Character> chars = new List<Character>{
            new Character(),
            new Character{id=1 ,Name = "jass" }
         };
    private readonly IMapper _mapper;

    public CharacterService(IMapper mapper)
    {
      _mapper = mapper;
    }

    public async Task<ServiceResponse< List<GetCharacterDto>>> AddCharacter (AddCharacterDto newCharacter)
    {
      var serviceREsponce = new ServiceResponse<List<GetCharacterDto>>();
      chars.Add(_mapper.Map<Character>(newCharacter));
      serviceREsponce.Data = chars.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
      return serviceREsponce;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
     ServiceResponse <List< GetCharacterDto >> response = new ServiceResponse<List<GetCharacterDto>>();

      try
      {


      Character character = chars.First(c => c.id == id );
      chars.Remove(character);
      response.Data = chars.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();

      }
      catch (System.Exception ex)
      {

        response.Success = false;
        response.Message = ex.Message;
      }

      return response;


    }

    public async Task< ServiceResponse < List<GetCharacterDto>>> GetAllCharacters()
    {

     return new ServiceResponse<List<GetCharacterDto>>
     {Data=chars.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList()};
    }

    public async Task < ServiceResponse< GetCharacterDto >> GetCharacterById(int id)
    {
      var serviceResponse = new ServiceResponse<GetCharacterDto>();
      var character = chars.FirstOrDefault(c => c.id ==id);
      serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
      return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {

      ServiceResponse < GetCharacterDto > response = new ServiceResponse<GetCharacterDto>();

      try
      {


      Character character = chars.FirstOrDefault(c => c.id == updatedCharacter.id );


      _mapper.Map(updatedCharacter,character);
      // character.Name = updatedCharacter.Name;
      // character.HitPoints = updatedCharacter.HitPoints;
      // character.Streanth = updatedCharacter.Streanth;
      // character.Defense = updatedCharacter.Defense;
      // character.Intelligence = updatedCharacter.Intelligence;
      // character.Class = updatedCharacter.Class;
      response.Data = _mapper.Map<GetCharacterDto>(character);
      }
      catch (System.Exception ex)
      {

        response.Success = false;
        response.Message = ex.Message;
      }

      return response;

    }
  }
}