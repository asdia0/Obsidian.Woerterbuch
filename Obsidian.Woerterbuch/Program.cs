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
        private static string Path = "dict.json";

        private static List<Term> Dictionary;

        readonly static Dictionary<string, TermType> StringToType = new()
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

        readonly static Dictionary<Tense, string> TenseToGerman = new()
        {
            { Tense.Present, "Präsens" },
            { Tense.Perfect, "Perfekt" },
            { Tense.SimplePast, "Präteritum" },
            { Tense.PastPerfect, "Plusquamperfekt" },
            { Tense.Future1, "Futur 1" },
            { Tense.Future2, "Futur 2" },
        };
        readonly static List<string> ValidOptionsMain = new()
        {
            "v",
            "a",
            "e",
            "d"
        };

        static void Main(string[] args)
        {
            // Initialize Dictionary
            if (File.Exists(Path))
            {
                Dictionary = JsonConvert.DeserializeObject<List<Term>>(File.ReadAllText(Path));
                if (Dictionary == null)
                {
                    Dictionary = new();
                }
            }
            else
            {
                File.Create(Path);
                Dictionary = new();
            }

            // Get inputs
            while (true)
            {
                Console.Clear();

                Console.WriteLine("Options:\nv - View term\na - Add term\ne - Edit term\nd - Delete term\n");

                List<string> inputs = Console.ReadLine().Split(" ").ToList();
                string option = inputs[0].ToLower();
                string term = (inputs.Count > 1 ? inputs[1] : string.Empty);
                string typeS = (inputs.Count > 2 ? inputs[2] : string.Empty);

                if (!ValidOptionsMain.Contains(option))
                {
                    Console.WriteLine($"\n\"{option}\" is not a recognised option.");
                }

                if (inputs.Count != 3)
                {
                    Console.WriteLine("\nIncomplete argument. Please input a term and its type.");
                    Proceed();
                    continue;
                }

                if (!StringToType.ContainsKey(typeS))
                {
                    Console.WriteLine($"\n\"{typeS}\" is not a valid word type.");
                    Proceed();
                    continue;
                }
                
                TermType type = StringToType[typeS];

                Console.Clear();

                switch (option)
                {
                    case "v":
                        if (TermExists(term, type))
                        {
                            TermViewer(term, type);
                        }
                        else
                        {
                            Console.WriteLine($"{FormatTerm(term, typeS)} does not exist.");
                        }
                        break;
                    case "a":
                        if (TermExists(term, type))
                        {
                            Console.WriteLine($"{FormatTerm(term, typeS)} already exists.");
                        }
                        else
                        {
                            Dictionary.Add(new(term, type));
                            TermEditor(term, type);
                        }
                        break;
                    case "e":
                        TermEditor(term, type);
                        break;
                    case "d":
                        Dictionary.RemoveAll(i => i.Name == term && i.Type == type);
                        break;
                }

                SaveDictionary();
                Proceed();
            }
        }

        public static void Proceed()
        {
            Console.WriteLine("\nEnter a character to proceed.");
            Console.ReadLine();
        }

        public static string FormatConjugations(List<Conjugation> conjugations)
        {
            string text = string.Empty;

            foreach (Conjugation conjugation in conjugations)
            {
                text += $"  * {TenseToGerman[conjugation.Tense]}\n";
                text += $"    * ich: {conjugation.Conjugations[1]}\n";
                text += $"    * du: {conjugation.Conjugations[2]}\n";
                text += $"    * er/sie/es: {conjugation.Conjugations[3]}\n";
                text += $"    * wir: {conjugation.Conjugations[4]}\n";
                text += $"    * ihr: {conjugation.Conjugations[5]}\n";
                text += $"    * sie/Sie: {conjugation.Conjugations[6]}\n";
            }

            return text;
        }

        public static string FormatLists(List<string> list)
        {
            string text = string.Empty;

            foreach (string element in list)
            {
                text += $"\n  * {element}";
            }

            return text;
        }

        public static string FormatTerm(string term, string type)
        {
            return $"\"{term} ({type}.)\"";
        }

        /// <summary>
        /// Deserialize and write the dictionary to a file.
        /// </summary>
        public static void SaveDictionary()
        {
            File.WriteAllText(Path, Utility.Serialize(Dictionary));
        }

        public static bool TermExists(string term, TermType type)
        {
            return Dictionary.Where(i => i.Name == term && i.Type == type).Any();
        }

        public static void TermViewer(string term, TermType type)
        {
            Term termV = Dictionary.Where(i => i.Name == term && i.Type == type).First();

            string text = string.Empty;
            text += $"Term: {termV.Name}\n";
            text += $"Type: {termV.Type}\n";
            text += $"Definition{(termV.Definitions.Count > 1 ? "s" : string.Empty)}:{FormatLists(termV.Definitions)}\n";
            text += $"Synonym{(termV.Synonyms.Count > 1 ? "s" : string.Empty)}:{FormatLists(termV.Synonyms)}\n";

            if (type == TermType.Noun)
            {
                text += $"Gender: {termV.Gender}\n";
                text += $"Plural: {termV.Plural}\n";
            }

            if (type == TermType.Verb)
            {
                text += $"Conjugations:\n{FormatConjugations(termV.Conjugations)}";
            }

            if (type == TermType.Verb || type == TermType.Preposition)
            {
                text += $"Is Akkusativ: {termV.IsAkkusativ}\n";
                text += $"Is Dativ: {termV.IsDativ}\n";
            }

            Console.WriteLine(text);
        }

        public static void TermEditor(string term, TermType type)
        {
            Console.Clear();

            Term termE = Dictionary.Where(i => i.Name == term && i.Type == type).First();

            // Show current state
            TermViewer(term, type);

            // Give options
            Console.WriteLine("Options:\n  * n <new_name> - Name\n  * t <new_type> - Type\n  * d <index> <new_definition> - Defintion\n  * s <index> <new-synonym> - Synonym");
            if (type == TermType.Noun)
            {
                Console.WriteLine("  * g <new_gender> - Gender\n  * p <new_plural_form> - Plural");
            }
            if (type == TermType.Verb)
            {
                Console.WriteLine("  * c <tense> <index> <new_conjugation> - Conjugation");
            }
            if (type == TermType.Verb || type == TermType.Preposition)
            {
                Console.WriteLine("  * akk <bool> - Is Akkusativ\n  * dat <bool> - Is Dativ");
            }
            Console.WriteLine("To add a new entry, set index to -1.\n");
        }
    }
}
