using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Dtos.Fight;
using dotnet_rpg.Services.FightService;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FightController : ControllerBase
    {
    private readonly IFightService fightService;

    public FightController(IFightService  _fightService)
        {
      fightService = _fightService;
    }
    [HttpPost("Weapon")]
    public async Task <ActionResult<ServiceResponse<AttackResultDto>>>WeaponAttack (WeaponAttackDto req)
    {
        return Ok(await fightService.WeaponAttack(req));
    }

    [HttpPost("Skill")]
    public async Task <ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack (SkillAttackDto req)
    {
        return Ok (await fightService.SkillAttack(req));
    }

    [HttpPost]
    public async Task <ActionResult<ServiceResponse<FightResultDto>>> Fight (FightRequestDto req)
    {
        return Ok (await fightService.Fight(req));
    }

     [HttpGet]
    public async Task <ActionResult<ServiceResponse<List<HighScoreDto>>>> GetHighScore ()
    {
        return Ok (await fightService.GetHighScore());
    }

    }
}