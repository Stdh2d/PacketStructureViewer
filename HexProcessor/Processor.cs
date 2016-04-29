using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexProcessor
{
    public enum CastType
    {
        BYTE,
        INT16,
        INT32,
        INT64,
        STRING_ASCII,
        STRING_UNICODE,
        BYTE_ARRAY,
        NOP
    }

    public enum Endianness
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

        public void ProcessString(string s, List<DisplayedType> typeList)
        {
            string[] bytes = s.Split(' ');

            foreach (DisplayedType dt in typeList)
            {
                HexChunk hc = new HexChunk();
                hc.SetType(dt.Type, dt.Length);
                int chunkSize = hc.Size;

                if (bytes.Length < chunkSize)
                    return;

                hc.processChunk(bytes.SubArray(0, chunkSize));
                bytes = bytes.SubArray(chunkSize, bytes.Length - chunkSize);
                chunkList.Add(hc);
            }
        }

        public List<HexChunk> GetResult()
        {
            return chunkList;
        }

        public void SetChunks(List<HexChunk> chunks)
        {
            chunkList = chunks;
        }

        public void Reset()
        {
            chunkList.Clear();
        }
    }
}
