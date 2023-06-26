using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Warriors
{
    class Equipment
    {
        public struct EquipmentGroup
        {
            public double Money;
            public double Price;
            public string Weapon;
            public double AttackUp;
            public string Armor;
            public double DefenseUp;
            public string Potion;
            public double HealthUp;
            public string SpeedRing;
            public double SpeedUp;
        }

        public static void Equip(Characters char1, Characters char2)
        {
            //this is my comment
            bool repeatEquip = true;
            int characterCount = 0;

            while (repeatEquip == true)
            {
                Console.Clear();
                List<Characters> CharacterListings = new List<Characters> { char1, char2 };
                SelectedCharEquipDisplay(CharacterListings, characterCount);

                while (true)
                {
                    Console.Write("Select character to upgrade. 1 or 2: ");
                    try
                    {
                        int UpgradeChar = int.Parse(Console.ReadLine());

                        if (UpgradeChar > 0 && UpgradeChar < 3)
                        {
                            List<Characters> characterToUpgrade = new List<Characters>();
                            Console.Clear();

                            if (UpgradeChar == 1)
                            {
                                characterToUpgrade.Add(char1);
                                characterCount = 1;
                            }
                            else
                            {
                                characterToUpgrade.Add(char2);
                                characterCount = 2;
                            }

                            EquipmentGroup EquipForChar = new EquipmentGroup()
                            {
                                Money = 100,
                                Price = 0,
                                Weapon = "",
                                AttackUp = 0,
                                Armor = "",
                                DefenseUp = 0,
                                Potion = "",
                                HealthUp = 0,
                                SpeedRing = "",
                                SpeedUp = 0
                            };

                            List<double> TotalBuy = new() { 35, 35, 50, 40 };

                            while (true)
                            {
                                if ((TotalBuy.Any(x => EquipForChar.Money >= x)) == true)
                                {
                                    SelectedCharEquipDisplay(characterToUpgrade, characterCount);
                                    EquipForChar = SelectEquip(EquipForChar, TotalBuy);
                                    EquipForChar = BuyEquipCalculate(EquipForChar, characterToUpgrade[0], TotalBuy);
                                    Console.Clear();
                                }
                                else
                                {
                                    SelectedCharEquipDisplay(characterToUpgrade, characterCount);
                                    EquipForChar = SelectEquip(EquipForChar, TotalBuy);
                                    EquipForChar = BuyEquipCalculate(EquipForChar, characterToUpgrade[0], TotalBuy);
                                    break;
                                }
                            }

                            Console.Write("Do you want to upgrade the other character? Y to upgrade: ");
                            string UpgradeNext = Console.ReadLine();
                            if (UpgradeNext.ToLower() == "y")
                            {
                                characterToUpgrade.Clear();
                                Console.Clear();
                                if (UpgradeChar == 1)
                                {
                                    characterToUpgrade.Add(char2);
                                }
                                else characterToUpgrade.Add(char1);

                                EquipForChar = new EquipmentGroup()
                                {
                                    Money = 100,
                                    Price = 0,
                                    Weapon = "",
                                    AttackUp = 0,
                                    Armor = "",
                                    DefenseUp = 0,
                                    Potion = "",
                                    HealthUp = 0,
                                    SpeedRing = "",
                                    SpeedUp = 0
                                };

                                TotalBuy = new() { 35, 35, 50, 40 };

                                switch (characterCount)
                                {
                                    case 1: characterCount = 2; break;
                                    case 2: characterCount = 1; break;
                                    default: characterCount = 0; break;
                                }

                                while (true)
                                {
                                    if ((TotalBuy.Any(x => EquipForChar.Money >= x)) == true)
                                    //if (EquipForChar.Money > 35)
                                    {
                                        SelectedCharEquipDisplay(characterToUpgrade, characterCount);

                                        EquipForChar = SelectEquip(EquipForChar, TotalBuy);
                                        EquipForChar = BuyEquipCalculate(EquipForChar, characterToUpgrade[0], TotalBuy);
                                        Console.Clear();
                                    }
                                    else
                                    {
                                        SelectedCharEquipDisplay(characterToUpgrade, characterCount);
                                        EquipForChar = SelectEquip(EquipForChar, TotalBuy);
                                        EquipForChar = BuyEquipCalculate(EquipForChar, characterToUpgrade[0], TotalBuy);
                                        break;
                                    }
                                }
                                Console.ReadLine();

                            }

                            repeatEquip = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid.\nPress any button to select again");
                            Console.ReadLine();
                            ClearLastLines(4);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                        Console.WriteLine("Press any button to select again");
                        Console.ReadLine();
                        ClearLastLines(4);
                    }
                }
            }

        }

        public static void SelectedCharEquipDisplay(List<Characters> CharacterListings, int Count)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Selected Characters: ");
            Console.ResetColor(); Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("           Name        Health   Attack   Defense  Element   Speed ");
            Console.ResetColor();
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, Console.CursorTop);
            CharacterListings.ForEach(x =>
            {
                if (Count == 0)
                {
                    Console.Write($"Player {CharacterListings.IndexOf(x) + 1} - ");
                }
                else
                {
                    Console.Write($"Player {Count} - ");
                }
                Console.Write($"{x.Name,-11} {x.Health,-9}{x.AttackMax,-9}{x.DefenseMax,-9}");
                ElementColors.ChangeElementColor(x.Element);
                Console.Write($"{x.Element,-10}");
                Console.ResetColor();
                Console.WriteLine($"{x.Speed,-7}");
            });

            if (Count == 0)
            {
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("------------------------------------------------------------------");
            }
                
        }

        public static EquipmentGroup SelectEquip(EquipmentGroup EquipData, List<double> TotalBuy)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Equipment Selection:           Remaining Balance: {EquipData.Money}");
            Console.ResetColor(); Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("    Equipment      Attribute       Price   Status");
            Console.ResetColor();
            Console.Write("1 - Potion:      "); Console.ForegroundColor = ConsoleColor.Green; Console.Write("+150");
            Console.ResetColor(); Console.WriteLine($" to Health     50     {EquipData.Potion}");
            Console.Write("2 - Weapon:       "); Console.ForegroundColor = ConsoleColor.Green; Console.Write("+30");
            Console.ResetColor(); Console.WriteLine($" to Attack     35     {EquipData.Weapon}");
            Console.Write("3 - Armor:        "); Console.ForegroundColor = ConsoleColor.Green; Console.Write("+30");
            Console.ResetColor(); Console.WriteLine($" to Defense    35     {EquipData.Armor}");
            Console.Write("4 - Speed Ring:   "); Console.ForegroundColor = ConsoleColor.Green; Console.Write("+10");
            Console.ResetColor(); Console.WriteLine($" to Speed      40     {EquipData.SpeedRing}");

            Console.WriteLine();

            //if (EquipData.Money > 35)
            if ((TotalBuy.Any(x => EquipData.Money >= x)) == true)
            {
                while (true)
                {
                    Console.Write("Select item to buy: ");
                    try
                    {
                        int BuyItemSelect = int.Parse(Console.ReadLine());

                        if (BuyItemSelect > 0 && BuyItemSelect < 5)
                        {
                            if ((BuyItemSelect == 1 && String.Equals(EquipData.Potion, "Bought")) ||
                                (BuyItemSelect == 2 && String.Equals(EquipData.Weapon,"Bought")) ||
                                (BuyItemSelect == 3 && String.Equals(EquipData.Armor,"Bought")) ||
                                (BuyItemSelect == 4 && String.Equals(EquipData.SpeedRing,"Bought")))
                            {
                                Console.WriteLine("Already bought.\nPress any button to select again");
                                Console.ReadLine();
                                ClearLastLines(4);
                            }
                            else
                            {
                                switch (BuyItemSelect)
                                {
                                    case 1:
                                        EquipData.Potion = "Bought";
                                        EquipData.Price = 50;
                                        EquipData.HealthUp = 150;
                                        break;
                                    case 2:
                                        EquipData.Weapon = "Bought";
                                        EquipData.Price = 35;
                                        EquipData.AttackUp = 30;
                                        break;
                                    case 3:
                                        EquipData.Armor = "Bought";
                                        EquipData.Price = 35;
                                        EquipData.DefenseUp = 30;
                                        break;
                                    case 4:
                                        EquipData.SpeedRing = "Bought";
                                        EquipData.Price = 40;
                                        EquipData.SpeedUp = 10;
                                        break;
                                }
                                return EquipData;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid.\nPress any button to select again");
                            Console.ReadLine();
                            ClearLastLines(4);
                        }
                    }
                    catch (Exception a)
                    {
                        Console.WriteLine($"{a.Message}");
                        Console.WriteLine("Press any button to select again");
                        Console.ReadLine();
                        ClearLastLines(4);
                    }
                }
            }
            else
            {
                Console.WriteLine("\nNot enough balance to buy more.");
                return EquipData;
            }
        }

        public static EquipmentGroup BuyEquipCalculate(EquipmentGroup EquipData, Characters charToProcess, List<double> PriceData)
        {
            EquipData.Money = EquipData.Money - EquipData.Price;
            charToProcess.AttackMax += EquipData.AttackUp;
            charToProcess.DefenseMax += EquipData.DefenseUp;
            charToProcess.Health += EquipData.HealthUp;
            charToProcess.Speed += EquipData.SpeedUp;
            PriceData.Remove(EquipData.Price);

            EquipData.AttackUp = 0;
            EquipData.DefenseUp = 0;
            EquipData.HealthUp = 0;
            EquipData.SpeedUp = 0;

            return EquipData;
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
