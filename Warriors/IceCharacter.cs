using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warriors
{
    class IceCharacter : Characters
    {
        int FreezeChance = 0;
        CanFreeze freezeType = new CanFreeze();

        public IceCharacter(string name = "Ice Character",
            double health = 0,
            double attackMax = 0,
            double defenseMax = 0,
            string element = "Ice",
            double speed = 0,
            int FreezeChance = 0,
            string skill = "Freeze")
            : base(name, health, attackMax, defenseMax, element, speed, FreezeChance, skill)
        {
            this.FreezeChance = FreezeChance;
        }

        public override bool FreezeAttack()
        {
            Random rnd = new Random();
            int rndFreeze = rnd.Next(1,100);

            if (rndFreeze <= FreezeChance)
            {
                return true;
            }
            else return false;
        }
    }
}
