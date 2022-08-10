using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obsidian.Woerterbuch.UI
{
    using System.ComponentModel;

    public class TotallyNotAString : INotifyPropertyChanged
    {
        private string _notAString;
        public string NotAString
        {
            get { return _notAString; }
            set { _notAString = value; OnPropertyChanged(nameof(NotAString)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TotallyNotAString(string str)
        {
            this.NotAString = str;
        }
    }
}
