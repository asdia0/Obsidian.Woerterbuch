namespace Obsidian.Woerterbuch
{
    using Dictionary;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;

    public static class Program
    {
        public static string Path = "dict.json";

        public static List<Term> Dictionary;

        static void Main(string[] args)
        {
            // Initialize Dictionary
            if (File.Exists(Path))
            {
                Dictionary = JsonConvert.DeserializeObject<List<Term>>(File.ReadAllText(Path));
            }
            else
            {
                File.Create(Path);
                Dictionary = new();
            }

            // Get inputs
            while (true)
            {
                Console.WriteLine("Options:\na - Add term\ne - Edit term\nd - Delete term\nv - View term\nq - Quit");

                List<string> inputs = Console.ReadLine().Split(" ").ToList();
                string option = inputs[0].ToLower();
                string term = (inputs.Count == 0 ? string.Empty : inputs[1]);

                switch (option)
                {
                    case "a":

                        break;
                    case "e":

                        break;
                    case "d":

                        break;
                    case "v":

                        break;
                    case "q":

                        return;
                }
            }
        }
    }
}
