using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warriors
{
    class Characters
    {
        public string Name { get; set; }
        public double Health { get; set; }
        public double AttackMax { get; set; }
        public double DefenseMax { get; set; }
        public string Element { get; set; }
        public double Speed { get; set; }

        public Characters(string name, double health, double attackMax, double defenseMax, string element, double speed)
        {
            Name = name;
            Health = health;
            AttackMax = attackMax;
            DefenseMax = defenseMax;
            Element = element;
            Speed = speed;
        }

        public double Attack()
        {
            Random rnd = new Random();

            return rnd.Next(100, (int)AttackMax);
        }

        public virtual double Block()
        {
            Random rnd = new Random();

            return rnd.Next(1, (int)DefenseMax);
        }

        public virtual bool FreezeAttack()
        {
            return false;
        }

        public virtual bool SlowAttack()
        {
            return false;
        }

        public struct DOTResult
        {
            public string DotType;
            public double DotDamage;
        }

        public DOTResult DamageOverTime()
        {
            DOTResult dOTResult = new DOTResult();
            Random rnd = new Random();

            dOTResult.DotDamage = 0.5*(rnd.Next(100, (int)AttackMax));

            switch(Element)
            {
                case "Wind":
                    dOTResult.DotType = "Wind Shear";
                    break;
                case "Physical":
                    dOTResult.DotType = "Bleed";
                    break;
                case "Lightning":
                    dOTResult.DotType = "Shock";
                    break;
                case "Fire":
                    dOTResult.DotType = "Burn";
                    break;
                default:
                    dOTResult.DotType = "Damage over time";
                    break;
            }

            return dOTResult;
        }

        public virtual bool DOTAttack()
        {
            return false;
        }
    }
}
