using System.Collections.Generic;
using System.Collections;
using Warriors;
using System.Linq;
using System;

internal class Program
{
    public static void Main(string[] args)
    {
        List<Characters> Players = new();
        //List<string> PlayersName = new();
        bool continueflag1 = true;
        bool continueflag2 = true;
        bool repeatflag = true;
        int player1 = 0;
        int player2 = 0;
        List<Characters> CharacterPlay = new();

        while (repeatflag == true)
        {
            List<Characters> PlayersCopy = new();

            ImaginaryCharacters Yukong = new ImaginaryCharacters("Yukong", 1000, 150, 50, "Imaginary", 110, 65, "Slow");
            IceCharacter March7th = new IceCharacter("March 7th", 1000, 150, 60, "Ice", 100, 50, "Freeze");
            DOTCharacter DanHeng = new DOTCharacter("Dan Heng", 1000, 200, 45, "Wind", 100, 40, "Slow");
            DOTCharacter Hook = new DOTCharacter("Hook", 1000, 175, 50, "Fire", 100, 50, "Burn");
            DOTCharacter Natasha = new DOTCharacter("Natasha", 1250, 100, 100, "Physical", 100, 40,"Heal");
            DOTCharacter Serval = new DOTCharacter("Serval", 1000, 180, 45, "Lightning", 110, 40,"Shock");
            QuantumCharacters Qingque = new QuantumCharacters("Qingque", 1000, 150, 60, "Quantum", 120, 40,"Slow");

            Players.Clear();
            PlayersCopy.Clear();
            Players.Add(Yukong);
            Players.Add(March7th);
            Players.Add(DanHeng);
            Players.Add(Hook);
            Players.Add(Natasha);
            Players.Add(Serval);
            Players.Add(Qingque);

            PlayersCopy = Players;

            continueflag1 = true;
            continueflag2 = true;
            player1 = 0;
            player2 = 0;
            CharacterPlay.Clear();
            Console.Clear();

            CharacterListsProcess(PlayersCopy);

            while (continueflag1 == true)
            {
                Console.Write("Select player 1: ");
                if (continueflag1)
                {
                    try
                    {
                        player1 = int.Parse(Console.ReadLine());

                        if (player1 > 0 && player1 <= PlayersCopy.Count)
                        {
                            continueflag1 = false;
                            player1 -= 1;
                            CharacterPlay = Players.Where(character => character.Name == PlayersCopy[player1].Name).ToList();
                            Console.WriteLine($"1st player is {CharacterPlay[0].Name}\n");
                            PlayersCopy.RemoveAt(player1);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid selection. \nPress any button to select again");
                            Console.ReadLine();
                            continueflag1 = true;
                            Console.Clear();
                            CharacterListsProcess(PlayersCopy);
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"{e.Message}");
                        Console.WriteLine("Press any button to select again");
                        Console.ReadLine();
                        continueflag1 = true;
                        Console.Clear();
                        CharacterListsProcess(PlayersCopy);
                    }
                }
            }

            CharacterListsProcess(PlayersCopy);

            while (continueflag2 == true)
            {
                Console.Write("Select player 2: ");
                if (continueflag2)
                {
                    try
                    {
                        player2 = int.Parse(Console.ReadLine());

                        if (player2 > 0 && player2 <= PlayersCopy.Count)
                        {
                            continueflag2 = false;
                            player2 -= 1;
                            CharacterPlay.AddRange(Players.Where(character => character.Name == PlayersCopy[player2].Name));
                            Console.WriteLine($"2nd player is {CharacterPlay[1].Name}\n");
                            PlayersCopy.RemoveAt(player2);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid selection. \nPress any button to select again");
                            Console.ReadLine();
                            continueflag2 = true;
                            ClearLastLine(PlayersCopy.Count);
                            CharacterListsProcess(PlayersCopy);
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"{e.Message}");
                        Console.WriteLine("Press any button to select again");
                        Console.ReadLine();
                        continueflag2 = true;
                        ClearLastLine(PlayersCopy.Count);
                        CharacterListsProcess(PlayersCopy);
                    }
                }
            }

            string UserConfirmChar = "";
            Console.Write("Confirm characters? Y to confirm, or N to select again: ");

            while (true)
            {
                UserConfirmChar = Console.ReadLine();

                if (UserConfirmChar.ToLower() == "y" ||
                    UserConfirmChar.ToLower() == "n")
                {
                    break;
                }
                else
                {
                    Console.Write("\nInvalid. Confirm characters? Y to confirm, or N to select again: ");
                }
            }

            if (UserConfirmChar.ToLower() == "n")
            {
                repeatflag = true;
                continue;
            }

            string EquipBuy = "";
            bool EquipBuyFlag = false;
            Console.Write("Do you want to purchase equipments? Y or N: ");

            while (true)
            {
                EquipBuy = Console.ReadLine();

                if (EquipBuy.ToLower() == "y" ||
                    EquipBuy.ToLower() == "n")
                {
                    EquipBuyFlag = EquipBuy.ToLower() == "y" ? true : false;
                    break;
                }
                else
                {
                    Console.Write("\nInvalid. Do you want to purchase equipments? Y or N: ");
                }
            }

            if (EquipBuyFlag)
            {
                Equipment.Equip(CharacterPlay[0], CharacterPlay[1]);

                Console.Clear();
                string ViewPlayerStat = "";
                bool StatViewFlag = false;
                Console.Write("Do you want to view characters' current stats before going to battle? Y or N: ");

                while (true)
                {
                    ViewPlayerStat = Console.ReadLine();

                    if (ViewPlayerStat.ToLower() == "y" ||
                        ViewPlayerStat.ToLower() == "n")
                    {
                        StatViewFlag = ViewPlayerStat.ToLower() == "y" ? true : false;
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.Write("\nInvalid. Do you want to view characters' current stats before going to battle? Y or N: ");
                    }
                }

                if (StatViewFlag)
                {
                    CharacterListsProcess(CharacterPlay);
                }

            }

            Fight.Battle(CharacterPlay[0], CharacterPlay[1]);

            string UserRepeat = "";
            Console.Write("\nDo you want to play again? Y or N: ");

            while (true)
            {
                UserRepeat = Console.ReadLine();

                if (UserRepeat.ToLower() == "y" ||
                    UserRepeat.ToLower() == "n")
                {
                    repeatflag = UserRepeat.ToLower() == "y" ? true : false;
                    break;
                }
                else
                {
                    Console.Write("\nInvalid. Do you want to play again? Y or N: ");
                }
            }

            if (repeatflag == false)
            {
                Console.WriteLine("Thank you for playing.");
            }
        }
    }

    public static void ClearLastLine(int CharacterCount)
    {
        Console.SetCursorPosition(0, Console.CursorTop - (CharacterCount + 7));
        for (int i = 0; i < (CharacterCount + 7); i++)
        {
            Console.Write(new string(' ', Console.BufferWidth));
        }
        Console.SetCursorPosition(0, Console.CursorTop - (CharacterCount + 7));
    }

    public static void CharacterListsProcess(List<Characters> CharacterListings)
    {
        Console.ForegroundColor = ConsoleColor.Cyan; 
        Console.WriteLine("Characters Selection:");
        Console.ResetColor(); Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("    Name       Health  Attack  Defense  Element    Speed  Skill    Skill-Chance");
        Console.ResetColor(); 

        CharacterListings.ForEach(x =>
        {
            Console.Write($"{CharacterListings.IndexOf(x) + 1} - " +
                    $"{x.Name,-10} {x.Health,-8}{x.AttackMax,-8}{x.DefenseMax,-9}");

            ElementColors.ChangeElementColor(x.Element);
            Console.Write($"{x.Element,-11}");
            Console.ResetColor();
            Console.WriteLine($"{x.Speed,-7}{x.Skill,-9}{x.Chance}");
        } );
        Console.WriteLine();
        
    }
}