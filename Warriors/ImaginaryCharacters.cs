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
            int SlowChance = 0)
            : base(name, health, attackMax, defenseMax, element, speed)
        {
            this.SlowChance = SlowChance;
        }

        public override bool SlowAttack()
        {
            Random rnd = new Random();
            int rndSlow = rnd.Next(1, 100);

            if (rndSlow <= this.SlowChance)
            {
                return true;
            }
            else return false;
        }
    }
}
