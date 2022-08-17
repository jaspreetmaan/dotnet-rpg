using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Fight;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.FightService
{
    public class FightService:IFightService
    {
    private readonly DataContext context;
    private readonly IMapper mapper;

    public FightService(DataContext _context , IMapper _mapper )
        {
      context = _context;
      mapper = _mapper;
    }

    public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto req)
    {
        var response = new ServiceResponse<FightResultDto>
            {
                Data = new FightResultDto()
            };
        try
        {
            var characters = await context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .Where(c => req.CharacterIds.Contains(c.id)).ToListAsync();
                bool defeated = false;
                while(!defeated)
                {
                    foreach (Character attacker in characters)
                    {
                        var oppponents = characters.Where(c => c.id != attacker.id).ToList();
                        var opponet = oppponents[new Random().Next(oppponents.Count)];
                        int damage =0;
                        string attackUsed = string.Empty;
                        bool useWeapon = new Random().Next(2) ==0;
                        if(useWeapon){
                                attackUsed = attacker.Weapon.Name;
                                damage = DoWeaponAttack(attacker,opponet);
                        }
                        else
                        {
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, opponet, skill);
                        }
                        response.Data.Log
                            .Add($"{attacker.Name}) attacks {opponet.Name} using {attackUsed} With {(damage >=0 ? damage : 0 )}damage.");

                            if(opponet.HitPoints <= 0){
                                    defeated = true;
                                    attacker.Victories ++;
                                    opponet.Defeats++;
                                    response.Data.Log.Add($"{opponet.Name} has been defeated");
                                    response.Data.Log.Add($"{attacker.Name}wins with {attacker.HitPoints} HP left!");
                                    break;
                                }
                            }
                       }

                        characters.ForEach(c=> {
                            c.Fights++;
                            c.HitPoints =100;

                        });
                        await context.SaveChangesAsync();

        }
        catch (System.Exception ex)
        {

            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto req)
    {
      var response = new ServiceResponse<AttackResultDto>();
        try
      {

        var attacker = await context.Characters
            .Include(c => c.Skills)
            .FirstOrDefaultAsync(c => c.id == req.AttackId);

        var opponet = await context.Characters
            .Include(c => c.Weapon)
            .FirstOrDefaultAsync(c => c.id == req.OppenentId);

        var skill = attacker.Skills.FirstOrDefault(s => s.Id == req.SkillId);
        if (skill == null)
        {
          response.Success = false;
          response.Message = $"{attacker.Name} doesn't know that skill.";
          return response;
        }

        int damage = DoSkillAttack(attacker, opponet, skill);

        if (opponet.HitPoints <= 0)
          response.Message = $"{opponet.Name} has been defeated";

        await context.SaveChangesAsync();

        response.Data = new AttackResultDto
        {
          Attacker = attacker.Name,
          Opponent = opponet.Name,
          AttackerHp = attacker.HitPoints,
          OpponentHP = opponet.HitPoints,
          Damage = damage

        };
      }
      catch (System.Exception ex)
        {

            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }

    private static int DoSkillAttack(Character? attacker, Character? opponet, Skill? skill)
    {
      int damage = skill.Damage = (new Random().Next(attacker.Intelligence));
      damage -= new Random().Next(opponet.Defense);

      if (damage > 0)
        opponet.HitPoints -= damage;
      return damage;
    }

    public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto req)
    {
        var response = new ServiceResponse<AttackResultDto>();
        try
      {

        var attacker = await context.Characters
            .Include(c => c.Weapon)
            .FirstOrDefaultAsync(c => c.id == req.AttackerId);

        var opponet = await context.Characters
            .Include(c => c.Weapon)
            .FirstOrDefaultAsync(c => c.id == req.OpponentId);

        int damage = DoWeaponAttack(attacker, opponet);

        if (opponet.HitPoints <= 0)
          response.Message = $"{opponet.Name} has been defeated";

        await context.SaveChangesAsync();

        response.Data = new AttackResultDto
        {
          Attacker = attacker.Name,
          Opponent = opponet.Name,
          AttackerHp = attacker.HitPoints,
          OpponentHP = opponet.HitPoints,
          Damage = damage

        };
      }
      catch (System.Exception ex)
        {

            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }

    private static int DoWeaponAttack(Character? attacker, Character? opponet)
    {
      int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Streanth));
      damage -= new Random().Next(opponet.Defense);

      if (damage > 0)
        opponet.HitPoints -= damage;
      return damage;
    }

    public async Task<ServiceResponse<List<HighScoreDto>>> GetHighScore()
    {
      var character =  await context.Characters.Where(c => c.Fights > 0).OrderByDescending(c => c.Victories)
        .ThenBy(c=> c.Defeats).ToListAsync();
        var response = new ServiceResponse<List<HighScoreDto>>
        {
            Data = character.Select(c=> mapper.Map<HighScoreDto>(c)).ToList()
        };
        return response;
    }
  }
}