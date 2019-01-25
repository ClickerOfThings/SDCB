using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RPGshechka
{
    public class Items
    {
        private static int ids = 0;
        public string Name { get; set; }
        public string SkillUp = null;
        public int Up = 0;
        public int condition = 100;
        public int cost = 0;
        public int item_id;
        public Items(string name = "null", string skillup = null, int skillupInt = 0, bool trader = false)
        {
            Random random = new Random();
            Thread.Sleep(200);
            cost = !(trader)
                ? random.Next(10, 30) + Convert.ToInt32(Math.Round(Math.Sqrt(MainClass.player.GetSkills()[3]))) + (skillupInt * 2)
                : random.Next(10, 30) + (MainClass.player.LvL + 1) - Convert.ToInt32(Math.Round(Math.Sqrt(MainClass.player.GetSkills()[3]))) + (skillupInt * 2);
            Name = name;
            SkillUp = skillup;
            Up = skillupInt;
            item_id = ids + 1;
            ids++;
        }

        public class Consumable
        {

            public string statUp;
            public int up;
            public Consumable(string statup = null, int up = 0)
            {
                statUp = statup;
                this.up = up;
            }

        }

    }
}
