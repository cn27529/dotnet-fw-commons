using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic;

using System.Text.RegularExpressions;

namespace Commons
{
    /// <summary>
    /// Html字串轉換
    /// </summary>
    public class ConvertHtmlCode
    {
        /// <summary>
        /// 將 TEXT 轉為可讀取的 HTML 字串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TextToHtml(string value)
        {
            string txtStr = value;
            txtStr = txtStr.Replace("\"","&quot;");
            //txtStr = txtStr.Replace(Microsoft.VisualBasic.Constants.vbTab, "&nbsp;&nbsp;&nbsp;&nbsp;");
            txtStr = txtStr.Replace(" ", "&nbsp;");
            txtStr = txtStr.Replace("<", "&lt;");
            txtStr = txtStr.Replace(">", "&gt;");
            txtStr = txtStr.Replace(Microsoft.VisualBasic.Constants.vbCrLf, "<br/>");
            txtStr = txtStr.Replace(" ", "");
            txtStr = txtStr.Replace("　", "");
            return txtStr;
        }

        /// <summary>
        /// 將 HTML 轉為可讀取的 TEXT 字串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string HtmlToText(string value)
        {
            string txtStr = value;
            txtStr = txtStr.Replace("&quot;", "\"");
            //txtStr = txtStr.Replace("&nbsp;&nbsp;&nbsp;&nbsp;", Microsoft.VisualBasic.Constants.vbTab);
            txtStr = txtStr.Replace("&nbsp;", " ");
            txtStr = txtStr.Replace("&lt;", "<");
            txtStr = txtStr.Replace("&gt;", ">");
            txtStr = txtStr.Replace("<br/>", Microsoft.VisualBasic.Constants.vbCrLf);
            return txtStr;
        }

        /// <summary>
        /// 取得字串內是否包含用戶端指令碼
        /// </summary>
        /// <param name="value">true = 具有指令碼 , false : 無指令碼語言</param>
        /// <returns></returns>
        public static bool IsScriptText(string value)
        {
            string txtStr = value;
            txtStr = txtStr.Replace(" ", "").Replace("　", "");
            if ( txtStr.IndexOf(( "<script" ).ToLower()) > 0 || txtStr.IndexOf(( "</script>" ).ToLower()) > 0 )
                return true;
            else
                return false;
        }

        public static string Remove_Html_Tags(string Html)
        {
            //http://www.learnfast.ninja/posts/53e87793e76c5c741d4f3fd9
            string Only_Text = Regex.Replace(Html, @"<(.|\n)*?>", string.Empty);

            return Only_Text;
        }

    }

}
