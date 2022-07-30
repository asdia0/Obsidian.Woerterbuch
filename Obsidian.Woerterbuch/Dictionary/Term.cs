﻿namespace Obsidian.Woerterbuch.Dictionary
{
    using System.Collections.Generic;

    public class Term
    {
        // Universal properties
        /// <summary>
        /// The word or phrase.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of term.
        /// </summary>
        public TermType Type { get; set; }

        /// <summary>
        /// The definitions of the term.
        /// </summary>
        public List<string> Definitions { get; set; }

        /// <summary>
        /// The synonyms of the temr.
        /// </summary>
        public List<string> Synonyms { get; set; }

        // Noun properties
        /// <summary>
        /// The gender of the term. Only applies to nouns.
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// The plural form of the term. Only applies to nouns.
        /// </summary>
        public string? Plural { get; set; }

        // Verb properties
        /// <summary>
        /// The conjugated forms of the term in different tenses. Only applies to verbs.
        /// </summary>
        public List<Conjugation>? Conjugations { get; set; }

        // Verb + Preposition properties
        /// <summary>
        /// A value indicating whether the term can be written in Akkusativ. Only applies to verbs and prepositions.
        /// </summary>
        public bool? IsAkkusativ { get; set; }

        /// <summary>
        /// A value indicating whether the term can be written in Dativ. Only applies to verbs and prepositions.
        /// </summary>
        public bool? IsDativ { get; set; }

        public Term(string name, TermType type)
        {
            // Nullify all unessential properties on intialisation. Only fill in properties when edited.
            this.Name = name;
            this.Type = type;
            this.Definitions = new();
            this.Synonyms = new();
            this.Gender = null;
            this.Plural = null;
            this.Conjugations = null;
            this.IsDativ = null;
            this.IsAkkusativ = null;
        }
    }
}