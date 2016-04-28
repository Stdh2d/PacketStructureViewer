using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using HexProcessor;
using Newtonsoft.Json;
using Microsoft.Win32;

namespace PacketStructureViewer
{
    class SaveManager
    {
        private class SaveModel
        {
            public List<HexChunk> Chunks;
            public string HexChain;
        }

        public static void Save(string hexChain, Processor processor)
        {
            SaveModel sm = new SaveModel();
            sm.Chunks = processor.GetResult();
            sm.HexChain = hexChain;
            string toSave = JsonConvert.SerializeObject(sm);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "json file (*.json)|*.json";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, toSave);
        }

        public static void Load(ref string hexChain, ref Processor processor)
        {
            string toParse = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "json file (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
                toParse = File.ReadAllText(openFileDialog.FileName);
            SaveModel sm = JsonConvert.DeserializeObject<SaveModel>(toParse);
            hexChain = sm.HexChain;
            processor.SetChunks(sm.Chunks);
        }
    }
}
