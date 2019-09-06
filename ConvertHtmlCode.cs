using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic;

using System.Text.RegularExpressions;

namespace Commons
{
    /// <summary>
    /// Html�r���ഫ
    /// </summary>
    public class ConvertHtmlCode
    {
        /// <summary>
        /// �N TEXT �ର�iŪ���� HTML �r��
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
            txtStr = txtStr.Replace("�@", "");
            return txtStr;
        }

        /// <summary>
        /// �N HTML �ର�iŪ���� TEXT �r��
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
        /// ���o�r�ꤺ�O�_�]�t�Τ�ݫ��O�X
        /// </summary>
        /// <param name="value">true = �㦳���O�X , false : �L���O�X�y��</param>
        /// <returns></returns>
        public static bool IsScriptText(string value)
        {
            string txtStr = value;
            txtStr = txtStr.Replace(" ", "").Replace("�@", "");
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
