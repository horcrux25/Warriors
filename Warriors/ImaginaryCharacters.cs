using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warriors
{
    class ImaginaryCharacters : Characters
    {
        int SlowChance = 0;
        CanSlow SlowType = new CanSlow();

        public ImaginaryCharacters(string name = "Imaginary Character",
            double health = 0,
            double attackMax = 0,
            double defenseMax = 0,
            string element = "Imaginary",
            double speed = 0,
            int SlowChance = 0,
            string skill = "Slow")
            : base(name, health, attackMax, defenseMax, element, speed, SlowChance, skill)
        {
            this.SlowChance = SlowChance;
        }

        public override bool SlowAttack()
        {
            Random rnd = new Random();
            int rndSlow = rnd.Next(1, 100);

            if (rndSlow <= SlowChance)
            {
                return true;
            }
            else return false;
        }
    }
}
