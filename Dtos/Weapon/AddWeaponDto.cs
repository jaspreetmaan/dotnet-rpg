using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Dtos.Weapon
{
    public class AddWeaponDto
    {
        public String  Name { get; set; }

        public int Damage {get;set;}
        public int CharacterId { get; set; }
    }
}