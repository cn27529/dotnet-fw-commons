using System;
using System.Data;
using System.Configuration;
using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using System.IO;
using QiHe.Office.Excel;
using System.Text;


using System.Collections;
using System.Collections.Generic;

using NPOI.HSSF.UserModel;


namespace Commons
{
    /// <summary>
    /// Excel檔案讀取器 
    /// </summary>
    public static class ExcelConverter
    {
        /// <summary>
        /// 將資料寫入試算表檔案
        /// </summary>
        /// <param name="dt">存入的資料</param>
        /// <param name="realFilePath">實體檔案來源</param>
        /// <returns></returns>
        public static bool CreateFile(DataTable dt, string realFilePath)
        {
            bool result = false;
            //先算出欄位及列數   
            int rows = dt.Rows.Count;
            int cols = dt.Columns.Count;

            if (cols == 0 && rows == 0) return false;

            //用來建立命令    
            StringBuilder sb = new StringBuilder();
            //建立連線字串
            String connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + realFilePath + ";Extended Properties=Excel 8.0;";

            try
            {
                sb.Append("CREATE TABLE ");
                sb.Append(dt.TableName + " ( ");
                //用來做開TABLE的欄名資訊   
                for (int i = 0; i < cols; i++)
                {
                    if (i < cols - 1)
                        sb.Append(string.Format("{0} varchar,", dt.Columns[i].ColumnName));
                    else
                        sb.Append(string.Format("{0} varchar)", dt.Columns[i].ColumnName));
                }
                //把要開啟的臨時Excel建立起來   
                using (OleDbConnection objConn = new OleDbConnection(connString))
                {
                    OleDbCommand objCmd = new OleDbCommand();
                    objCmd.Connection = objConn;
                    objCmd.CommandText = sb.ToString();
                    objConn.Open();
                    //先執行CreateTable的任務   
                    objCmd.ExecuteNonQuery();

                    //開始處理資料內容的新增  
                    #region 開始處理資料內容的新增
                    //把之前 CreateTable 清空   
                    sb.Remove(0, sb.Length);
                    sb.Append("INSERT INTO ");
                    sb.Append(dt.TableName + " ( ");
                    //這邊開始組該Excel欄位順序   
                    for (int i = 0; i < cols; i++)
                    {
                        if (i < cols - 1)
                            sb.Append(dt.Columns[i].ColumnName + ",");
                        else
                            sb.Append(dt.Columns[i].ColumnName + ") values (");
                    }
                    //這邊組 DataTable裡面的值要給到Excel欄位的   
                    for (int i = 0; i < cols; i++)
                    {
                        if (i < cols - 1)
                            sb.Append("@" + dt.Columns[i].ColumnName + ",");
                        else
                            sb.Append("@" + dt.Columns[i].ColumnName + ")");
                    }
                    #endregion

                    //建立插入動作的Command   
                    objCmd.CommandText = sb.ToString();
                    OleDbParameterCollection param = objCmd.Parameters;

                    for (int i = 0; i < cols; i++) param.Add(new OleDbParameter("@" + dt.Columns[i].ColumnName, OleDbType.VarChar));

                    //使用參數化的方式來給予值   
                    foreach (DataRow row in dt.Rows)
                    {
                        for (int i = 0; i < param.Count; i++) param[i].Value = row[i];
                        //執行這一筆的給值   
                        objCmd.ExecuteNonQuery();
                    }
                    objConn.Close();
                }//end using  

                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }


        /// <summary>
        /// ReadExcelfile to dataset
        /// </summary>
        /// <param name="sheetName">工作表名稱</param>
        /// <param name="realFilePath">實體檔案來源</param>
        /// <param name="ds">資料容器</param>
        /// <returns></returns>
        public static bool ReadExcelfile(string sheetName, string realFilePath, out DataSet ds)
        {
            bool result = false;
            string fileName = realFilePath;
            string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";Data Source=" + fileName;

            //請將連線EXCEL文件的連線字串寫成這樣就可以解決:
            //Provider=Microsoft.Jet.OLEDB.4.0;Data Source=EXCEL文件檔名;Extended Properties='Excel 8.0;HDR=YES;IMEX=1 

            OleDbConnection conn = new OleDbConnection(connStr);
            OleDbCommand cmA = new OleDbCommand();
            //OleDbDataReader drA = default(OleDbDataReader);
            //string Sql = "SELECT * FROM [批次上架資料$] WHERE [標題]<>'' AND [檔名]<>''";
            string Sql = string.Format("SELECT * FROM [{0}$] ", sheetName);
            ds = new DataSet();
            try
            {
                cmA = new OleDbCommand(Sql, conn);
                cmA.Connection.Open();
                //drA = cmA.ExecuteReader();
                OleDbDataAdapter ad = new OleDbDataAdapter(cmA);
                ad.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0) result = true;
                //drA.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmA.Connection.Close();
                conn.Close();
                cmA.Dispose();
            }

            return result;
        }

