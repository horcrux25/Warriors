using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;
using static Warriors.Fight;

namespace Warriors
{
    class Fight
    {
        public struct BattleResult
        {
            public string battleOutcome;
            public bool frozenflagChar1Outcome;
            public bool frozenflagChar2Outcome;
            public bool prevFrozenflagChar1Outcome;
            public bool prevFrozenflagChar2Outcome;
            public double DOTOutcome;
            public string DOTTypeOutcome;
            public double speedOutcome;
        }

        public struct BattleInfo
        {
            public Characters char1;
            public Characters char2;
            public bool frozenflag1;
            public bool frozenflag2;
            public bool prevFrozenflag1;
            public bool prevFrozenflag2;
            public double DOTEnemyDamage;
            public string DOTEnemyType;
            public string turnBase;
        }

        public static void Battle(Characters char1, Characters char2)
        {
            Console.WriteLine("\nBattle start!\nPress ENTER to display every turn,\notherwise, press any key to end fight immediately.\n");

            List<Characters> CharacterToPlay = new List<Characters> { char1, char2 };

            CharacterToPlay = CharacterToPlay.OrderByDescending(x => x.Speed).ToList();
            double OriginalChar1Speed = char1.Speed;
            double OriginalChar2Speed = char2.Speed;
            int turnCounter = 1;
            ConsoleKeyInfo keyPress = Console.ReadKey();

            BattleInfo battleInfo = new()
            {
                char1 = char1,
                char2 = char2,
                frozenflag1 = false,
                frozenflag2 = false,
                prevFrozenflag1 = false,
                prevFrozenflag2 = false,
                DOTEnemyDamage = 0,
                DOTEnemyType = ""
            };

            (int Left, int Top) = Console.GetCursorPosition();
            if (Left != 0)
            {
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }

            while (true)
            {
                while (keyPress.Key.Equals(ConsoleKey.Enter))
                {
                    if (turnCounter != 1)
                    {
                        keyPress = Console.ReadKey();
                    }
                    break;
                }
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Turn {turnCounter}"); turnCounter++;
                Console.ResetColor();

                while (keyPress.Key.Equals(ConsoleKey.Enter))
                {
                    keyPress = Console.ReadKey();
                    break;
                }

                battleInfo.char1 = CharacterToPlay[0];
                battleInfo.char2 = CharacterToPlay[1];
                battleInfo.turnBase = "Char1";

                if (CharacterToPlay[0].Speed >= 100)
                {
                    BattleResult battleResult1 = Result(battleInfo);

                    if (battleResult1.battleOutcome == "Battle End")
                    {
                        Console.WriteLine("\nThe end");
                        break;
                    }
                    Console.Write("\n");

                    CharacterToPlay[0].Speed = CharacterToPlay[0].Speed - 100 + OriginalChar1Speed;
                    CharacterToPlay[1].Speed = CharacterToPlay[1].Speed - battleResult1.speedOutcome;

                    battleInfo.frozenflag1 = battleResult1.frozenflagChar1Outcome;
                    battleInfo.frozenflag2 = battleResult1.frozenflagChar2Outcome;
                    battleInfo.prevFrozenflag1 = battleResult1.prevFrozenflagChar1Outcome;
                    battleInfo.prevFrozenflag2 = battleResult1.prevFrozenflagChar2Outcome;
                    battleInfo.DOTEnemyDamage = battleResult1.DOTOutcome;
                    battleInfo.DOTEnemyType = battleResult1.DOTTypeOutcome;
                    battleInfo.turnBase = "Char2";
                }
                else
                {
                    Console.Write($"{CharacterToPlay[0].Name} is");
                    ElementColors.ChangeElementColor(CharacterToPlay[1].Element);
                    Console.Write($" slowed ");
                    Console.ResetColor();
                    Console.WriteLine("can't attack in current turn.\n");
                    CharacterToPlay[0].Speed += 100;
                }

                while (keyPress.Key.Equals(ConsoleKey.Enter))
                {
                    keyPress = Console.ReadKey();
                    break;
                }

                battleInfo.char1 = CharacterToPlay[1];
                battleInfo.char2 = CharacterToPlay[0];

                if (CharacterToPlay[1].Speed >= 100)
                {
                    BattleResult battleResult2 = Result(battleInfo);

                    if (battleResult2.battleOutcome == "Battle End")
                    {
                        Console.WriteLine("\nThe end");
                        break;
                    }

                    CharacterToPlay[1].Speed = CharacterToPlay[1].Speed - 100 + OriginalChar2Speed;
                    CharacterToPlay[0].Speed = CharacterToPlay[0].Speed - battleResult2.speedOutcome;

                    battleInfo.frozenflag1 = battleResult2.frozenflagChar1Outcome;
                    battleInfo.frozenflag2 = battleResult2.frozenflagChar2Outcome;
                    battleInfo.prevFrozenflag1 = battleResult2.prevFrozenflagChar1Outcome;
                    battleInfo.prevFrozenflag2 = battleResult2.prevFrozenflagChar2Outcome;
                    battleInfo.DOTEnemyDamage = battleResult2.DOTOutcome;
                    battleInfo.DOTEnemyType = battleResult2.DOTTypeOutcome;
                    Console.Write("\n");
                }
                else
                {
                    Console.Write($"{CharacterToPlay[1].Name} is");
                    ElementColors.ChangeElementColor(CharacterToPlay[0].Element);
                    Console.Write($" slowed ");
                    Console.ResetColor();
                    Console.WriteLine("can't attack in current turn.\n");
                    CharacterToPlay[1].Speed += 100;
                }
            }
        }

