namespace Obsidian.Woerterbuch.Dictionary
{
    using System.Collections.Generic;
    using System.IO;
    using Newtonsoft.Json;

    /// <summary>
    /// A class containing useful methods. Cannot be used in other assemblies.
    /// </summary>
    public static class Utility
    {
        public static string Path = "dict.json";

        public static List<Term> Dictionary;

        public static void InitializeDictionary()
        {
            if (File.Exists(Path))
            {
                Dictionary = JsonConvert.DeserializeObject<List<Term>>(File.ReadAllText(Path));
                if (Dictionary == null)
                {
                    Dictionary = new();
                    SaveDictionary();
                }
            }
            else
            {
                File.Create(Path);
                Dictionary = new();
                SaveDictionary();
            }
        }

        /// <summary>
        /// Deserialize and write the dictionary to a file.
        /// </summary>
        public static void SaveDictionary()
        {
            File.WriteAllText(Path, Utility.Serialize(Dictionary));
        }

        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>A JSON string of the object.</returns>
        public static string Serialize(object? value)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            jsonSerializerSettings.Formatting = Formatting.Indented;
            return JsonConvert.SerializeObject(value, jsonSerializerSettings);
        }

    }
}
