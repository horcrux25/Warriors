using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warriors
{
    class QuantumCharacters : ImaginaryCharacters
    {
        public QuantumCharacters(string name = "Quantum Character",
            double health = 0,
            double attackMax = 0,
            double defenseMax = 0,
            string element = "Quantum",
            double speed = 0,
            int slowChance = 0)
            : base(name, health, attackMax, defenseMax, element, speed, slowChance)
        {

        }
    }
}
