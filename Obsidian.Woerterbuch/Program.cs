namespace Obsidian.Woerterbuch
{
    using Dictionary;
    using System;
    using System.Collections.Generic;
    using System.IO;
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
        }
    }
}
