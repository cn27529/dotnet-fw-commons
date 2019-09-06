using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Commons
{


    /// <summary>
    /// 主控台應用程序說明
    /// </summary>
    public class ConsoleHelp
    {
        /// <summary>
        /// 主控台參數
        /// </summary>
        private string[] _args = new string[] { };

        /// <summary>
        /// 指定Help的參數值, 默認為?號字串
        /// </summary>
        private string _helpParameter = "?";

        /// <summary>
        /// 幫助的建構子
        /// </summary>
        /// <param name="args"></param>
        public ConsoleHelp(string[] args)
        {
            this._args = args;
        }

        /// <summary>
        /// 幫助的建構子
        /// </summary>
        /// <param name="args">主控台參數</param>
        /// <param name="helpParameter">幫助的識別參數值, 默認為?號字串</param>
        public ConsoleHelp(string[] args, string helpParameter)
        {
            this._args = args;
            this._helpParameter = helpParameter;
        }

        /// <summary>
        /// 指定Help的參數值, 默認為?號字串
        /// </summary>
        /// <param name="helpParameter">指定Help的參數值, 默認為?號字串</param>
        public void setHelpParameter(string helpParameter)
        {
            this._helpParameter = helpParameter;
        }

        /// <summary>
        /// 幫助開始
        /// </summary>
        public void playHelp()
        {
            if (this._args.Length == 0) return;
            string myHelpParameter = this._args[0].ToString();

            if (this._helpParameter.Equals(myHelpParameter))
            {
                string titleLine = "-------------------- Help --------------------";
                if (this._helps.Count > 0)
                {
                    Console.WriteLine(titleLine);
                    foreach (string help in this._helps)
                        Console.WriteLine(help);
                    Console.WriteLine(titleLine);
                }
                this._next = false;
                return;
            }
            else
                this._next = true;
        }

        private bool _next = false;
        /// <summary>
        /// 取得是否需要執行
        /// </summary>
        /// <returns>true:是 , false:否</returns>
        public bool next()
        {
            return this._next;
        }

        /// <summary>
        /// 幫助的說明文字清單
        /// </summary>
        private ArrayList _helps = new ArrayList();

        /// <summary>
        /// 加入幫助的說明文字
        /// </summary>
        /// <param name="help"></param>
        public void addHelp(string help)
        {
            this._helps.Add(help);
        }

        public static void outputConsoleAndWriteLog(string tMethodName, string tMessage)
        {
            Console.WriteLine(tMessage);
            Commons.LogController.WriteLog(tMethodName, tMessage);
        }

    }
}