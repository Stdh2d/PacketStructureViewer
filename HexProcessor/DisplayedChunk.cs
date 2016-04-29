using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexProcessor
{
    public class DisplayedChunk : INotifyPropertyChanged
    {
        private string type;
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

        private string mValue;
        public string Value
        {
            get { return mValue; }

            set
            {
                if (mValue != value)
                {
                    mValue = value;
                    NotifyPropertyChanged("Value");
                }
            }
        }

        private string hex;
        public string Hex
        {
            get { return hex; }

            set
            {
                if (value != hex)
                {
                    hex = value;
                    NotifyPropertyChanged("Hex");
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

        public DisplayedChunk()
        {
            type = "";
            mValue = "";
            hex = "";
        }
    }
}
