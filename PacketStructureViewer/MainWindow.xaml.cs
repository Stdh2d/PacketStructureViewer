using System;
using System.Collections;
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


        private bool switchEndian = false;
        private Endianness endian;

        public MainWindow()
        {
            InitializeComponent();
            endian = Endianness.BIG;

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

            List<DisplayedType> types = new List<DisplayedType>();
            DisplayedType dt = new DisplayedType();
            dt.Type = "int16";
            types.Add(dt);

            dataGrid.ItemsSource = types;
        }

        public IEnumerable<System.Windows.Controls.DataGridRow> GetDataGridRows(System.Windows.Controls.DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as System.Windows.Controls.DataGridRow;
                if (null != row) yield return row;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            processData();
        }

        private void button_SwitchEndian(object sender, RoutedEventArgs e)
        {
            switchEndian = !switchEndian;
            if (switchEndian)
                endian = Endianness.LITTLE;
            else
                endian = Endianness.BIG;
            processData();
        }

        private void processData()
        {
            Processor processor = new Processor();
            List<DisplayedType> list = new List<DisplayedType>();

            foreach (DisplayedType dt in dataGrid.ItemsSource)
            {
                list.Add(dt);
            }

            processor.ProcessString(hexBox.Text, list, endian);

            listView.Items.Clear();
            List<HexChunk> resultList = processor.GetResult();
            foreach (HexChunk chunk in resultList)
                this.listView.Items.Add(new DisplayedChunk
                {
                    Type = chunk.CastTypeString,
                    Value = chunk.ToString(),
                    Hex = chunk.HexString
                });
        }
    }
}
