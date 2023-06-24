using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warriors
{
    class ElementColors
    {
        public static void ChangeElementColor(string Element)
        {
            switch (Element)
            {
                case "Imaginary":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case "Quantum":
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case "Lightning":
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case "Ice":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case "Fire":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "Physical":
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "Wind":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }
        }
    }
}
