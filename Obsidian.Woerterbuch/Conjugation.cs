namespace Obsidian.Woerterbuch.Dictionary
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the conjugation of a verb in a specified tense.
    /// </summary>
    public class Conjugation
    {
        /// <summary>
        /// The conjugated verb in all tenses. Each array contains 6 elements, which are ordered as follows:
        /// 0. ich
        /// 1. fu
        /// 2. er/sie/es
        /// 3. wir
        /// 4. ihr
        /// 5. sie/Sie
        /// </summary>
        [JsonProperty]
        public Dictionary<Tense, string?[]> Conjugations { get; set; }

        public Conjugation()
        {
            this.Conjugations.Add(Tense.Present, new string[6]);
            this.Conjugations.Add(Tense.Perfect, new string[6]);
            this.Conjugations.Add(Tense.SimplePast, new string[6]);
            this.Conjugations.Add(Tense.PastPerfect, new string[6]);
            this.Conjugations.Add(Tense.Future1, new string[6]);
            this.Conjugations.Add(Tense.Future2, new string[6]);
        }

        public override string ToString()
        {
            return Utility.Serialize(this);
        }
    }
}
