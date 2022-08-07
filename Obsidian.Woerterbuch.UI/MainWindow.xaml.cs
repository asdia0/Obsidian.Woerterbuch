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
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Utility.InitializeDictionary();
            Global.Dictionary = new ObservableCollection<Term>(Utility.Dictionary);
            this.termList.ItemsSource = Global.Dictionary;

            Global.Dictionary.Add(new("Tier", TermType.Noun));
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView _ListView = sender as ListView;
            GridView _GridView = _ListView.View as GridView;
            var _ActualWidth = _ListView.ActualWidth - SystemParameters.VerticalScrollBarWidth;
            for (Int32 i = 1; i < _GridView.Columns.Count; i++)
            {
                _ActualWidth = _ActualWidth - _GridView.Columns[i].ActualWidth;
            }
            _GridView.Columns[0].Width = _ActualWidth;
        }

        private void NewTerm_Click(object sender, RoutedEventArgs e)
        {
            new TermWindow(null).Show();
        }

        private void ListViewItem_Click(object sender, MouseButtonEventArgs e)
        {
            Term item = (Term)termList.SelectedItem;

            if (item != null)
            {
                new TermWindow(item).Show();
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().Show();
        }
    }
}
