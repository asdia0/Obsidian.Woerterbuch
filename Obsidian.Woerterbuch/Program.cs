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

        public static Dictionary<Tense, string> TenseToGerman = new()
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
            }
            else
            {
                File.Create(Path);
                Dictionary = new();
            }

            // Get inputs
            while (true)
            {
                Console.WriteLine("Options:\nv - View term\na - Add term\ne - Edit term\nd - Delete term\ns - Save dictionary\nq - Quit");

                List<string> inputs = Console.ReadLine().Split(" ").ToList();
                string option = inputs[0].ToLower();
                string term = (inputs.Count > 1 ? inputs[1] : string.Empty);
                string typeS = (inputs.Count > 2 ? inputs[2] : string.Empty);

                if (!StringToType.ContainsKey(typeS))
                {
                    Console.WriteLine($"\"{typeS}\" is not a valid word type.");
                    continue;
                }

                TermType type = StringToType[typeS];

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
                            text += $"Definition{(termV.Definitions.Count > 1 ? "s" : string.Empty)}:{string.Join("\n  * ", termV.Definitions)}\n";
                            text += $"Synonym{(termV.Synonyms.Count > 1 ? "s" : string.Empty)}:{string.Join("\n  * ", termV.Synonyms)}\n";

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
                            Console.WriteLine($"\"{term} ({typeS}.)\" already exists.");
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
                            Dictionary.Add(new(term, type));
                            // Call e
                        }
                        break;
                    case "e":

                        break;
                    case "d":
                        Dictionary.RemoveAll(i => i.Name == term && i.Type == type);
                        break;
                    case "s":
                        SaveDictionary();
                        break;
                    case "q":
                        SaveDictionary();
                        return;
                }
            }
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

        /// <summary>
        /// Deserialize and write the dictionary to a file.
        /// </summary>
        public static void SaveDictionary()
        {
            File.WriteAllText(Path, Utility.Serialize(Dictionary));
        }
    }
}
