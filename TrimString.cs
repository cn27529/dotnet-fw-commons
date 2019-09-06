using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;

namespace Commons
{
    /// <summary>
    /// 清除字串
    /// </summary>
    public class TrimString
    {
        /// <summary>
        /// 清除空白字元
        /// </summary>
        /// <param name="tValue">字串值</param>
        /// <returns></returns>
        public static string TrimEmpty(string tValue)
        {
            if ( string.IsNullOrEmpty(tValue) ) tValue = "";
            tValue = tValue.Trim();
            tValue = tValue.Trim(' ');
            tValue = tValue.Trim('　');
            tValue = tValue.Replace(" ", "");
            tValue = tValue.Replace("　", "");
            tValue = tValue.Replace("&nbsp;", "");
            return tValue;
        }

        public static string TrimBrBn(string tValue)
        {
            tValue = TrimString.TrimEmpty(tValue);
            tValue = tValue.Replace("\n", "");
            tValue = tValue.Replace("\r", "");
            return tValue;
        }



        public static string Trim_Html_Tags(string Html)
        {
            //http://www.learnfast.ninja/posts/53e87793e76c5c741d4f3fd9
            string Only_Text = Regex.Replace(Html, @"<(.|\n)*?>", string.Empty);

            return Only_Text;
        }
       

    }
}
