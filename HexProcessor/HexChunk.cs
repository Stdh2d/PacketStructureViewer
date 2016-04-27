using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexProcessor
{
    public class HexChunk
    {
        private CastType castType;
        public string CastTypeString
        {
            get { return castType.ToString(); }
        }

        private string hexString;
        public string HexString
        {
            get { return String.Join(" ", hexArray); }
        }

        private string[] hexArray;
        private int length;

        public int Size
        {
            get
            {
                return ByteSizeOfType(castType);
            }
        }

        int ByteSizeOfType(CastType type)
        {
            switch (type)
            {
                case CastType.BYTE:
                    return 1;
                case CastType.INT16:
                    return 2;
                case CastType.INT32:
                    return 4;
                case CastType.BYTE_ARRAY:
                case CastType.STRING_ASCII:
                case CastType.STRING_UNICODE:
                    return length;
                case CastType.INT64:
                default:
                    return 8;
            }
        }

        public void processChunk(string[] hexChain)
        {
            hexArray = hexChain;
            hexString = String.Concat(hexChain);
        }

        public void SetType(string type, int length = 0)
        {
            this.length = length;
            switch (type)
            {
                case "byte":
                    castType = CastType.BYTE;
                    break;
                case "int16":
                    castType = CastType.INT16;
                    break;
                case "int32":
                    castType = CastType.INT32;
                    break;
                case "int64":
                    castType = CastType.INT64;
                    break;
                case "string_unicode":
                    castType = CastType.STRING_UNICODE;
                    break;
                case "string_ascii":
                    castType = CastType.STRING_ASCII;
                    break;
                case "byte_array":
                    castType = CastType.BYTE_ARRAY;
                    break;
            }
        }

        public override string ToString()
        {
            string res = "";
            try
            {
                switch (castType)
                {
                    case CastType.BYTE:
                        res += HexConverter.GetByte(hexString).ToString();
                        break;
                    case CastType.INT16:
                        res += HexConverter.GetInt16(hexString).ToString();
                        break;
                    case CastType.INT32:
                        res += HexConverter.GetInt32(hexString).ToString();
                        break;
                    case CastType.INT64:
                        res += HexConverter.GetInt64(hexString).ToString();
                        break;
                    case CastType.STRING_ASCII:
                        res += HexConverter.GetStringASCII(hexString);
                        break;
                    case CastType.STRING_UNICODE:
                        res += HexConverter.GetStringUNICODE(hexString);
                        break;
                    case CastType.BYTE_ARRAY:
                        res += HexConverter.GetByteArray(hexString);
                        break;
                }
            }
            catch
            {
                res += "INVALID HEX";
            }
            return res;
        }
    }
}
