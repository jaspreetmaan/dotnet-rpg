using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    private readonly IHttpContextAccessor httpContextAccessor;

    public CharacterService(IMapper mapper,DataContext context ,IHttpContextAccessor _httpContextAccessor){
      _mapper = mapper;
      _context = context;
      httpContextAccessor = _httpContextAccessor;
    }
    private int GetUserId() => int.Parse(httpContextAccessor.HttpContext.User
          .FindFirstValue(ClaimTypes.NameIdentifier));


    public async Task<ServiceResponse< List<GetCharacterDto>>> AddCharacter (AddCharacterDto newCharacter)
    {
      var serviceREsponce = new ServiceResponse<List<GetCharacterDto>>();

      Character character =_mapper.Map<Character>(newCharacter);
      character.user = await _context.Users.FirstOrDefaultAsync(u => u.id == GetUserId());
      _context.Characters.Add(character);
      await _context.SaveChangesAsync();
      serviceREsponce.Data = await _context.Characters
      .Where(c => c.user.id == GetUserId())
      .Select(c => _mapper.Map<GetCharacterDto>(c))
      .ToListAsync();
      return serviceREsponce;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
     ServiceResponse <List< GetCharacterDto >> response = new ServiceResponse<List<GetCharacterDto>>();

      try
      {


      Character character =await _context.Characters
        .FirstOrDefaultAsync(c => c.id == id && c.user.id == GetUserId() );
        if(character != null){
      _context.Characters.Remove(character);
      await _context.SaveChangesAsync();
      response.Data = _context.Characters
        .Where(c =>  c.user.id == GetUserId())
        .Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        }
        else
        {
          response.Success=false;
          response.Message ="character not found";
        }
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
      var dbChars = await _context.Characters
      .Include(c => c.Weapon)
      .Include(c => c.Skills)
      .Where(c=> c.user.id == GetUserId()).ToListAsync();
      response.Data = dbChars.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
     return response;
    }

    public async Task < ServiceResponse< GetCharacterDto >> GetCharacterById(int id)
    {
      var serviceResponse = new ServiceResponse<GetCharacterDto>();
      var character = _context.Characters
      .Include(c => c.Weapon)
      .Include(c => c.Skills)
      .FirstOrDefault(c => c.id == id && c.user.id == GetUserId());
      serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
      return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
    {

      ServiceResponse < GetCharacterDto > response = new ServiceResponse<GetCharacterDto>();

      try
      {


      var character = await _context.Characters.Include(c => c.user).FirstOrDefaultAsync(c => c.id == updatedCharacter.id );

      if(character.user.id == GetUserId()){


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

      else
      {
        response.Success =false;
        response.Message = "character not found";
      }
      }
      catch (System.Exception ex)
      {

        response.Success = false;
        response.Message = ex.Message;
      }

      return response;

    }

    public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
    {
      var response = new ServiceResponse<GetCharacterDto>();
      try{

          var character = await _context.Characters
            .Include(c=> c.Weapon)
            .Include(c=> c.Skills)
            .FirstOrDefaultAsync(c => c.id == newCharacterSkill.CharacterId &&
             c.user.id == GetUserId());

          if(character == null){
            response.Success =false;
            response.Message = "Charcter Not found";
            return response;
          }
          var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
          if(skill == null ){
            response.Success = false;
            response.Message ="Skill not found";
            return response;
          }
          character.Skills.Add(skill);
          await _context.SaveChangesAsync();
          response.Data = _mapper.Map<GetCharacterDto>(character);

      }

       catch(Exception ex){
        response.Success =false;
        response.Message =ex.Message;
      }
      return response;
    }
  }
}