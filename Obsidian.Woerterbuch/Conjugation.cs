namespace Obsidian.Woerterbuch
{
    public class Conjugation
    {
        public Tense Tense { get; set; }

        public string[] Conjugations { get; set; }

        public Conjugation(string infinitive, Tense tense)
        {
            this.Tense = tense;
            this.Conjugations = new string[7];
            this.Conjugations[0] = infinitive;
        }
    }
}
