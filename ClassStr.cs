using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;

namespace Commons
{
    public class ClassStr
    {
        public static string LeftByte(object ostr, int vLength, string vstrExtend)
        {
            string vstr;
            if (ostr == null)
            {
                vstr = "";
            }
            else
            {
                vstr = ostr.ToString();
            }

            string strRtn;
            if (vLength >= vstr.Length * 2)
            {
                strRtn = vstr;
            }
            else
            {
                Int16 LenCount;
                int entercnt = 0;
                bool blnOverLength = false;

                for (LenCount = 1; LenCount <= vstr.Length; LenCount++)
                {
                    entercnt = entercnt + 1;
                    // '�Y�J�� DoubleByte(����r)�A�hentercnt(�r��)�[1 
                    if (Microsoft.VisualBasic.Strings.Asc(Microsoft.VisualBasic.Strings.Mid(vstr, LenCount, 1)) < 0)
                        entercnt = entercnt + 1;
                    // '�Y�r�ƶW�L�̤j�ȫh���} 
                    if (entercnt > vLength)
                    {
                        blnOverLength = true;
                        break; // TODO: might not be correct. Was : Exit For 
                    }
                }
                if (blnOverLength)
                {
                    return vstr.Substring(0, LenCount - 1) + vstrExtend;
                }
                else
                {
                    return vstr.Substring(0, LenCount - 1);
                }
            }
            return strRtn;
        }

        public static int GetstringLen(object ostr)
        {
            string vstr;
            if (ostr == null)
            {
                vstr = "";
            }
            else
            {
                vstr = ostr.ToString();
            }

            Int16 LenCount;
            int entercnt = 0;

            for (LenCount = 1; LenCount <= vstr.Length; LenCount++)
            {
                entercnt = entercnt + 1;
                // '�Y�J�� DoubleByte(����r)�A�hentercnt(�r��)�[1 
                if (Microsoft.VisualBasic.Strings.Asc(Microsoft.VisualBasic.Strings.Mid(vstr, LenCount, 1)) < 0)
                    entercnt = entercnt + 1;
            }

            return entercnt;
        }

        public static string[] splitString(string input, string delimiter)
        {
            string[] split = null;
            char[] delimit = delimiter.ToCharArray();
            split = input.Split(delimit);
            return split;
        }

        /// <summary>
        /// �h�Ӥ�r���r����
        /// </summary>
        /// <param name="input">�ݩ�Ѫ��r��</param>
        /// <param name="separator">�h�Ӥ�r new string[] { "��r1", "��r2" ... }</param>
        /// <returns></returns>
        public static string[] splitString(string input, string[] separator)
        {
            //string[] separator = new string[] { "||", "," };
            return input.Split(separator, StringSplitOptions.None);
        }

        public static string[] RegexSplit(string input, string delimiter)
        {
            string[] split = null;
            split = Regex.Split(input, delimiter, RegexOptions.IgnoreCase);
            return split;
        }

        /// <summary>
        /// �r�������
        /// </summary>
        /// <param name="input">���@�r����</param>
        /// <returns>���Φr����</returns>
        public static string StrConvWide(string input)
        {
            //�b������ΡG
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                //���ΪŮ欰12288�A�b�ΪŮ欰32
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                //&#55422;&#56357;
                //��L�r���b��(33-126)�P����(65281-65374)���������Y�O�G���ۮt65248
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// �r�������
        /// </summary>
        /// <param name="input">���@�r����</param>
        /// <returns>���Φr����</returns>
        public static string[] StrConvWide_to_char_code(string input)
        {
            //�b������ΡG
            char[] c = input.ToCharArray();
            System.Collections.ArrayList char_code_list = new System.Collections.ArrayList();
            for (int i = 0; i < c.Length; i++)
            {
                char_code_list.Add(c[i].ToString());
            }
            return (string[])char_code_list.ToArray(typeof(string));
        }

        /// <summary>
        /// �r����b��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StrConvNarrow(string input)
        {
            try
            {
                char[] c = input.ToCharArray();
                for (int i = 0; i < c.Length; i++)
                {
                    if (c[i] == 12288)
                    {
                        c[i] = (char)32;
                        continue;
                    }
                    if (c[i] > 65280 && c[i] < 65375)
                        c[i] = (char)(c[i] - 65248);
                }
                return new string(c);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
