using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexProcessor
{
    enum CastType
    {
        INT16,
        INT32,
        INT64
    }

    enum Endianness
    {
        BIG,
        LITTLE
    }

    public class Processor
    {
        private List<HexChunk> chunkList;

        public Processor()
        {
            chunkList = new List<HexChunk>();
        }

        public void ProcessString(string s, List<string> typeList)
        {
            string[] bytes = s.Split(' ');

            foreach(string typeString in typeList)
            {
                HexChunk hc = new HexChunk();
                hc.SetType(typeString);
                hc.processChunk(String.Concat(bytes.SubArray(0, hc.Size)), Endianness.LITTLE);
                chunkList.Add(hc);
            }
        }

        public string GetResult()
        {
            return String.Concat(chunkList);
        }
    }
}
