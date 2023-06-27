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

        static int SkillChanceChar1 = 0;
        static int SkillChanceChar2 = 0;
        static bool SkillUseChar1 = false;
        static bool SkillUseChar2 = false;
        //static bool Char1Slow = false;
        //static bool Char2Slow = false;

        public static void Battle(Characters charOne, Characters charTwo)
        {
            Console.WriteLine("\nBattle start!\nPress ENTER to display every turn,\notherwise, press any key to end fight immediately.");
            Console.WriteLine("For auto-battle, skills will be casted automatically.\n");

            List<Characters> CharacterToPlay = new List<Characters> { charOne, charTwo };

            CharacterToPlay = CharacterToPlay.OrderByDescending(x => x.Speed).ToList();
            double OriginalChar1Speed = charOne.Speed;
            double OriginalChar2Speed = charTwo.Speed;
            int turnCounter = 1;
            ConsoleKeyInfo keyPress = Console.ReadKey();
            bool TheEnd = false;
            bool Char1TurnEnd = false;
            bool Char2TurnEnd = false;
            string OrginialChar1Name = charOne.Name;
            string OrginialChar2Name = charTwo.Name;

            BattleInfo battleInfo = new()
            {
                char1 = charOne,
                char2 = charTwo,
                frozenflag1 = false,
                frozenflag2 = false,
                prevFrozenflag1 = false,
                prevFrozenflag2 = false,
                DOTEnemyDamage = 0,
                DOTEnemyType = ""
            };

            while (TheEnd == false)
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
                (int Left, int Top) = Console.GetCursorPosition();
                if (Left != 0)
                {
                    Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                }
                Console.WriteLine($"Turn {turnCounter}"); turnCounter++;
                Console.ResetColor();

                while ((CharacterToPlay[0].Speed >= 100 ||
                        CharacterToPlay[1].Speed >= 100))
                {

                    while (keyPress.Key.Equals(ConsoleKey.Enter))
                    {
                        keyPress = Console.ReadKey();
                        break;
                    }

                    CharacterToPlay = CharacterToPlay.OrderByDescending(x => x.Speed).ToList();
                    //CharacterToPlay.ForEach(x => Console.WriteLine(x.Name + " " + x.Speed));

                    if (CharacterToPlay[0].Name == charOne.Name)
                    {
                        battleInfo.char1 = charOne;
                        battleInfo.char2 = charTwo;
                        battleInfo.turnBase = "Char1";
                    }
                    else
                    {
                        battleInfo.char1 = charTwo;
                        battleInfo.char2 = charOne;
                        battleInfo.turnBase = "Char2";
                    }
                    

                    if (CharacterToPlay[0].Speed >= 100)
                    {
                        if (keyPress.Key.Equals(ConsoleKey.Enter))
                        {
                            if (SkillChanceChar1 >= 2 && battleInfo.frozenflag1 == false)
                            {
                                string SkillUser = "";

                                while (true)
                                {
                                    Console.Write($"{CharacterToPlay[0].Name} has 2 attack stacks. Use skill? Y or N: ");
                                    SkillUser = Console.ReadLine();
                                    if (SkillUser.ToLower() == "n" ||
                                        SkillUser.ToLower() == "y")
                                    {
                                        SkillChanceChar1 = SkillUser.ToLower() == "y" ? 0 : 3;
                                        SkillUseChar1 = SkillUser.ToLower() == "y" ? true : false;
                                        break;
                                    }
                                    else
                                    {
                                        ClearLastLines(1);
                                        Console.Write("Invalid. Please select again. ");
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (SkillChanceChar1 >= 2)
                            {
                                SkillChanceChar1 = 0;
                                SkillUseChar1 = true;
                            }
                        }

                        BattleResult battleResult1 = Result(battleInfo);

                        SkillChanceChar1 = SkillUseChar1 == true ? 0 : ++SkillChanceChar1;
                        SkillUseChar1 = SkillUseChar1 == true ? false : false;

                        if (battleResult1.battleOutcome == "Battle End")
                        {
                            Console.WriteLine("\nThe end");
                            CharacterToPlay[0].Speed = 0;
                            CharacterToPlay[1].Speed = 0;
                            SkillChanceChar1 = 0;
                            SkillChanceChar2 = 0;
                            TheEnd = true;
                            break;
                        }
                        Console.Write("\n");

                        battleInfo.frozenflag1 = battleResult1.frozenflagChar1Outcome;
                        battleInfo.frozenflag2 = battleResult1.frozenflagChar2Outcome;
                        battleInfo.prevFrozenflag1 = battleResult1.prevFrozenflagChar1Outcome;
                        battleInfo.prevFrozenflag2 = battleResult1.prevFrozenflagChar2Outcome;
                        battleInfo.DOTEnemyDamage = battleResult1.DOTOutcome;
                        battleInfo.DOTEnemyType = battleResult1.DOTTypeOutcome;
                        //battleInfo.turnBase = "Char2";

                        CharacterToPlay[1].Speed = CharacterToPlay[1].Speed - battleResult1.speedOutcome;
                        CharacterToPlay[0].Speed = CharacterToPlay[0].Speed - 100;

                        if (CharacterToPlay[0].Speed < 100)
                        {
                            Char1TurnEnd = true;
                        }
                    }
                    else
                    {
                        if (Char1TurnEnd == false)
                        {
                            Console.Write($"{CharacterToPlay[0].Name} is");
                            ElementColors.ChangeElementColor(CharacterToPlay[1].Element);
                            Console.Write($" slowed ");
                            Console.ResetColor();
                            Console.WriteLine("can't attack in current turn.\n");
                            Char1TurnEnd = true;
                        }
                    }

                    while (keyPress.Key.Equals(ConsoleKey.Enter))
                    {
                        keyPress = Console.ReadKey();
                        break;
                    }

                    if (CharacterToPlay[0].Name == charOne.Name)
                    {
                        battleInfo.char1 = charTwo;
                        battleInfo.char2 = charOne;
                        battleInfo.turnBase = "Char2";
                    }
                    else
                    {
                        battleInfo.char1 = charOne;
                        battleInfo.char2 = charTwo;
                        battleInfo.turnBase = "Char1";
                    }

                    if (CharacterToPlay[1].Speed >= 100)
                    {
                        if (keyPress.Key.Equals(ConsoleKey.Enter))
                        {
                            if (SkillChanceChar2 >= 2 && battleInfo.frozenflag2 == false)
                            {

                                string SkillUser = "";

                                while (true)
                                {
                                    Console.Write($"{CharacterToPlay[1].Name} has 2 attack stacks. Use skill? Y or N: ");
                                    SkillUser = Console.ReadLine();

                                    if (SkillUser.ToLower() == "n" ||
                                        SkillUser.ToLower() == "y")
                                    {
                                        SkillChanceChar2 = SkillUser.ToLower() == "y" ? 0 : 3;
                                        SkillUseChar2 = SkillUser.ToLower() == "y" ? true : false;
                                        break;
                                    }
                                    else
                                    {
                                        ClearLastLines(1);
                                        Console.Write("Invalid. Please select again. ");
                                        //Console.ReadLine();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (SkillChanceChar2 >= 2)
                            {
                                SkillChanceChar2 = 0;
                                SkillUseChar2 = true;
                            }
                        }

                        BattleResult battleResult2 = Result(battleInfo);

                        SkillChanceChar2 = SkillUseChar2 == true ? 0 : ++SkillChanceChar2;
                        SkillUseChar2 = SkillUseChar2 == true ? false : false;

                        if (battleResult2.battleOutcome == "Battle End")
                        {
                            Console.WriteLine("\nThe end");
                            CharacterToPlay[0].Speed = 0;
                            CharacterToPlay[1].Speed = 0;
                            SkillChanceChar1 = 0;
                            SkillChanceChar2 = 0;
                            TheEnd = true;
                            break;
                        }

                        battleInfo.frozenflag1 = battleResult2.frozenflagChar1Outcome;
                        battleInfo.frozenflag2 = battleResult2.frozenflagChar2Outcome;
                        battleInfo.prevFrozenflag1 = battleResult2.prevFrozenflagChar1Outcome;
                        battleInfo.prevFrozenflag2 = battleResult2.prevFrozenflagChar2Outcome;
                        battleInfo.DOTEnemyDamage = battleResult2.DOTOutcome;
                        battleInfo.DOTEnemyType = battleResult2.DOTTypeOutcome;

                        CharacterToPlay[0].Speed = CharacterToPlay[0].Speed - battleResult2.speedOutcome;
                        CharacterToPlay[1].Speed = CharacterToPlay[1].Speed - 100;
                        Console.Write("\n");

                        if (CharacterToPlay[1].Speed < 100)
                        {
                            Char2TurnEnd = true;
                        }
                    }
                    else
                    {
                        if (Char2TurnEnd == false)
                        {
                            Console.Write($"{CharacterToPlay[1].Name} is");
                            ElementColors.ChangeElementColor(CharacterToPlay[0].Element);
                            Console.Write($" slowed ");
                            Console.ResetColor();
                            Console.WriteLine("can't attack in current turn.\n");
                            Char2TurnEnd = true;
                        }
                    }
                }


                if (Char1TurnEnd && Char2TurnEnd)
                {
                    if (CharacterToPlay[0].Name == charOne.Name)
                    {
                        CharacterToPlay[1].Speed = CharacterToPlay[1].Speed + OriginalChar2Speed;
                        CharacterToPlay[0].Speed = CharacterToPlay[0].Speed + OriginalChar1Speed;
                    }
                    else
                    {
                        CharacterToPlay[0].Speed = CharacterToPlay[0].Speed + OriginalChar2Speed;
                        CharacterToPlay[1].Speed = CharacterToPlay[1].Speed + OriginalChar1Speed;
                    }
                    Char1TurnEnd = false;
                    Char2TurnEnd = false;
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
                    if ((battleInfo.char1.DOTAttack() == true) &&
                       ((battleInfo.turnBase == "Char1" && SkillUseChar1 == true) ||
                        (battleInfo.turnBase == "Char2" && SkillUseChar2 == true)))
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

                    if ((battleInfo.char1 is ImaginaryCharacters ||
                         battleInfo.char1 is QuantumCharacters) &&
                        (battleInfo.turnBase == "Char1" && SkillUseChar1 == true) ||
                        (battleInfo.turnBase == "Char2" && SkillUseChar2 == true))
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

                if ((battleInfo.char1 is IceCharacter) && 
                    (battleInfo.turnBase == "Char1" && SkillUseChar1 == true) || 
                    (battleInfo.turnBase == "Char2" && SkillUseChar2 == true))
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

                if (battleInfo.char1.Skill == "Heal")
                {
                    if ((battleInfo.turnBase == "Char1" && SkillUseChar1 == true) ||
                        (battleInfo.turnBase == "Char2" && SkillUseChar2 == true))
                    {
                        if (Heal.HealSelf(battleInfo.char1.Chance) == true)
                        {
                            int HealValue = (int)(0.05 * battleInfo.char1.Health);
                            Console.Write($"{battleInfo.char1.Name} ");
                            Console.ForegroundColor = ConsoleColor.White; Console.Write("heals "); Console.ResetColor();
                            Console.WriteLine($"{HealValue}");
                            battleInfo.char1.Health = battleInfo.char1.Health + HealValue;
                            Console.WriteLine($"{battleInfo.char1.Name}'s health is now {battleInfo.char1.Health}");
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

        public static void ClearLastLines(int LineCount)
        {
            (int LeftCursor, int TopCursor) = Console.GetCursorPosition();
            Console.SetCursorPosition(0, TopCursor - (LineCount));
            for (int i = 0; i < (LineCount); i++)
            {
                Console.Write(new string(' ', Console.BufferWidth));
            }
            Console.SetCursorPosition(0, TopCursor - (LineCount));
        }
    }
}
