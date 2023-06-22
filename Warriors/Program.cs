using System.Collections.Generic;
using System.Collections;
using Warriors;
using System.Linq;

internal class Program
{
    public static void Main(string[] args)
    {
        List<Characters> Players = new();
        List<string> PlayersName = new();
        bool continueflag1 = true;
        bool continueflag2 = true;
        int player1 = 0;
        int player2 = 0;
        List<Characters> CharacterToPlay = new();


        IceCharacter March7th = new IceCharacter("March 7th", 1000, 150, 50, "Ice", 100, 40);
        DOTCharacter DanHeng = new DOTCharacter("Dan Heng", 1000, 200, 50, "Wind", 100, 40);
        DOTCharacter Hook = new DOTCharacter("Hook", 1000, 180, 60, "Fire", 120, 40);
        DOTCharacter Natasha = new DOTCharacter("Natasha", 1000, 150, 70, "Physical", 100, 40);
        DOTCharacter Serval = new DOTCharacter("Serval", 1000, 190, 55, "Lightning", 100, 40);

        Players.Add(March7th);
        Players.Add(DanHeng);
        Players.Add(Hook);
        Players.Add(Natasha);
        Players.Add(Serval);

        Players.ForEach(x => PlayersName.Add(x.Name));

        CharacterListProcess(PlayersName);

        while (continueflag1 == true)
        {
            Console.Write("Select player 1: ");
            if (continueflag1)
            {
                try
                {
                    player1 = int.Parse(Console.ReadLine());

                    if (player1 > 0 &&  player1 <= PlayersName.Count)
                    {
                        continueflag1 = false;
                        player1 -= 1;
                        Console.WriteLine($"1st player is {PlayersName[player1]}\n");
                        CharacterToPlay = Players.Where(character => character.Name == PlayersName[player1]).ToList();
                        PlayersName.RemoveAt(player1);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection. \nPress any button to select again");
                        Console.ReadLine();
                        continueflag1 = true;
                        Console.Clear();
                        CharacterListProcess(PlayersName);
                    }
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                    Console.WriteLine("Press any button to select again");
                    Console.ReadLine();
                    continueflag1 = true;
                    Console.Clear();
                    CharacterListProcess(PlayersName);
                }
            }
        }

        CharacterListProcess(PlayersName);

        while (continueflag2 == true)
        {
            Console.Write("Select player 2: ");
            if (continueflag2)
            {
                try
                {
                    player2 = int.Parse(Console.ReadLine());

                    if (player2 > 0 && player2 <= PlayersName.Count)
                    {
                        continueflag2 = false;
                        player2 -= 1;
                        Console.WriteLine($"2nd player is {PlayersName[player2]}\n");
                        CharacterToPlay.AddRange(Players.Where(character => character.Name == PlayersName[player2]));
                        PlayersName.RemoveAt(player2);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection. \nPress any button to select again");
                        Console.ReadLine();
                        continueflag2 = true;
                        ClearLastLine(PlayersName.Count);
                        CharacterListProcess(PlayersName);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                    Console.WriteLine("Press any button to select again");
                    Console.ReadLine();
                    continueflag2 = true;
                    ClearLastLine(PlayersName.Count);
                    CharacterListProcess(PlayersName);
                }
            }
        }

        Console.WriteLine("Press any button to start battle.");
        Console.ReadLine();

        Fight.Battle(CharacterToPlay[0], CharacterToPlay[1]);
    }

    public static void ClearLastLine(int CharacterCount)
    {
        Console.SetCursorPosition(0, Console.CursorTop - (CharacterCount + 5));
        for (int i = 0; i < 9; i++)
        {
            Console.Write(new string(' ', Console.BufferWidth) + "\n");
        }
        Console.SetCursorPosition(0, Console.CursorTop - (CharacterCount + 5));
    }

    public static void CharacterListProcess(List<string> PlayersName)
    {
        Console.WriteLine("Characters:");
        PlayersName.ForEach(x => Console.WriteLine($"{PlayersName.IndexOf(x) + 1} - {x}"));
    }
}