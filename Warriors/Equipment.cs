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
            bool repeatEquip = true;

            while (repeatEquip == true)
            {
                Console.Clear();
                List<Characters> CharacterListings = new List<Characters> { char1, char2 };
                SelectedCharEquipDisplay(CharacterListings);

                while (true)
                {
                    Console.Write("Select character to upgrade. 1 or 2:");
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
                            }
                            else
                            {
                                characterToUpgrade.Add(char2);
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

                            while (true)
                            {
                                if (EquipForChar.Money > 35)
                                {
                                    SelectedCharEquipDisplay(characterToUpgrade);
                                    EquipForChar = SelectEquip(EquipForChar);
                                    EquipForChar = BuyEquipCalculate(EquipForChar, char1);
                                    Console.Clear();
                                }
                                else
                                {
                                    SelectedCharEquipDisplay(characterToUpgrade);
                                    EquipForChar = SelectEquip(EquipForChar);
                                    EquipForChar = BuyEquipCalculate(EquipForChar, char1);
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

                                while (true)
                                {
                                    if (EquipForChar.Money > 35)
                                    {
                                        SelectedCharEquipDisplay(characterToUpgrade);
                                        EquipForChar = SelectEquip(EquipForChar);
                                        EquipForChar = BuyEquipCalculate(EquipForChar, char1);
                                        Console.Clear();
                                    }
                                    else
                                    {
                                        SelectedCharEquipDisplay(characterToUpgrade);
                                        EquipForChar = SelectEquip(EquipForChar);
                                        EquipForChar = BuyEquipCalculate(EquipForChar, char1);
                                        break;
                                    }
                                }

                            }

                            repeatEquip = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid.\nPress any button to select again");
                            Console.ReadLine();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                        Console.WriteLine("Press any button to select again");
                        Console.ReadLine();
                    }
                }
            }

        }

        public static void SelectedCharEquipDisplay(List<Characters> CharacterListings)
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
                Console.Write($"Player {CharacterListings.IndexOf(x) + 1} - " +
                    $"{x.Name,-11} {x.Health,-9}{x.AttackMax,-9}{x.DefenseMax,-9}");
                ElementColors.ChangeElementColor(x.Element);
                Console.Write($"{x.Element,-10}");
                Console.ResetColor();
                Console.WriteLine($"{x.Speed,-7}");
            });
            Console.WriteLine();
        }

        public static EquipmentGroup SelectEquip(EquipmentGroup EquipData)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Equipment Selection:           Remaining Balance: {EquipData.Money}");
            Console.ResetColor(); Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("    Equipment     Attribute       Price  Status");
            Console.ResetColor();
            Console.WriteLine($"1 - Weapon:      +30 to Attack     35    {EquipData.Weapon}");
            Console.WriteLine($"2 - Armor:       +30 to Defense    35    {EquipData.Armor}");
            Console.WriteLine($"3 - Potion:     +150 to Health     50    {EquipData.Potion}");
            Console.WriteLine($"4 - Speed Ring:  +10 to Speed      40    {EquipData.SpeedRing}");
            Console.WriteLine();

            if (EquipData.Money > 35)
            {
                while (true)
                {
                    Console.Write("Select item to buy: ");
                    try
                    {
                        int BuyItemSelect = int.Parse(Console.ReadLine());

                        if (BuyItemSelect > 0 && BuyItemSelect < 5)
                        {
                            switch (BuyItemSelect)
                            {
                                case 1:
                                    EquipData.Weapon = "Bought";
                                    EquipData.Price = 35;
                                    EquipData.AttackUp = 30;
                                    break;
                                case 2:
                                    EquipData.Armor = "Bought";
                                    EquipData.Price = 35;
                                    EquipData.DefenseUp = 30;
                                    break;
                                case 3:
                                    EquipData.Potion = "Bought";
                                    EquipData.Price = 50;
                                    EquipData.HealthUp = 100;
                                    break;
                                case 4:
                                    EquipData.SpeedRing = "Bought";
                                    EquipData.Price = 40;
                                    EquipData.SpeedUp = 10;
                                    break;
                            }
                            return EquipData;
                            //break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid.\nPress any button to select again");
                            Console.ReadLine();
                        }
                    }
                    catch (Exception a)
                    {
                        Console.WriteLine($"{a.Message}");
                        Console.WriteLine("Press any button to select again");
                        Console.ReadLine();
                    }
                }
            }
            else
            {
                Console.WriteLine("Not enough balance to buy");
                return EquipData;
            }
        }

        public static EquipmentGroup BuyEquipCalculate(EquipmentGroup EquipData, Characters charToProcess)
        {
            EquipData.Money = EquipData.Money - EquipData.Price;
            charToProcess.AttackMax += EquipData.AttackUp;
            charToProcess.DefenseMax += EquipData.DefenseUp;
            charToProcess.Health += EquipData.HealthUp;
            charToProcess.Speed += EquipData.SpeedUp;

            EquipData.AttackUp = 0;
            EquipData.DefenseUp = 0;
            EquipData.HealthUp = 0;
            EquipData.SpeedUp = 0;

            return EquipData;
        }
    }
}
