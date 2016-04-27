using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace HexProcessor
{
    class HexConverter
    {

        public static short GetInt16(string s)
        {
            return Int16.Parse(s, NumberStyles.HexNumber);
        }

        public static int GetInt32(string s)
        {
            return Int32.Parse(s, NumberStyles.HexNumber);
        }

        public static long GetInt64(string s)
        {
            return Int64.Parse(s, NumberStyles.HexNumber);
        }

        public static byte GetByte(string s)
        {
            return Byte.Parse(s, NumberStyles.HexNumber);
        }

        public static byte[] GetByteArray(string s)
        {
            int NumberChars = s.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
            return bytes;
        }

        public static string GetStringASCII(string s)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetChars(GetByteArray(s)).ToString();
        }

        public static string GetStringUNICODE(string s)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            return encoding.GetChars(GetByteArray(s)).ToString();
        }
    }
}
