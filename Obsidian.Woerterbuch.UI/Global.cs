namespace Obsidian.Woerterbuch.UI
{
    using Dictionary;
    using System.Collections.ObjectModel;
    using System.Linq;

    public static class Global
    {
        public static ObservableCollection<Term> Dictionary { get; set; }

        public static void SaveDictionary()
        {
            Utility.Dictionary = Dictionary.ToList();
            Utility.SaveDictionary();
        }
    }
}
