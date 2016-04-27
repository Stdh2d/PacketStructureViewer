using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace HexProcessor
{
    class HexChunk
    {
        private CastType castType;
        private string hexString;

        public int Size
        {
            get
            {
                return ByteSizeOfType(castType);
            }
        }

        static int ByteSizeOfType(CastType type)
        {
            switch(type)
            {
                case CastType.INT16:
                    return 2;
                case CastType.INT32:
                    return 4;
                case CastType.INT64:
                default:
                    return 8;
            }
        }

        static string SwapByteEndianness(string hexChain)
        {
            string s = "";
            s += hexChain[2];
            s += hexChain[3];
            s += hexChain[0];
            s += hexChain[1];
            return s;
        }

        public void processChunk(string hexChain, Endianness endianness)
        {
            if(endianness == Endianness.LITTLE)
                hexString = SwapByteEndianness(hexChain);
            else
                hexString = hexChain;
        }

        public void SetType(string type)
        {
            switch(type)
            {
                case "int16":
                    castType = CastType.INT16;
                    break;
                case "int32":
                    castType = CastType.INT32;
                    break;
                case "int64":
                    castType = CastType.INT64;
                    break;
            }
        }

        public int getInt()
        {
            try
            {
                return Int16.Parse(hexString, NumberStyles.HexNumber);
            }
            catch
            {
                return -1;
            }
        }

        public override string ToString()
        {
            string res = "";
            switch(castType)
            {
                case CastType.INT16:
                    res = getInt().ToString();
                    break;
            }
            return res;
        }
    }
}