        /// <summary>
        /// ReadExcelfile to dataset
        /// </summary>
        /// <param name="realFilePath">實體檔案來源</param>
        /// <param name="ds">資料容器</param>
        /// <returns></returns>
        public static bool ReadExcelfile(string realFilePath, out DataSet ds)
        {
            //bool result = false;
            //string tMethodName = "ExcelConverter.ReadExcelfile";
            //ds = new DataSet();
            //try
            //{
            //    realFilePath = realFilePath.Trim();
            //    System.IO.FileStream fs = new System.IO.FileStream(realFilePath, System.IO.FileMode.Open);
            //    ExcelDataReader.ExcelDataReader rd = new ExcelDataReader.ExcelDataReader(fs);
            //    fs.Close();
            //    ds = new DataSet();
            //    ds = rd.WorkbookData;
            //    if (ds.Tables.Count > 0) result = true;
            //}
            //catch (Exception ex)
            //{
            //    string tMessage = ex.Message;
            //    Console.WriteLine(tMessage);
            //    Commons.LogController.WriteLog(tMethodName, tMessage);
            //}
            //return result;

            string sheetName = "Sheet1";

            bool result = false;
            string fileName = realFilePath;
            string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";Data Source=" + fileName;

            //請將連線EXCEL文件的連線字串寫成這樣就可以解決:
            //Provider=Microsoft.Jet.OLEDB.4.0;Data Source=EXCEL文件檔名;Extended Properties='Excel 8.0;HDR=YES;IMEX=1 

            OleDbConnection conn = new OleDbConnection(connStr);
            OleDbCommand cmA = new OleDbCommand();
            //OleDbDataReader drA = default(OleDbDataReader);
            //string Sql = "SELECT * FROM [批次上架資料$] WHERE [標題]<>'' AND [檔名]<>''";
            string Sql = string.Format("SELECT * FROM [{0}$] ", sheetName);
            ds = new DataSet();
            try
            {
                cmA = new OleDbCommand(Sql, conn);
                cmA.Connection.Open();
                //drA = cmA.ExecuteReader();
                OleDbDataAdapter ad = new OleDbDataAdapter(cmA);
                ad.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0) result = true;
                //drA.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmA.Connection.Close();
                conn.Close();
                cmA.Dispose();
            }

            return result;

        }

        /// <summary>
        /// QiHe.Office.Excel.Workbook book
        /// </summary>
        /// <param name="realFilePath">實體檔案來源</param>
        /// <param name="book">Workbook</param>
        /// <returns></returns>
        public static bool ReadExcelfile(string realFilePath, out QiHe.Office.Excel.Workbook book)
        {
            bool result = false;
            string tMethodName = "ExcelConverter.ReadExcelfile";
            book = new Workbook();
            try
            {
                System.IO.Stream fileStream = System.IO.File.OpenRead(realFilePath);
                book = new QiHe.Office.Excel.Workbook();
                book.Open(fileStream);
                result = true;
            }
            catch (Exception ex)
            {
                string tMessage = ex.Message;
                Console.WriteLine(tMessage);
                Commons.LogController.WriteLog(tMethodName, tMessage);
            }
            return result;

        }


