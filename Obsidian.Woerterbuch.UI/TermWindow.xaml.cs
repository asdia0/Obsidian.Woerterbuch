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

        public ObservableCollection<string> Definitions { get; set; }

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

            this.Type_Value.ItemsSource = Enum.GetValues(typeof(TermType));
            this.Gender_Value.ItemsSource = Enum.GetValues(typeof(Gender));

            this.Definitions = new();
            this.Definitions_Value.ItemsSource = this.Definitions;

            this.UpdateTitle();
            this.UpdateVisibility();
            
            SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
        }

        private void UpdateVisibility()
        {
            if (IsEdit)
            {
                this.Noun_Group.Visibility = Visibility.Collapsed;
                this.VerbPrep_Group.Visibility = Visibility.Collapsed;
                this.VerbPrep_Group.Visibility = Visibility.Collapsed;
            }
            else
            {
                TermType type = (TermType)Enum.Parse(typeof(TermType), this.Type_Value.SelectedValue.ToString());
                switch (type)
                {
                    case TermType.Noun:
                        this.VerbPrep_Group.Visibility = Visibility.Collapsed;
                        this.Verb_Group.Visibility = Visibility.Collapsed;
                        break;
                    case TermType.Verb:
                        this.VerbPrep_Group.Visibility = Visibility.Collapsed;
                        break;
                    case TermType.Preposition:
                        this.Noun_Group.Visibility = Visibility.Collapsed;
                        this.Verb_Group.Visibility = Visibility.Collapsed;
                        break;
                    default:
                        break;

                }
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

        }

        private void Definitions_Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Synonyms_Add_Click(object sender, RoutedEventArgs e)
        {

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
