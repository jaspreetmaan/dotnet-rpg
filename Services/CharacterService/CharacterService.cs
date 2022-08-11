using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Controllers.Services.CharacterService
{


  public class CharacterService : ICharacterService
  {

    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public CharacterService(IMapper mapper,DataContext context){
      _mapper = mapper;
      _context = context;

    }


    public async Task<ServiceResponse< List<GetCharacterDto>>> AddCharacter (AddCharacterDto newCharacter)
    {
      var serviceREsponce = new ServiceResponse<List<GetCharacterDto>>();

      Character character =_mapper.Map<Character>(newCharacter);
      _context.Characters.Add(character);
      await _context.SaveChangesAsync();
      serviceREsponce.Data = await _context.Characters
      .Select(c => _mapper.Map<GetCharacterDto>(c))
      .ToListAsync();
      return serviceREsponce;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
     ServiceResponse <List< GetCharacterDto >> response = new ServiceResponse<List<GetCharacterDto>>();

      try
      {


      Character character =await _context.Characters.FirstAsync(c => c.id == id );
      _context.Characters.Remove(character);
      await _context.SaveChangesAsync();
      response.Data = _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();

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
      var response = new ServiceResponse<List<GetCharacterDto>>();
      var dbChars = await _context.Characters.ToListAsync();
      response.Data = dbChars.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
     return response;
    }

    public async Task < ServiceResponse< GetCharacterDto >> GetCharacterById(int id)
    {
      var serviceResponse = new ServiceResponse<GetCharacterDto>();
      var character = _context.Characters.FirstOrDefault(c => c.id ==id);
      serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
      return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {

      ServiceResponse < GetCharacterDto > response = new ServiceResponse<GetCharacterDto>();

      try
      {


      var character = await _context.Characters.FirstOrDefaultAsync(c => c.id == updatedCharacter.id );


      _mapper.Map(updatedCharacter,character);
      // character.Name = updatedCharacter.Name;
      // character.HitPoints = updatedCharacter.HitPoints;
      // character.Streanth = updatedCharacter.Streanth;
      // character.Defense = updatedCharacter.Defense;
      // character.Intelligence = updatedCharacter.Intelligence;
      // character.Class = updatedCharacter.Class;

      await _context.SaveChangesAsync();
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