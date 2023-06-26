using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warriors
{
    class DOTCharacter : Characters
    {
        int DOTChance = 0;
        CanDOT DOTType = new CanDOT();

        public DOTCharacter(string name = "Wind Character",
            double health = 0,
            double attackMax = 0,
            double defenseMax = 0,
            string element = "",
            double speed = 0,
            int DOTChance = 0,
            string skill = "")
            : base(name, health, attackMax, defenseMax, element, speed, DOTChance, skill)
        {
            this.DOTChance = DOTChance;
        }

        public override bool DOTAttack()
        {
            Random rnd = new Random();
            int rndDOT = rnd.Next(1, 100);

            if (rndDOT <= DOTChance)
            {
                return true;
            }
            else return false;
        }

    }
}
