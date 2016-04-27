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
using HexProcessor;

namespace PacketStructureViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Processor processor = new Processor();
            List<string> list = new List<string>();
            list.Add("int16");
            list.Add("int32");
            processor.ProcessString(hexBox.Text, list);

            resultBox.Items.Clear();
            List<string> resultList = processor.GetResult();
            foreach(string resString in resultList)
                resultBox.Items.Add(resString);
        }
    }
}
