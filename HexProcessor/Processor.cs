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
                int chunkSize = hc.Size;
                hc.processChunk(bytes.SubArray(0, chunkSize), Endianness.BIG);
                bytes = bytes.SubArray(chunkSize, bytes.Length - chunkSize - 1);
                chunkList.Add(hc);
            }
        }

        public List<string> GetResult()
        {
            List<string> result = new List<string>();

            foreach(HexChunk hc in chunkList)
            {
                result.Add(hc.ToString());
            }

            return result;
        }
    }
}
