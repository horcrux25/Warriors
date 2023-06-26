using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warriors
{
    class Heal
    {
        public static bool HealSelf(int HealChance)
        {
            Random rnd = new Random();
            int rndHeal = rnd.Next(1, 100);

            if (rndHeal <= HealChance)
            {
                return true;
            }
            else return false;
        }
    }
}
