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
        BYTE_ARRAY
    }

    public enum Endianness
    {
        BIG,
        LITTLE
    }

    public class DisplayedType
    {
        public string Type { get; set; }
        public int Length { get; set; }
    }

    public class Processor
    {
        private List<HexChunk> chunkList;

        public Processor()
        {
            chunkList = new List<HexChunk>();
        }
        
        static string[] SwapByteEndianness(string[] bytes)
        {
            for (int i = 0; i < bytes.Length - 1; i += 2)
            {
                string swap = bytes[i];
                bytes[i] = bytes[i + 1];
                bytes[i + 1] = swap;
            }
            return bytes;
        }

        public void ProcessString(string s, List<DisplayedType> typeList, Endianness endian)
        {
            string[] bytes = s.Split(' ');

            if (endian == Endianness.LITTLE)
                SwapByteEndianness(bytes);

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
