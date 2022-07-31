namespace Obsidian.Woerterbuch
{
    using Dictionary;
    using System.Collections.Generic;
    using System.IO;
    using Newtonsoft.Json;

    /// <summary>
    /// A class containing useful methods. Cannot be used in other assemblies.
    /// </summary>
    internal static class Utility
    {
        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>A JSON string of the object.</returns>
        public static string Serialize(object? value)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            jsonSerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            return JsonConvert.SerializeObject(value, jsonSerializerSettings);
        }
    }
}
