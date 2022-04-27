using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ConsoleApp1
{
    internal class Program
    {
        public static readonly Dictionary<int, string> _cache = new Dictionary<int, string>();

        static void Main(string[] args)
        {
            ReadInput();
        }

        private static void ReadInput()
        {
            Console.WriteLine("Options:");
            Console.WriteLine("Press 1 to list people.");
            Console.WriteLine("Press 2 to check if string or number is a palindrome.");
            Console.WriteLine("Press 3 to load Star Wars character.");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ListPeople();
                    return;
                case "2":
                    CheckIfPalindrome();
                    return;
                case "3":
                    LoadPerson();
                    return;
            }
        }

        private static void Reset()
        {
            Console.Clear();
            ReadInput();
        }

        private static void ListPeople()
        {
            Console.Clear();

            var people = new List<Person>
            {
                new Person { FirstName = "Nicholas", LastName = "Magnussen" },
                new Person { FirstName = "Birger", LastName = "Højrup" },
                new Person { FirstName = "Andreas", LastName = "Jensen" }
            };

            foreach (var person in people)
            {
                Console.WriteLine($"{person.FirstName} {person.LastName}");
                Console.WriteLine("Press any key to continue.");
            }

            Console.ReadLine();
            Reset();
        }

        private static void CheckIfPalindrome()
        {
            Console.Clear();
            Console.WriteLine("Enter input (number or string).");

            var input = Console.ReadLine();

            if (int.TryParse(input, out var number))
            {
                CheckIfPalindrome(number);
                return;
            }

            input = input.RemoveWhitespaceAndSpecialCharacters();

            var charArray = input.ToCharArray();
            Array.Reverse(charArray);
            var reverse = new string(charArray);
            Console.Clear();

            if (input.Equals(reverse, StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Input is a palindrome.");
                Console.WriteLine("Press any key to continue.");
            }
            else
            {
                Console.WriteLine("Input is not a palindrome.");
                Console.WriteLine("Press any key to continue.");
            }

            Console.ReadLine();
            Reset();
        }

        private static void CheckIfPalindrome(int input)
        {
            Console.Clear();

            var output = input;
            var reverse = 0;

            while (input > 0)
            {
                var digit = input % 10;
                reverse = reverse * 10 + digit;
                input = input / 10;
            }

            if (output == reverse)
            {
                Console.WriteLine("Input is a palindrome.");
                Console.WriteLine("Press any key to continue.");
            }
            else
            {
                Console.WriteLine("Input is not a palindrome.");
                Console.WriteLine("Press any key to continue.");
            }

            Console.ReadLine();
            Reset();
        }

        private static async void LoadPerson()
        {
            Console.Clear();
            Console.WriteLine("Enter id.");

            var input = Console.ReadLine();
            Console.Clear();

            if (int.TryParse(input, out var id))
            {
                if (_cache.ContainsKey(id))
                {
                    Console.WriteLine(_cache[id]);
                    Console.ReadLine();
                    Reset();
                    return;
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://swapi.dev/api/");

                    var result = client.GetAsync("people/1").Result;

                    if (result.IsSuccessStatusCode)
                    {
                        var content = await result.Content.ReadAsStringAsync();
                        var output = JsonConvert.DeserializeObject(content);
                        var formatted = JsonConvert.SerializeObject(output, Formatting.Indented);

                        _cache.Add(id, formatted);

                        Console.WriteLine(formatted);
                    }
                    else
                    {
                        Console.WriteLine("An error occured.");
                        Console.WriteLine("Press any key to continue.");
                    }

                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
                Console.WriteLine("Press any key to continue.");
            }

            Console.ReadLine();
            Reset();
        }
    }
}
