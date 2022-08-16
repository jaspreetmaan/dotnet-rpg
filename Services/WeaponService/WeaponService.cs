using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Dtos.Weapon;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.WeaponService
{
  public class WeaponService : IWeaponService
  {
    private readonly DataContext context;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IMapper mapper;

    public WeaponService(DataContext _Context , IHttpContextAccessor  _httpContextAccessor , IMapper _mapper )
    {
      context = _Context;
      httpContextAccessor = _httpContextAccessor;
      mapper = _mapper;
    }
    public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
    {
      ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
      try
      {
        Character character = await context.Characters
            .FirstOrDefaultAsync(c => c.id == newWeapon.CharacterId && c.user.id == int.
            Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));

       if(character == null){
        response.Success = false;
        response.Message ="Character not found";
        return response;
       }
       Weapon weapon = new Weapon{
        Name = newWeapon.Name,
        Damage = newWeapon.Damage,
        Character = character
       };
       context.Weapons.Add(weapon);
       await context.SaveChangesAsync();
       response.Data = mapper.Map<GetCharacterDto>(character);
      }
      catch (System.Exception ex)
      {

        response.Success =false;
        response.Message = ex.Message;
      }
      return response;
    }
  }
}