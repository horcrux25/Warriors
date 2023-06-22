using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
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
            List<Characters> CharacterToPlay = new List<Characters> { char1, char2 };

            CharacterToPlay = CharacterToPlay.OrderByDescending(x => x.Speed).ToList();

            BattleInfo battleInfo = new BattleInfo();

            battleInfo.char1 = char1;
            battleInfo.char2 = char2;
            battleInfo.frozenflag1 = false;
            battleInfo.frozenflag2 = false;
            battleInfo.prevFrozenflag1 = false;
            battleInfo.prevFrozenflag2 = false;
            battleInfo.DOTEnemyDamage = 0;
            battleInfo.DOTEnemyType = "";

            while (true)
            {
                battleInfo.char1 = char1;
                battleInfo.char2 = char2;
                battleInfo.turnBase = "Char1";

                BattleResult battleResult1 = Result(battleInfo);
               
                if (battleResult1.battleOutcome == "Battle End")
                {
                    Console.WriteLine("The end");
                    break;
                }
                Console.Write("\n");

                battleInfo.char1 = char2;
                battleInfo.char2 = char1;
                battleInfo.frozenflag1 = battleResult1.frozenflagChar1Outcome;
                battleInfo.frozenflag2 = battleResult1.frozenflagChar2Outcome;
                battleInfo.prevFrozenflag1 = battleResult1.prevFrozenflagChar1Outcome;
                battleInfo.prevFrozenflag2 = battleResult1.prevFrozenflagChar2Outcome;
                battleInfo.DOTEnemyDamage = battleResult1.DOTOutcome;
                battleInfo.DOTEnemyType = battleResult1.DOTTypeOutcome;
                battleInfo.turnBase = "Char2";

                BattleResult battleResult2 = Result(battleInfo);
                if (battleResult2.battleOutcome == "Battle End")
                {
                    Console.WriteLine("The end");
                    break;
                }

                battleInfo.frozenflag1 = battleResult2.frozenflagChar1Outcome;
                battleInfo.frozenflag2 = battleResult2.frozenflagChar2Outcome;
                battleInfo.prevFrozenflag1 = battleResult1.prevFrozenflagChar1Outcome;
                battleInfo.prevFrozenflag2 = battleResult1.prevFrozenflagChar2Outcome;
                battleInfo.DOTEnemyDamage = battleResult2.DOTOutcome;
                battleInfo.DOTEnemyType = battleResult2.DOTTypeOutcome;
                Console.Write("\n");
            }
        }

        public static BattleResult Result(BattleInfo battleInfo)
        {
            double char1Attack = battleInfo.char1.Attack();
            double char2Block = battleInfo.char2.Block();
            double damage;
            double dOTDamageEnemy;
            string dOTTypeEnemy;
            Characters.DOTResult dotDamage = new();

            damage = char1Attack - char2Block;

            if ((battleInfo.frozenflag1 && string.Equals(battleInfo.turnBase,"Char1")) ||
                (battleInfo.frozenflag2 && string.Equals(battleInfo.turnBase,"Char2")))
            {
                Console.WriteLine($"{battleInfo.char1.Name} is frozen.");
            }        
            else 
            {
                if (battleInfo.prevFrozenflag1 == true && 
                    battleInfo.frozenflag1 == false && 
                    string.Equals(battleInfo.turnBase,"Char1"))
                {
                    Console.WriteLine($"{battleInfo.char1.Name} is now unfrozen.");
                    battleInfo.prevFrozenflag1 = false;
                }

                if (battleInfo.prevFrozenflag2 == true &&
                    battleInfo.frozenflag2 == false &&
                    string.Equals(battleInfo.turnBase, "Char2"))
                {
                    Console.WriteLine($"{battleInfo.char2.Name} is now unfrozen.");
                    battleInfo.prevFrozenflag2 = false;
                }

                if (battleInfo.DOTEnemyDamage > 0)
                {
                    Console.WriteLine($"{battleInfo.char1.Name} was hit with {(int)battleInfo.DOTEnemyDamage} damage from {battleInfo.DOTEnemyType}");
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
                Console.WriteLine($"{battleInfo.char2.Name} died. {battleInfo.char1.Name} won!");
                return new BattleResult
                {
                    battleOutcome = "Battle End",
                    frozenflagChar1Outcome = battleInfo.frozenflag1,
                    frozenflagChar2Outcome = battleInfo.frozenflag2,
                    prevFrozenflagChar1Outcome = battleInfo.prevFrozenflag1,
                    prevFrozenflagChar2Outcome = battleInfo.prevFrozenflag2,
                    DOTOutcome = 0,
                    DOTTypeOutcome = ""
                };
            }
            else if (battleInfo.char1.Health <= 0)
            {
                Console.WriteLine($"{battleInfo.char1.Name} died. {battleInfo.char2.Name} won!");
                return new BattleResult
                {
                    battleOutcome = "Battle End",
                    frozenflagChar1Outcome = battleInfo.frozenflag1,
                    frozenflagChar2Outcome = battleInfo.frozenflag2,
                    prevFrozenflagChar1Outcome = battleInfo.prevFrozenflag1,
                    prevFrozenflagChar2Outcome = battleInfo.prevFrozenflag2,
                    DOTOutcome = 0,
                    DOTTypeOutcome = ""
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
                        Console.WriteLine($"{battleInfo.char1.Name} applies {dOTTypeEnemy} to {battleInfo.char2.Name}");
                    }
                    else
                    {
                        dOTDamageEnemy = 0;
                        dOTTypeEnemy = "";
                    }
                }
                else
                {
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


                    dOTDamageEnemy = 0;
                    dOTTypeEnemy = "";
                }

                if (battleInfo.char1 is IceCharacter)
                {
                    if (battleInfo.char1.FreezeAttack() == true)
                    {
                        Console.WriteLine($"{battleInfo.char1.Name} freezes {battleInfo.char2.Name}");
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
                    else
                    {
                        if (battleInfo.turnBase == "Char1")
                        {
                            battleInfo.prevFrozenflag2 = battleInfo.frozenflag2;
                            battleInfo.frozenflag2 = false;
                        }
                        else
                        {
                            battleInfo.prevFrozenflag1 = battleInfo.frozenflag1;
                            battleInfo.frozenflag1 = false;
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
                    DOTTypeOutcome = dOTTypeEnemy
                };
            }
        }
    }
}
