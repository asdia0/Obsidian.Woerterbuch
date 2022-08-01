namespace Obsidian.Woerterbuch.Dictionary
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;

    public static class Dictionary
    {
        private static string Path = "dict.json";

        private static string Separator = ",,";

        public static List<Term> Dict;

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

        readonly static Dictionary<string, Gender> StringToGender = new()
        {
            { "r", Gender.Masculine },
            { "e", Gender.Feminine },
            { "s", Gender.Neutral }
        };

        readonly static Dictionary<string, Tense> StringToTense = new()
        {
            { "pre", Tense.Present },
            { "per", Tense.Perfect },
            { "sp", Tense.SimplePast },
            { "pp", Tense.PastPerfect },
            { "f1", Tense.Future1 },
            { "f2", Tense.Future2 },
        };

        readonly static List<string> ValidOptionsMain = new()
        {
            "v",
            "a",
            "e",
            "d"
        };

        readonly static List<string> ValidOptionsEdit = new()
        {
            "n",
            "t",
            "d",
            "s",
            "g",
            "p",
            "c",
            "akk",
            "dat"
        };

        readonly static List<string> ValidOptionsN = new()
        {
            "g",
            "p"
        };

        readonly static List<string> ValidOptionsV = new()
        {
            "c"
        };

        readonly static List<string> ValidOptionsVA = new()
        {
            "akk",
            "dat"
        };

        public static void IntializeDictionary()
        {
            if (File.Exists(Path))
            {
                Dict = JsonConvert.DeserializeObject<List<Term>>(File.ReadAllText(Path));
                if (Dict == null)
                {
                    Dict = new();
                }
            }
            else
            {
                File.Create(Path);
                Dict = new();
            }
        }

        public static void Loop()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("Options:\nv <term> <type> - View term\na <term> <type> - Add term\ne <term> <type> - Edit term\nd <term> <type> - Delete term\n");
                Console.WriteLine("Types:\n  * n - noun\n  * v - verb\n  * adv - adverb\n  * adj- adjective\n  * pro - pronoun\n  * preposition\n  * c - conjunction\n  * d - determiner\n  * e - exclamation\n");

                List<string> inputs = Console.ReadLine().Split(Separator).ToList();
                string option = inputs[0].ToLower();
                string term = (inputs.Count > 1 ? inputs[1] : string.Empty);
                string typeS = (inputs.Count > 2 ? inputs[2] : string.Empty);

                if (!ValidOptionsMain.Contains(option))
                {
                    Console.WriteLine($"\n\"{option}\" is not a recognised option.");
                }

                if (inputs.Count != 3)
                {
                    Console.WriteLine("\nIncomplete argument. Please input a term and type.");
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
                            Dict.Add(new(term, type));
                            SaveDictionary();
                            TermEditor(term, type);
                        }
                        break;
                    case "e":
                        TermEditor(term, type);
                        break;
                    case "d":
                        Dict.RemoveAll(i => i.Name == term && i.Type == type);
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
            File.WriteAllText(Path, Utility.Serialize(Dict));
        }

        public static bool TermExists(string term, TermType type)
        {
            return Dict.Where(i => i.Name == term && i.Type == type).Any();
        }

        public static void TermViewer(string term, TermType type)
        {
            Term termV = Dict.Where(i => i.Name == term && i.Type == type).First();

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

            Term termE = Dict.Where(i => i.Name == term && i.Type == type).First();

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
            Console.WriteLine("\nIndexes start from 0.");
            Console.WriteLine("To add a new entry, set index to -1.");
            Console.WriteLine("To delete an entry, set content to \"[REMOVE ENTRY]\".\n");
            Console.WriteLine("Types:\n  * n - noun\n  * v - verb\n  * adv - adverb\n  * adj- adjective\n  * pro - pronoun\n  * preposition\n  * c - conjunction\n  * d - determiner\n  * e - exclamation\n");
            if (type == TermType.Noun)
            {
                Console.WriteLine("Gender:\n  * r - masculine (der)\n  * e - feminine (die)\n  * s - neutral (das)\n");
            }
            if (type == TermType.Verb)
            {
                Console.WriteLine("Tense:\n  * pre - present (Präsens)\n  * per - perfect (Perfekt)\n  * sp - simple past (Präteritum)\n  * pp - past perfect (Plusquamperfekt)\n  * f1 - future 1 (Futur 1)\n  * f2 - future 2 (Futur 2)\n");
            }
            if (type == TermType.Verb || type == TermType.Preposition)
            {
                Console.WriteLine("Booleans:\n  * 1 - true\n  * anything else - false\n");
            }

            List<string> arguments = Console.ReadLine().Split(Separator).ToList();

            if (!ValidOptionsEdit.Contains(arguments[0]) 
                || (type != TermType.Noun && ValidOptionsN.Contains(arguments[0]))
                || (type != TermType.Verb && ValidOptionsV.Contains(arguments[0]))
                || (type != TermType.Verb && type != TermType.Preposition && ValidOptionsVA.Contains(arguments[0])))
            {
                Console.WriteLine($"\n\"{arguments[0]}\" is an invalid option.");
            }

            int count = arguments.Count;

            switch (arguments[0])
            {
                case "n":
                    if (CheckArgumentLength(arguments, 2))
                    {
                        termE.Name = arguments[1];
                    }
                    break;
                case "t":
                    if (CheckArgumentLength(arguments, 2))
                    {
                        if (StringToType.ContainsKey(arguments[1]))
                        {
                            termE.Type = StringToType[arguments[1]];
                        }
                        else
                        {
                            Console.WriteLine($"\"{arguments[1]}\" is not a valid word type.");
                        }
                    }
                    break;
                case "d":
                    if (CheckArgumentLength(arguments, 3))
                    {
                        int index = int.Parse(arguments[1]);

                        if (index == -1)
                        {
                            termE.Definitions.Add(arguments[2]);
                        }
                        else if (0 <= index && index < termE.Definitions.Count)
                        {
                            if (arguments[2] == "[REMOVE ENTRY]")
                            {
                                termE.Definitions.RemoveAt(index);
                                break;
                            }
                            termE.Definitions[index] = arguments[2];
                        }
                        else
                        {
                            Console.WriteLine($"Invalid index ({index}).");
                        }
                    }
                    break;
                case "s":
                    if (CheckArgumentLength(arguments, 3))
                    {
                        int index = int.Parse(arguments[1]);

                        if (index == -1)
                        {
                            termE.Synonyms.Add(arguments[2]);
                        }
                        else if (0 <= index && index < termE.Synonyms.Count)
                        {
                            if (arguments[2] == "[REMOVE ENTRY]")
                            {
                                termE.Synonyms.RemoveAt(index);
                                break;
                            }
                            termE.Synonyms[index] = arguments[2];
                        }
                        else
                        {
                            Console.WriteLine($"Invalid index ({index}).");
                        }
                    }
                    break;
                case "g":
                    if (CheckArgumentLength(arguments, 2))
                    {
                        if (StringToGender.ContainsKey(arguments[1]))
                        {
                            termE.Gender = StringToGender[arguments[1]];
                        }
                        else
                        {
                            Console.WriteLine($"\"{arguments[1]}\" is not a valid gender.");
                        }
                    }
                    break;
                case "p":
                    if (CheckArgumentLength(arguments, 2))
                    {
                        termE.Plural = arguments[1];
                    }
                    break;
                case "c":
                    if (CheckArgumentLength(arguments, 4))
                    {
                        if (StringToTense.ContainsKey(arguments[1]))
                        {
                            int index = int.Parse(arguments[2]);
                            if (0 <= index && index < 7)
                            {
                                Tense tense = StringToTense[arguments[1]];
                                // Create conjugation if it does not exist.
                                if (!termE.Conjugations.Where(i => i.Tense == tense).Any())
                                {
                                    termE.Conjugations.Add(new(term, tense));
                                }
                                termE.Conjugations.Where(i => i.Tense == tense).First().Conjugations[index] = arguments[3];
                            }
                            else
                            {
                                Console.WriteLine($"Invalid index ({index}).");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"\"{arguments[1]}\" is not a valid tense.");
                        }
                    }
                    break;
                case "akk":
                    if (CheckArgumentLength(arguments, 2))
                    {
                        termE.IsAkkusativ = arguments[1] == "1";
                    }
                    break;
                case "dat":
                    if (CheckArgumentLength(arguments, 2))
                    {
                        termE.IsDativ = arguments[1] == "1";
                    }
                    break;
            }

            SaveDictionary();
        }

        public static bool CheckArgumentLength(List<string> arguments, int count)
        {
            if (arguments.Count != count)
            {
                Console.WriteLine("\nIncomplete argument.");
                return false;
            }
            return true;
        }
    }
}
