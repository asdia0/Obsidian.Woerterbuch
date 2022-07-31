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

                if (inputs.Count != 3)
                {
                    Console.WriteLine("\nIncomplete argument. Please input a term and its type.");
                    Proceed();
                    continue;
                }

                TermType? type = null;
                if (!StringToType.ContainsKey(typeS))
                {
                    Console.WriteLine($"\"{typeS}\" is not a valid word type.");
                    Proceed();
                    continue;
                }
                
                type = StringToType[typeS];

                Console.Clear();

                switch (option)
                {
                    case "v":
                        // Term exists
                        if (Dictionary.Where(i => i.Name == term && i.Type == type).Any())
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
                        // Term does not exist
                        else
                        {
                            Console.WriteLine($"\"{term} ({typeS}.)\" does not exist.");
                        }
                        break;
                    case "a":
                        // Term already exists
                        if (Dictionary.Where(i => i.Name == term && i.Type == type).Any())
                        {
                            Console.WriteLine($"\"{term}\" already exists.");
                        }
                        // Term does not exist
                        else
                        {
                            Dictionary.Add(new(term, (TermType)type));
                            // Call e
                        }
                        break;
                    case "e":

                        break;
                    case "d":
                        Dictionary.RemoveAll(i => i.Name == term && i.Type == type);
                        break;
                    default:
                        Console.WriteLine($"\"{option}\" is not a recognised option.");
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

        /// <summary>
        /// Deserialize and write the dictionary to a file.
        /// </summary>
        public static void SaveDictionary()
        {
            File.WriteAllText(Path, Utility.Serialize(Dictionary));
        }
    }
}