        public static BattleResult Result(BattleInfo battleInfo)
        {
            double char1Attack = battleInfo.char1.Attack();
            double char2Block = battleInfo.char2.Block();
            double damage;
            double dOTDamageEnemy;
            string dOTTypeEnemy;
            double speed;
            Characters.DOTResult dotDamage = new();

            damage = char1Attack - char2Block;

            if ((battleInfo.frozenflag1 && string.Equals(battleInfo.turnBase,"Char1")) ||
                (battleInfo.frozenflag2 && string.Equals(battleInfo.turnBase,"Char2")))
            {
                Console.Write($"{battleInfo.char1.Name} is ");
                ElementColors.ChangeElementColor("Ice");
                Console.Write("frozen.");
                Console.ResetColor();
                Console.Write("\n");
            }        
            else 
            {
                if (battleInfo.prevFrozenflag1 == true && 
                    battleInfo.frozenflag1 == false && 
                    string.Equals(battleInfo.turnBase,"Char1"))
                {
                    Console.Write($"{battleInfo.char1.Name} is now ");
                    ElementColors.ChangeElementColor("Ice");
                    Console.WriteLine("unfrozen.");
                    Console.ResetColor();
                    battleInfo.prevFrozenflag1 = false;
                }

                if (battleInfo.prevFrozenflag2 == true &&
                    battleInfo.frozenflag2 == false &&
                    string.Equals(battleInfo.turnBase, "Char2"))
                {
                    Console.Write($"{battleInfo.char1.Name} is now ");
                    ElementColors.ChangeElementColor("Ice");
                    Console.WriteLine("unfrozen.");
                    Console.ResetColor();
                    battleInfo.prevFrozenflag2 = false;
                }

                if (battleInfo.DOTEnemyDamage > 0)
                {
                    Console.Write($"{battleInfo.char1.Name} was hit with {(int)battleInfo.DOTEnemyDamage} damage from ");
                    ElementColors.ChangeElementColor(battleInfo.char2.Element);
                    Console.WriteLine($"{battleInfo.DOTEnemyType}");
                    Console.ResetColor();
                    battleInfo.char1.Health -= (int)battleInfo.DOTEnemyDamage;
                    Console.WriteLine($"{battleInfo.char1.Name}'s health is now {battleInfo.char1.Health}");
                }

                if (battleInfo.char1.Health > 0)
                {
                    if (damage > 0)
                    {
                        battleInfo.char2.Health -= damage;
                    }
                    else damage = 0;

                    Console.WriteLine("{0} deals {1} damage to {2}.",
                        battleInfo.char1.Name,
                        damage,
                        battleInfo.char2.Name);

                    Console.WriteLine($"{battleInfo.char2.Name}'s health is now {battleInfo.char2.Health}");
                }
            }

            if (battleInfo.char2.Health <= 0)
            {
                Console.WriteLine("\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{battleInfo.char2.Name} died. {battleInfo.char1.Name} won!");
                Console.ResetColor();
                return new BattleResult
                {
                    battleOutcome = "Battle End",
                    frozenflagChar1Outcome = battleInfo.frozenflag1,
                    frozenflagChar2Outcome = battleInfo.frozenflag2,
                    prevFrozenflagChar1Outcome = battleInfo.prevFrozenflag1,
                    prevFrozenflagChar2Outcome = battleInfo.prevFrozenflag2,
                    DOTOutcome = 0,
                    DOTTypeOutcome = "",
                    speedOutcome = 0
                };
            }
            else if (battleInfo.char1.Health <= 0)
            {
                Console.WriteLine("\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{battleInfo.char1.Name} died. {battleInfo.char2.Name} won!");
                Console.ResetColor();
                return new BattleResult
                {
                    battleOutcome = "Battle End",
                    frozenflagChar1Outcome = battleInfo.frozenflag1,
                    frozenflagChar2Outcome = battleInfo.frozenflag2,
                    prevFrozenflagChar1Outcome = battleInfo.prevFrozenflag1,
                    prevFrozenflagChar2Outcome = battleInfo.prevFrozenflag2,
                    DOTOutcome = 0,
                    DOTTypeOutcome = "",
                    speedOutcome = 0
                };
            }
            else
            {
                if ((battleInfo.frozenflag1 == false && string.Equals(battleInfo.turnBase, "Char1")) ||
                    (battleInfo.frozenflag2 == false && string.Equals(battleInfo.turnBase, "Char2")))
                {
                    if (battleInfo.char1.DOTAttack() == true)
                    {
                        dotDamage = battleInfo.char1.DamageOverTime();
                        dOTDamageEnemy = dotDamage.DotDamage;
                        dOTTypeEnemy = dotDamage.DotType;
                        Console.Write($"{battleInfo.char1.Name} applies ");
                        ElementColors.ChangeElementColor(battleInfo.char1.Element);
                        Console.Write($"{dOTTypeEnemy}");
                        Console.ResetColor();
                        Console.WriteLine($" to {battleInfo.char2.Name}");
                    }
                    else
                    {
                        dOTDamageEnemy = 0;
                        dOTTypeEnemy = "";
                    }

                    if (battleInfo.char1 is ImaginaryCharacters ||
                        battleInfo.char1 is QuantumCharacters)
                    {
                        if (battleInfo.char1.SlowAttack() == true)
                        {
                            Console.Write($"{battleInfo.char1.Name} ");

                            if (battleInfo.char1.Element == "Quantum")
                                ElementColors.ChangeElementColor("Quantum");
                            else ElementColors.ChangeElementColor("Imaginary");
                            Console.Write("slows");
                            Console.ResetColor();
                            Console.WriteLine($" {battleInfo.char2.Name}");
                            speed = 40;
                        }
                        else speed = 0;
                    }
                    else speed = 0;
                }
                else
                {
                    dOTDamageEnemy = 0;
                    dOTTypeEnemy = "";
                    speed = 0;
                }

                if (battleInfo.turnBase == "Char1")
                {
                    battleInfo.prevFrozenflag1 = battleInfo.frozenflag1;
                    battleInfo.frozenflag1 = false;
                }
                else
                {
                    battleInfo.prevFrozenflag2 = battleInfo.frozenflag2;
                    battleInfo.frozenflag2 = false;
                }

                if (battleInfo.char1 is IceCharacter)
                {
                    if (battleInfo.char1.FreezeAttack() == true)
                    {
                        Console.Write($"{battleInfo.char1.Name} ");
                        ElementColors.ChangeElementColor("Ice");
                        Console.Write("freezes");
                        Console.ResetColor();
                        Console.WriteLine($" {battleInfo.char2.Name}");
                        if (battleInfo.turnBase == "Char1")
                        {
                            battleInfo.prevFrozenflag2 = battleInfo.frozenflag2;
                            battleInfo.frozenflag2 = true;
                        }
                        else
                        {
                            battleInfo.prevFrozenflag1 = battleInfo.frozenflag1;
                            battleInfo.frozenflag1 = true;
                        }
                    }
                }

                return new BattleResult
                {
                    battleOutcome = "Battle continues",
                    frozenflagChar1Outcome = battleInfo.frozenflag1,
                    frozenflagChar2Outcome = battleInfo.frozenflag2,
                    prevFrozenflagChar1Outcome = battleInfo.prevFrozenflag1,
                    prevFrozenflagChar2Outcome = battleInfo.prevFrozenflag2,
                    DOTOutcome = dOTDamageEnemy,
                    DOTTypeOutcome = dOTTypeEnemy,
                    speedOutcome = speed
                };
            }
        }
    }
}
