using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Dtos.Fight
{
    public class SkillAttackDto
    {
        public int AttackId { get; set; }

        public int OppenentId { get; set; }

        public int SkillId { get; set; }
    }
}