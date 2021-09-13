using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TerraFirmaConsole
{
    internal static class Program
    {
        private static bool _exit;

        private static readonly List<ITabs> TabsList = new List<ITabs>();

        private static readonly Dictionary<string, Action> TabsDictionary = new Dictionary<string, Action>
        {
            {"Alloys", CheckTabsList<Alloys>}
            //{"Crossbreeds", ()=>{}}
            //...
        };

        private static void Main()
        {
            var rand = new Random();
            
            if (true)
            {
                var table = new BoxDrawing
                    {
                        Height = 3, 
                        Width = 22
                    }
                    .DrawHeader("Bismuth Bronze")
                    .DrawBox()
                    .DrawHeader("Boi in dress");

                for (var i = 2; i <= 10; i++)
                {
                    switch (rand.Next(0, 2))
                    {
                        case 0: table.DrawRow(i); break;
                        case 1: table.DrawBox(); break;
                    }
                }
                
                
                    

                Console.Out.WriteLine(string.Join('\n', table.ValidateDraw()));
            }
            else
            {
                Greet();
                while (true)
                {
                    Menu();
                    if (_exit)
                        break;
                    Console.Clear();
                }
            }
        }

        private static void Greet()
        {
            Console.Out.WriteLineAsync(
                "TerraFirmaCraft: The New Generation\n" +
                "  TechNodeFirmaCraft modpack Cheat \n" +
                "===================================\n" +
                "      Created by: Franklyfied        ");

            Thread.Sleep(2000);
            Console.Clear();
        }

        private static void CheckTabsList<T>() where T : class, new()
        {
            var v = new T();
            if (TabsList.Any(i => i.GetType() == v.GetType()))
                v = TabsList.Find(i => i.GetType() == v.GetType()) as T;
            else
                TabsList.Add(v as ITabs);

            ((ITabs) v)?.Menu();
        }

        private static void Menu()
        {
            Console.Out.WriteLine(
                "Menu:\n" +
                "=====================");

            for (var i = 0; i <= TabsDictionary.Count; i++)
            {
                if (i == TabsDictionary.Count)
                {
                    Console.Out.WriteLine($"{i}: Exit");
                    continue;
                }

                Console.Out.WriteLine($"{i}: {TabsDictionary.ElementAt(i).Key}");
            }

            Console.Out.Write("Choose: ");

            var input = GetInput();

            if (input < 0)
                return;
            if (input == TabsDictionary.Count)
            {
                _exit = true;
                return;
            }

            ChangePage(input);
        }


        private static void ChangePage(int page)
        {
            Console.Clear();
            var change = TabsDictionary.ElementAt(page).Value;
            change();
        }


        private static int GetInput()
        {
            if (!int.TryParse(Console.In.ReadLine(), out var page))
            {
                Console.Out.WriteLine("Wrong choice");
                return -1;
            }

            if (page >= TabsDictionary.Count + 1 || page < 0)
            {
                Console.Out.WriteLine("Choice out of bounds");
                return -1;
            }

            return page;
        }
    }
}