namespace Obsidian.Woerterbuch.UI
{
    using Dictionary;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for TermWindow.xaml
    /// </summary>
    public partial class TermWindow : Window
    {
        /// <summary>
        /// The term to edit. If `null`, add new term.
        /// </summary>
        public Term? Term { get; init; }

        public ObservableCollection<TotallyNotAString> Definitions { get; set; }

        public ObservableCollection<TotallyNotAString> Synonyms { get; set; }

        public static RoutedCommand SaveCommand = new();

        /// <summary>
        /// Returns `true` if in editing mode (not adding a term).
        /// </summary>
        public bool IsEdit
        {
            get
            {
                if (Term == null)
                {
                    return false;
                }
                return true;
            }
        }
        public TermWindow(Term? term)
        {
            this.Term = term;
            InitializeComponent();
            this.Definitions = new(this.Term.Definitions.Select(i => new TotallyNotAString(i)));
            this.Synonyms = new(this.Term.Synonyms.Select(i => new TotallyNotAString(i)));

            this.Type_Value.ItemsSource = Enum.GetValues(typeof(TermType));
            this.Gender_Value.ItemsSource = Enum.GetValues(typeof(Gender));

            this.Definitions_Value.ItemsSource = this.Definitions;
            this.Synonyms_Value.ItemsSource = this.Synonyms;

            this.Term_Value.Text = this.Term.Name;
            this.Type_Value.SelectedItem = this.Term.Type;

            this.UpdateTitle();
            this.UpdateVisibility();
            
            SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
        }

