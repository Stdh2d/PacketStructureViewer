using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexProcessor
{
    public class DisplayedType : INotifyPropertyChanged
    {
        private string type { get; set; }
        public string Type
        {
            get { return type; }

            set
            {
                if (value != type)
                {
                    type = value;
                    NotifyPropertyChanged("Type");
                }
            }
        }

        private int length { get; set; }
        public int Length
        {
            get { return length; }
            set
            {
                if (value != length)
                {
                    length = value;
                    NotifyPropertyChanged("Length");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public DisplayedType()
        {
            type = "";
            length = 0;
        }
    }
}
