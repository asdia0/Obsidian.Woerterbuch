namespace Obsidian.Woerterbuch.Dictionary
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the conjugation of a verb in a specified tense.
    /// </summary>
    public class Conjugation
    {
        /// <summary>
        /// The tense the verb is being conjugated to.
        /// </summary>
        [JsonProperty]
        public Tense Tense { get; set; }

        /// <summary>
        /// The conjugated verb. Contains 7 elements, which are ordered as follows:
        /// 0. infinitive
        /// 1. ich
        /// 2. fu
        /// 3. er/sie/es
        /// 4. wir
        /// 5. ihr
        /// 6. sie/Sie
        /// </summary>
        [JsonProperty]
        public string[] Conjugations { get; set; }

        public Conjugation(string infinitive, Tense tense)
        {
            this.Tense = tense;
            this.Conjugations = new string[7];
            this.Conjugations[0] = infinitive;
        }
    }
}
