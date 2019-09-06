using System;
using System.Collections.Generic;
using System.Text;

//using Microsoft.Security;
//using Microsoft.Security.Application;

namespace Commons
{
    /// <summary>
    /// AntiXSS
    /// </summary>
    public class AntiXSS
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string HtmlAttributeEncode(string input)
        {
            return Microsoft.Security.Application.Encoder.HtmlAttributeEncode(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetSafeHtmlFragment(string input)
        {
            return Microsoft.Security.Application.Sanitizer.GetSafeHtmlFragment(input);
        }

        //public static string HtmlFormUrlEncode(string input)
        //{
        //    input = Microsoft.Security.Application.Encoder.HtmlFormUrlEncode(input);
        //    return input;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string HtmlEncode(string input)
        {
            return Microsoft.Security.Application.Encoder.HtmlEncode(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string LdapEncode(string input)
        {
            return Microsoft.Security.Application.Encoder.LdapEncode(input);
        }
    }
}
