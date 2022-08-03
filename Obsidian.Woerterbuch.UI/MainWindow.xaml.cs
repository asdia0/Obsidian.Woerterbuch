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
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Term> Dictionary { get; set; }

        public List<string> TermSource
        {
            get
            {
                return this.Dictionary.Select(i => i.Name).ToList();
            }
        }

        public List<string> ClassSource
        {
            get
            {
                return this.Dictionary.Select(i => i.Type.ToString()).ToList();
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            Utility.InitializeDictionary();
            this.Dictionary = Utility.Dictionary;

            Binding termBind = new("Terms");
            termBind.Mode = BindingMode.OneWay;
            termBind.Source = this.TermSource;

            Binding classBind = new("Classes");
            classBind.Mode = BindingMode.OneWay;
            classBind.Source = this.ClassSource;
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
    }
}
