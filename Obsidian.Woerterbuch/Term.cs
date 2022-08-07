namespace Obsidian.Woerterbuch.Dictionary
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Term
    {
        // Universal properties
        /// <summary>
        /// The word or phrase.
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// The type of term.
        /// </summary>
        [JsonProperty]
        public TermType Type { get; set; }

        /// <summary>
        /// The definitions of the term.
        /// </summary>
        [JsonProperty]
        public List<string> Definitions { get; set; }

        /// <summary>
        /// The synonyms of the temr.
        /// </summary>
        [JsonProperty]
        public List<string> Synonyms { get; set; }

        // Noun properties
        /// <summary>
        /// The gender of the term. Only applies to nouns.
        /// </summary>
        [JsonProperty]
        public Gender? Gender { get; set; }

        /// <summary>
        /// The plural form of the term. Only applies to nouns.
        /// </summary>
        [JsonProperty]
        public string? Plural { get; set; }

        // Verb properties
        /// <summary>
        /// The conjugated forms of the term in different tenses. Only applies to verbs.
        /// </summary>
        [JsonProperty]
        public List<Conjugation>? Conjugations { get; set; }

        /// <summary>
        /// A value indicating whether the term is a regular verb. Only applies to verbs.
        /// </summary>
        public bool? IsRegular { get; set; }

        // Verb + Preposition properties
        /// <summary>
        /// A value indicating whether the term can be written in Akkusativ. Only applies to verbs and prepositions.
        /// </summary>
        [JsonProperty]
        public bool? IsAkkusativ { get; set; }

        /// <summary>
        /// A value indicating whether the term can be written in Dativ. Only applies to verbs and prepositions.
        /// </summary>
        [JsonProperty]
        public bool? IsDativ { get; set; }

        public Term(string name, TermType type)
        {
            // Set properties to default values.
            this.Name = name;
            this.Type = type;
            this.Definitions = new();
            this.Synonyms = new();

            // Nouns
            this.Gender = null;
            this.Plural = null;

            // Verbs
            if (this.Type == TermType.Verb)
            {
                this.Conjugations = new();
            }
            else
            {
                this.Conjugations = null;
            }
            this.IsRegular = null;

            // Verbs + Adjectives
            this.IsDativ = null;
            this.IsAkkusativ = null;
        }

        public override string ToString()
        {
            return Utility.Serialize(this);
        }
    }
}
