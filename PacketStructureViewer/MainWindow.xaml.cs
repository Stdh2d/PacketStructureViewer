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
using System.Collections.ObjectModel;

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
        public ObservableCollection<string> TypeList { get; set; }

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

            ObservableCollection<DisplayedType> types = new ObservableCollection<DisplayedType>();
            types.Add(new DisplayedType());
            dataGrid.ItemsSource = types;

            TypeList = new ObservableCollection<string>() { "byte", "byte_array", "NOP", "int16", "int32", "int64", "string_unicode", "string_ascii"};
            typeComboBox.ItemsSource = TypeList;
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
            ObservableCollection<DisplayedType> types = new ObservableCollection<DisplayedType>();

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
                if (dt.Type != null)
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

        private void updateEditableCell()
        {
            var rows = GetDataGridRows(dataGrid);
            foreach (DataGridRow r in rows)
            {
                DisplayedType dt = (DisplayedType)r.Item;
                if (dt.Type != "byte_array" && dt.Type != "string_unicode" && dt.Type != "string_ascii")
                {
                    if(dt.Type != null)
                    {
                        dataGrid.Columns[1].IsReadOnly = true;
                        (dataGrid.Columns[1].GetCellContent(r).Parent as DataGridCell).Background = Brushes.LightGray;
                    }
                }
                else
                {
                    dataGrid.Columns[1].IsReadOnly = false;
                    (dataGrid.Columns[1].GetCellContent(r).Parent as DataGridCell).Background = Brushes.White;
                }
            }
            dataGrid.CommitEdit();
            dataGrid.CommitEdit();
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            updateEditableCell();
        }

        private void DataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            // Lookup for the source to be DataGridCell
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                // Starts the Edit on the row;
                DataGrid grd = (DataGrid)sender;
                grd.BeginEdit(e);
            }
        }
        
        private void SomeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateEditableCell();
        }
    }
}
