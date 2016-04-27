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
        public class DisplayedChunk
        {
            public string Type { get; set; }
            public string Value { get; set; }
            public string Hex { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            var gridView = new GridView();
            this.listView.View = gridView;
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Type",
                DisplayMemberBinding = new Binding("Type")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Value",
                DisplayMemberBinding = new Binding("Value")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Hex",
                DisplayMemberBinding = new Binding("Hex")
            });
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Processor processor = new Processor();
            List<string> list = new List<string>();
            list = typeList.Text.Split(' ').ToList<string>();
            processor.ProcessString(hexBox.Text, list);
            
            listView.Items.Clear();
            List<HexChunk> resultList = processor.GetResult();
            foreach(HexChunk chunk in resultList)
                this.listView.Items.Add(new DisplayedChunk {
                    Type = chunk.CastTypeString,
                    Value = chunk.ToString(),
                    Hex = chunk.HexString
                });
        }
    }
}
