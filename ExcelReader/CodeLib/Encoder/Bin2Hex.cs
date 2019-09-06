using System;
using System.Text;

namespace QiHe.CodeLib
{
    /// <summary>
    /// Binary data to Hexadecimal string
    /// </summary>
    public class Bin2Hex
    {
        public static string Encode(byte[] data)
        {
            StringBuilder code = new StringBuilder(data.Length * 2);
            foreach (byte bt in data)
            {
                code.Append(bt.ToString("X2"));
            }
            return code.ToString();
        }

        public static string Format(byte[] data)
        {
            StringBuilder code = new StringBuilder();
            int count = 0;
            foreach (byte bt in data)
            {
                code.AppendFormat("{0:X2} ", bt);
                count++;
                if (count == 16)
                {
                    code.AppendLine();
                    count = 0;
                }
            }
            return code.ToString();
        }

        public static byte[] Decode(string code)
        {
            byte[] bytes = new byte[code.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(code.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return bytes;
        }
    }
}