        /// <summary>
        /// ReadExcelfile to dataset
        /// </summary>
        /// <param name="realFilePath">實體檔案來源</param>
        /// <param name="ds">資料容器</param>
        /// <returns></returns>
        public static bool ReadExcelfile(string realFilePath, out string[] string_array)
        {
            bool result = false;
            string tMethodName = "ExcelConverter.ReadExcelfile";
            string_array = new string[] { };

            ArrayList value_list = new ArrayList();

            try
            {
                //realFilePath = realFilePath.Trim();
                //System.IO.FileStream fs = new System.IO.FileStream(realFilePath, System.IO.FileMode.Open);
                //ExcelDataReader.ExcelDataReader rd = new ExcelDataReader.ExcelDataReader(fs);
                //fs.Close();
                //ds = new DataSet();
                //ds = rd.WorkbookData;
                //if (ds.Tables.Count > 0) result = true;

                HSSFWorkbook workbook;
                //讀取專案內中的sample.xls 的excel 檔案
                using (FileStream file = new FileStream(realFilePath, FileMode.Open, System.IO.FileAccess.Read))
                {
                    workbook = new HSSFWorkbook(file);
                }

                var sheet = workbook.GetSheet("Sheet1");

                if (sheet == null)
                    sheet = workbook.GetSheet("sheet1");

                if (sheet == null)
                    sheet = workbook.GetSheet("工作表1");

                if (sheet.LastRowNum > 0) string_array = new string[sheet.LastRowNum];

                for (int excel_row = 0; excel_row <= sheet.LastRowNum; excel_row++)
                {
                    try
                    {
                        //第N筆ROW
                        if (sheet.GetRow(excel_row) != null) //null is when the row only contains empty cells 
                        {
                            

                        }

                        string item_values = string.Empty;

                        foreach (var excel_cell in sheet.GetRow(excel_row).Cells)
                        {
                            //每個cells加分隔號
                            //string_array[row] += TrimString.TrimEmpty(c.StringCellValue.ToString()) + ",";

                            string excel_value = (string)excel_cell.StringCellValue;

                            //excel_value = TrimString.TrimEmpty(excel_value);

                            ////如果是數字型 就要取 NumericCellValue  這屬性的值
                            //if (c.CellType == NPOI.SS.UserModel.CellType.NUMERIC)
                            //{
                            //    //Response.Write(c.NumericCellValue.ToString("#"));
                            //}

                            ////如果是字串型 就要取 StringCellValue  這屬性的值
                            //if (c.CellType == NPOI.SS.UserModel.CellType.STRING)
                            //{
                            //    //Response.Write(c.StringCellValue);
                            //}

                            //每個欄中間加上逗點
                            //Response.Write(",");
                            item_values += excel_value + ",";

                        }
                        //每一行補上換行
                        //Response.Write("<br />");

                        //去除最後一個逗點號
                        //string_array[row] = string_array[row].Substring(0, string_array[row].Length - 1);
                        if (item_values.Length > 0)
                            item_values = item_values.Substring(0, item_values.Length - 1);

                        value_list.Add(item_values);

                    }
                    catch (Exception ex)
                    {
                        string tMessage = ex.Message;
                        Console.WriteLine(tMessage);
                        Commons.LogController.WriteLog(tMethodName, tMessage);

                        //下一筆
                        continue;
                    }




                }

                // Copies the elements of the ArrayList to a string array.
                string_array = (String[])value_list.ToArray(typeof(string));

            }
            catch (Exception ex)
            {
                string tMessage = ex.Message;
                Console.WriteLine(tMessage);
                Commons.LogController.WriteLog(tMethodName, tMessage);
            }
            return result;
        }
    }
}

