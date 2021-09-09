using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TerraFirmaConsole
{
    public class Alloys : ITabs
    {
        private readonly Dictionary<string, Action> _alloysDictionary;
        private static short _isCorrect;

        public Alloys()
        {
            _isCorrect = -1;
            _alloysDictionary = new Dictionary<string, Action>()
            {
                {"Bismuth Bronze", BismuthBronze},
                {"Black Bronze", BlackBronze},
                {"Bronze", Bronze},
                {"Brass", Brass},
                {"Rose Gold", RoseGold},
                {"Sterling Silver", SterlingSilver},
                {"Weak Steel", WeakSteel},
                {"Weak Blue Steel", WeakBlueSteel},
                {"Weak Red Steel", WeakRedSteel}
            };
        }

        public void Menu()
        {
            while (true)
            {
                Console.Out.WriteLine(
                    "Alloys:\n" + 
                    "=====================");
                for (var i = 0; i <= _alloysDictionary.Count; i++)
                {
                    if (i == _alloysDictionary.Count)
                    {
                        Console.Out.WriteLine($"{i}: Back");
                        continue;
                    }
                    Console.Out.WriteLine($"{i}: {_alloysDictionary.ElementAt(i).Key}");
                    
                }
                Console.Out.Write("Choose: ");

                var input = GetInput();
                
                if (input == _alloysDictionary.Count)
                    break;
                
                var choice = _alloysDictionary.ElementAt(input).Value;
                choice();
            }
        }

        private static void Base(string name, List<Tuple<string, int>> alloyComp, IReadOnlyList<Tuple<int, int>> limit)
        {
            var table = new Table()
                .AddHeader(name)
                .SetColumn(4)
                .AddRow("Metal", "Min %", "Max %", "Amount");

            for (var i = 0; i < alloyComp.Count; i++)
            {
                table.AddRow(
                    CultureInfo.CurrentCulture.TextInfo.ToTitleCase(alloyComp[i].Item1),
                    limit[i].Item1.ToString(),
                    limit[i].Item2.ToString(),
                    alloyComp[i].Item2.ToString()
                    );
            }

            table
                .AddRow("Total", "0", "Remaining", "0")
                .Validate();

            while (true)
            {
                Console.Clear();
                Console.Out.WriteLine(table.Build());
                if (_isCorrect >= 0)
                {
                    Console.Out.WriteLine(_isCorrect == 1 ? "Can create alloy!" : "Proportions are wrong!");
                    _isCorrect = -1;
                }
                Console.Out.Write("Command:");
                
                string input;
                if ((input = Console.In.ReadLine()) is null )
                {
                    Console.Out.WriteLine("Wrong Input");
                    continue;
                }

                var command = input.Split(' ');
                if (command[0].Length <= 2)
                {
                    foreach (var tuple in alloyComp.Where(tuple => tuple.Item1.StartsWith(command[0])))
                    {
                        command[0] = tuple.Item1;
                        break;
                    }
                }

                if (command[0] == "back")
                    break;

                if (command[0] == "validate")
                {
                    var list = alloyComp.Select(tuple => (float)tuple.Item2).ToList();
                    Validate(list, limit);
                    continue;
                }

                var index = alloyComp.FindIndex(tuple => tuple.Item1 == command[0]);
                alloyComp[index] = new Tuple<string, int>(command[0], int.Parse(command[1]));
                table
                    .ChangeCell(3, index+1, alloyComp[index].Item2.ToString())
                    .ChangeCell(1, 4, alloyComp.Select(tuple => tuple.Item2).Sum().ToString())
                    .ChangeCell(3,4, (alloyComp.Select(tuple => tuple.Item2).Sum() % 100).ToString())
                    .Validate();
            }
            
        }

        private static void Validate(IReadOnlyCollection<float> component, IReadOnlyList<Tuple<int, int>> limit)
        {
            if (component.Any(tuple => tuple == 0f))
            {
                Console.Out.WriteLine("You did not add one of the items");
                return;
            }

            var total = component.Sum();
            var percentage = component.Select(value => value / total).ToList();
            var isInRange = true;

            for (var i = 0; i < component.Count; i++)
                isInRange = percentage[i] >= limit[i].Item1 / 100f &&
                            percentage[i] <= limit[i].Item2 / 100f;

            var remainder = total % 100;
            total -= remainder;
            _isCorrect = isInRange ? (short) 1 : (short) 0;
        }
        
        private int GetInput()
        {
            if (int.TryParse(Console.In.ReadLine(), out var page))
                if (page < _alloysDictionary.Count + 1 && page >= 0) return page;
            Console.Out.WriteLine("Error");
            return -1;
        }

        #region Metals

        private static void BismuthBronze()
        {
            var alloyComp = new List<Tuple<string, int>>
            {
                new Tuple<string, int>("bismuth", 0),
                new Tuple<string, int>("copper", 0),
                new Tuple<string, int>("zinc", 0)
            };

            var limit = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(10, 20),
                new Tuple<int, int>(50, 65),
                new Tuple<int, int>(20, 30)
            };

            Base("Bismuth Bronze", alloyComp, limit);
        }
        
        private static void Bronze()
        {
            var alloyComp = new List<Tuple<string, int>>
            {
                new Tuple<string, int>("copper", 0),
                new Tuple<string, int>("tin", 0)
            };

            var limit = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(88, 92),
                new Tuple<int, int>(8, 12)
            };

            Base("Bronze", alloyComp, limit);
        }
        
        private static void BlackBronze()
        {
            var alloyComp = new List<Tuple<string, int>>
            {
                new Tuple<string, int>("copper", 0),
                new Tuple<string, int>("silver", 0),
                new Tuple<string, int>("gold", 0)
            };

            var limit = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(50, 70),
                new Tuple<int, int>(10, 25),
                new Tuple<int, int>(10,25)
            };

            Base("Black Bronze", alloyComp, limit);
        }
        
        private static void Brass()
        {
            var alloyComp = new List<Tuple<string, int>>
            {
                new Tuple<string, int>("copper", 0),
                new Tuple<string, int>("zinc", 0)
            };

            var limit = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(88, 92),
                new Tuple<int, int>(8, 12)
            };

            Base("Brass", alloyComp, limit);
        }
        
        private static void RoseGold()
        {
            var alloyComp = new List<Tuple<string, int>>
            {
                new Tuple<string, int>("copper", 0),
                new Tuple<string, int>("gold", 0)
            };

            var limit = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(15, 30),
                new Tuple<int, int>(70, 85)
            };

            Base("Rose Gold", alloyComp, limit);
        }
        
        private static void SterlingSilver()
        {
            var alloyComp = new List<Tuple<string, int>>
            {
                new Tuple<string, int>("copper", 0),
                new Tuple<string, int>("silver", 0)
            };

            var limit = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(20, 40),
                new Tuple<int, int>(60, 80)
            };

            Base("Sterling Silver", alloyComp, limit);
        }
        
        private static void WeakSteel()
        {
            var alloyComp = new List<Tuple<string, int>>
            {
                new Tuple<string, int>("steel", 0),
                new Tuple<string, int>("nickel", 0),
                new Tuple<string, int>("black bronze", 0)
            };

            var limit = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(50, 70),
                new Tuple<int, int>(15, 25),
                new Tuple<int, int>(15,25)
            };

            Base("Weak Steel", alloyComp, limit);
        }
        
        private static void WeakBlueSteel()
        {
            var alloyComp = new List<Tuple<string, int>>
            {
                new Tuple<string, int>("black steel", 0),
                new Tuple<string, int>("steel", 0),
                new Tuple<string, int>("bismuth bronze", 0),
                new Tuple<string, int>("sterling silver", 0)
            };

            var limit = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(50, 55),
                new Tuple<int, int>(20, 25),
                new Tuple<int, int>(10, 15),
                new Tuple<int, int>(10, 15)
            };

            Base("Weak Blue Steel", alloyComp, limit);
        }
        
        private static void WeakRedSteel()
        {
            var alloyComp = new List<Tuple<string, int>>
            {
                new Tuple<string, int>("black steel", 0),
                new Tuple<string, int>("steel", 0),
                new Tuple<string, int>("brass", 0),
                new Tuple<string, int>("rose gold", 0)
            };

            var limit = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(50, 55),
                new Tuple<int, int>(20, 25),
                new Tuple<int, int>(10, 15),
                new Tuple<int, int>(10, 15)
            };

            Base("Weak Red Steel", alloyComp, limit);
        }

        #endregion
    }
}