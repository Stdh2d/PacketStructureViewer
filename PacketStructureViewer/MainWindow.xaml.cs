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

        private string currentFilePath;
        private bool switchEndian = false;
        private Endianness endian;
        Processor processor = new Processor();

        public MainWindow()
        {
            InitializeComponent();
            endian = Endianness.BIG;
            currentFilePath = "";

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
            types.Add(new DisplayedType());
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

        private void OpenFile_button(object sender, RoutedEventArgs e)
        {
            string hex = "";
            SaveManager.Load(ref hex, ref processor);
            hexBox.Text = hex;
            List<HexChunk> chunks = processor.GetResult();
            List<DisplayedType> types = new List<DisplayedType>();
            
            foreach (HexChunk chunk in chunks)
            {
                DisplayedType dt = new DisplayedType();
                dt.Type = chunk.CastTypeString;
                dt.Length = chunk.Size;
                types.Add(dt);
            }

            dataGrid.ItemsSource = types;
        }

        private void SaveFile_button(object sender, RoutedEventArgs e)
        {
            processData();
            if (currentFilePath == "")
            {
                SaveAsFile_button(sender, e);
            }
            else
            {
                SaveManager.Save(hexBox.Text, processor);
            }
        }
        
        private void SaveAsFile_button(object sender, RoutedEventArgs e)
        {
            SaveManager.Save(hexBox.Text, processor);
        }

        private void NewFile_button(object sender, RoutedEventArgs e)
        {
            currentFilePath = "";
            hexBox.Text = "";
            dataGrid.Items.Clear();
            listView.Items.Clear();
        }

        private string browseFiles()
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".json";
            dlg.Filter = "JSON Files (*.json)|*.json";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                return dlg.FileName;
            }
            return "";
        }

        private void processData(bool reset = true)
        {
            processor.Reset();
            List<DisplayedType> list = new List<DisplayedType>();

            foreach (DisplayedType dt in dataGrid.ItemsSource)
            {
                if(dt.Type != null)
                    list.Add(dt);
            }

            processor.ProcessString(hexBox.Text, list);

            listView.Items.Clear();
            List<HexChunk> resultList = processor.GetResult();
            foreach (HexChunk chunk in resultList)
                this.listView.Items.Add(new DisplayedChunk
                {
                    Type = chunk.CastTypeString,
                    Value = chunk.ToString(endian),
                    Hex = chunk.HexString
                });
        }
    }
}
