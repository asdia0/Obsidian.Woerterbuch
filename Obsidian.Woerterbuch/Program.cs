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

        public static Dictionary<string, TermType> StringToType = new()
        {
            { "n", TermType.Noun },
            { "v", TermType.Verb },
            { "adj", TermType.Adjective },
            { "adv", TermType.Adverb },
            { "pro", TermType.Pronoun },
            { "pre", TermType.Preposition },
            { "c", TermType.Conjunction },
            { "d", TermType.Determiner },
            { "e", TermType.Exclamation },
        };

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
                string term = (inputs.Count > 1 ? string.Empty : inputs[1]);
                string typeS = (inputs.Count > 2 ? string.Empty : inputs[2]);

                if (!StringToType.ContainsKey(typeS))
                {
                    Console.WriteLine($"\"{typeS}\" is not a valid word type.");
                    continue;
                }

                TermType type = StringToType[typeS];

                switch (option)
                {
                    case "a":
                        // Entry already exists
                        if (Dictionary.Where(i => i.Name == term).Any())
                        {
                            Console.WriteLine($"\"{term}\" already exists.");
                        }
                        // Entry does not exist
                        else
                        {
                            Dictionary.Add(new(term, type));
                            // Call e
                        }
                        break;
                    case "e":

                        break;
                    case "d":
                        Dictionary.RemoveAll(i => i.Name == term);
                        break;
                    case "v":

                        break;
                    case "q":
                        // Save Dictionary
                        File.WriteAllText(Path, Utility.Serialize(Dictionary));
                        return;
                }
            }
        }
    }
}
