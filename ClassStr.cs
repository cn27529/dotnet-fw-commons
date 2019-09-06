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
                    // '若遇到 DoubleByte(中文字)，則entercnt(字數)加1 
                    if (Microsoft.VisualBasic.Strings.Asc(Microsoft.VisualBasic.Strings.Mid(vstr, LenCount, 1)) < 0)
                        entercnt = entercnt + 1;
                    // '若字數超過最大值則離開 
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
                // '若遇到 DoubleByte(中文字)，則entercnt(字數)加1 
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
        /// 多個文字做字串拆解
        /// </summary>
        /// <param name="input">待拆解的字串</param>
        /// <param name="separator">多個文字 new string[] { "文字1", "文字2" ... }</param>
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
        /// 字串轉全形
        /// </summary>
        /// <param name="input">任一字元串</param>
        /// <returns>全形字元串</returns>
        public static string StrConvWide(string input)
        {
            //半形轉全形：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                //全形空格為12288，半形空格為32
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                //&#55422;&#56357;
                //其他字元半形(33-126)與全形(65281-65374)的對應關係是：均相差65248
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 字串轉全形
        /// </summary>
        /// <param name="input">任一字元串</param>
        /// <returns>全形字元串</returns>
        public static string[] StrConvWide_to_char_code(string input)
        {
            //半形轉全形：
            char[] c = input.ToCharArray();
            System.Collections.ArrayList char_code_list = new System.Collections.ArrayList();
            for (int i = 0; i < c.Length; i++)
            {
                char_code_list.Add(c[i].ToString());
            }
            return (string[])char_code_list.ToArray(typeof(string));
        }

        /// <summary>
        /// 字串轉半形
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
