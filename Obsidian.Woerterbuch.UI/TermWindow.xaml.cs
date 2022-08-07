namespace Obsidian.Woerterbuch.UI
{
    using Dictionary;
    using System;
    using System.Collections.Generic;
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
    }
}
