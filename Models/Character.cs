using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Models
{
    public class Character
    {
        public int id { get; set; }
        public String Name { get; set; } ="Frodo";

        public int HitPoints { get; set; } =100;
        public int Streanth { get; set; } =10;


        public int Defense { get; set; } =10;

        public int Intelligence { get; set; } =10;

        public RpgClass Class { get; set; } = RpgClass.knight;

        public User? user { get; set; }
        public Weapon Weapon {get;set;}
        public List<Skill> Skills {get;set;}


}
}