        private void UpdateVisibility()
        {
            if (IsEdit)
            {
                TermType type = (TermType)Enum.Parse(typeof(TermType), this.Type_Value.SelectedValue.ToString());
                switch (type)
                {
                    case TermType.Noun:
                        this.Noun_Group.Visibility = Visibility.Visible;
                        this.VerbPrep_Group.Visibility = Visibility.Collapsed;
                        this.Verb_Group.Visibility = Visibility.Collapsed;

                        this.Gender_Value.SelectedItem = this.Term.Gender;
                        this.Plural_Value.Text = this.Term.Plural;
                        break;
                    case TermType.Verb:
                        this.Verb_Group.Visibility = Visibility.Visible;
                        this.VerbPrep_Group.Visibility = Visibility.Visible;
                        this.Noun_Group.Visibility = Visibility.Collapsed;
                        break;
                    case TermType.Preposition:
                        this.VerbPrep_Group.Visibility = Visibility.Visible;
                        this.Noun_Group.Visibility = Visibility.Collapsed;
                        this.Verb_Group.Visibility = Visibility.Collapsed;
                        break;
                    default:
                        break;

                }
            }
            else
            {
                this.Noun_Group.Visibility = Visibility.Collapsed;
                this.VerbPrep_Group.Visibility = Visibility.Collapsed;
                this.Verb_Group.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateTitle()
        {
            if (IsEdit)
            {
                this.Title = $"Editing {this.Term.Name} ({this.Term.Type})";
            }
            else
            {
                this.Title = "Adding new term";
            }
        }

        private void SaveTerm_Click(object sender, RoutedEventArgs e)
        {
            this.Term.Name = this.Term_Value.Text;
            this.Term.Type = (TermType)Enum.Parse(typeof(TermType), this.Type_Value.SelectedItem.ToString());
            this.Term.Definitions = this.Definitions.Where(i => i.NotAString != string.Empty).Select(i => i.NotAString).ToList();
            this.Term.Synonyms = this.Synonyms.Where(i => i.NotAString != string.Empty).Select(i => i.NotAString).ToList();

            // Edit applicable properties and nullify all others.
            switch (this.Term.Type)
            {
                case TermType.Noun:
                    this.Term.Gender = (Gender)Enum.Parse(typeof(Gender), this.Gender_Value.SelectedItem.ToString());
                    this.Term.Plural = this.Plural_Value.Text;
                    this.Term.Conjugations = null;
                    this.Term.IsRegular = null;
                    this.Term.IsAkkusativ = null;
                    this.Term.IsDativ = null;
                    break;
                case TermType.Verb:
                    this.Term.Gender = null;
                    this.Term.Plural = null;
                    #region Parse conjugations
                    string[] present = new string[6]
                    {
                        this.Praesens_ich_Value.Text,
                        this.Praesens_du_Value.Text,
                        this.Praesens_er_Value.Text,
                        this.Praesens_wir_Value.Text,
                        this.Praesens_ihr_Value.Text,
                        this.Praesens_Sie_Value.Text,
                    };

                    string[] perfect = new string[6]
                    {
                        this.Perfekt_ich_Value.Text,
                        this.Perfekt_du_Value.Text,
                        this.Perfekt_er_Value.Text,
                        this.Perfekt_wir_Value.Text,
                        this.Perfekt_ihr_Value.Text,
                        this.Perfekt_Sie_Value.Text,
                    };

                    string[] simplePast = new string[6]
                    {
                        this.Präteritum_ich_Value.Text,
                        this.Präteritum_du_Value.Text,
                        this.Präteritum_er_Value.Text,
                        this.Präteritum_wir_Value.Text,
                        this.Präteritum_ihr_Value.Text,
                        this.Präteritum_Sie_Value.Text,
                    };

                    string[] pastPerfect = new string[6]
                    {
                        this.Plusquamperfekt_ich_Value.Text,
                        this.Plusquamperfekt_du_Value.Text,
                        this.Plusquamperfekt_er_Value.Text,
                        this.Plusquamperfekt_wir_Value.Text,
                        this.Plusquamperfekt_ihr_Value.Text,
                        this.Plusquamperfekt_Sie_Value.Text,
                    };

                    string[] future1 = new string[6]
                    {
                        this.Futur1_ich_Value.Text,
                        this.Futur1_du_Value.Text,
                        this.Futur1_er_Value.Text,
                        this.Futur1_wir_Value.Text,
                        this.Futur1_ihr_Value.Text,
                        this.Futur1_Sie_Value.Text,
                    };

                    string[] future2 = new string[6]
                    {
                        this.Futur2_ich_Value.Text,
                        this.Futur2_du_Value.Text,
                        this.Futur2_er_Value.Text,
                        this.Futur2_wir_Value.Text,
                        this.Futur2_ihr_Value.Text,
                        this.Futur2_Sie_Value.Text,
                    };

                    this.Term.Conjugations = new();
                    this.Term.Conjugations.Conjugations[Tense.Present] = present;
                    this.Term.Conjugations.Conjugations[Tense.Perfect] = perfect;
                    this.Term.Conjugations.Conjugations[Tense.SimplePast] = simplePast;
                    this.Term.Conjugations.Conjugations[Tense.PastPerfect] = pastPerfect;
                    this.Term.Conjugations.Conjugations[Tense.Future1] = future1;
                    this.Term.Conjugations.Conjugations[Tense.Future2] = future2;

                    foreach (string[] arr in this.Term.Conjugations.Conjugations.Values)
                    {
                        for (int i = 0; i <= 5; i++)
                        {
                            if (arr[i] == string.Empty)
                            {
                                arr[i] = null;
                            }
                        }
                    }
                    #endregion
                    this.Term.IsRegular = this.Regular_Value.IsChecked;
                    this.Term.IsAkkusativ = this.Akk_Value.IsChecked;
                    this.Term.IsDativ = this.Dat_Value.IsChecked;
                    break;
                case TermType.Preposition:
                    this.Term.Gender = null;
                    this.Term.Plural = null;
                    this.Term.Conjugations = null;
                    this.Term.IsRegular = null;
                    this.Term.IsAkkusativ = this.Akk_Value.IsChecked;
                    this.Term.IsDativ = this.Dat_Value.IsChecked;
                    break;
                default:
                    this.Term.Gender = null;
                    this.Term.Plural = null;
                    this.Term.Conjugations = null;
                    this.Term.IsRegular = null;
                    this.Term.IsAkkusativ = null;
                    this.Term.IsDativ = null;
                    break;
            }

            Global.SaveDictionary();
            this.UpdateTitle();
            this.UpdateVisibility();
            
            MessageBox.Show($"{this.Term.Name} ({this.Term.Type}) saved successfully.", "Saved!");
        }

        private void DeleteTerm_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Global.Dictionary.Remove(this.Term);
                Global.SaveDictionary();
            }

            this.Close();
        }

        private void Type_Value_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.UpdateVisibility();
        }

        private void Definitions_Add_Click(object sender, RoutedEventArgs e)
        {
            this.Definitions.Add(new(string.Empty));
        }

        private void Synonyms_Add_Click(object sender, RoutedEventArgs e)
        {
            this.Synonyms.Add(new(string.Empty));
        }

        private void SaveCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.SaveTerm_Click(sender, e);
        }
    }

    public class LabelValueGrid : Grid
    {
        public LabelValueGrid()
            : base()
        {
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions[0].Width = new GridLength(3, GridUnitType.Star);
            ColumnDefinitions[1].Width = new GridLength(15, GridUnitType.Star);
            ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);

            int curRow = -1;
            int curCol = 1;

            RowDefinitions.Clear();

            if (Children != null)
                foreach (UIElement curChild in Children)
                {
                    if (curCol == 0)
                        curCol = 1;
                    else
                    {
                        curCol = 0;
                        curRow++;

                        RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Auto) });
                    }

                    SetRow(curChild, curRow);
                    SetColumn(curChild, curCol);
                }

            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Auto) });
        }
    }
}
