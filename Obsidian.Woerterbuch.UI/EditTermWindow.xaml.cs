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
    /// Interaction logic for Term.xaml
    /// </summary>
    public partial class EditTermWindow : Window
    {
        /// <summary>
        /// The term to edit. If `null`, add new term.
        /// </summary>
        public Term? Term { get; init; }

        public EditTermWindow(Term? term)
        {
            this.Term = term;
            InitializeComponent();
            this.Title += $": {this.Term.Name} ({this.Term.Type})";
        }
    }
}
