using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Commons
{

    /// <summary>
    /// 泛型集合轉換為Dataset物件的摘要描述
    /// </summary>
    public class ListObjToDataSet
    {

        /// <summary>
        /// 泛型集合轉換為Dataset物件
        /// </summary>
        /// <param name="ResList"></param>
        /// <returns></returns>
        public static DataSet ListToDataSet(IList ResList)
        {
            DataSet RDS = new DataSet();
            DataTable TempDT = new DataTable();

            //此?遍?IList的?构并建立同?的DataTable
            System.Reflection.PropertyInfo[] p = ResList[0].GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo pi in p)
            {
                if (! TempDT.Columns.Contains(pi.Name))
                {
                    TempDT.Columns.Add(pi.Name, System.Type.GetType(pi.PropertyType.ToString()));
                }                
            }

            for (int i = 0; i < ResList.Count; i++)
            {
                IList TempList = new ArrayList();
                //?IList中的一????入ArrayList
                foreach (System.Reflection.PropertyInfo pi in p)
                {
                    object oo = pi.GetValue(ResList[i], null);
                    TempList.Add(oo);
                }

                object[] itm = new object[p.Length];
                //遍?ArrayList向object[]里放?据
                for (int j = 0; j < TempList.Count; j++)
                {
                    itm.SetValue(TempList[j], j);
                }
                //?object[]的?容放入DataTable
                TempDT.LoadDataRow(itm, true);
            }
            //?DateTable放入DataSet
            RDS.Tables.Add(TempDT);
            //返回DataSet
            return RDS;
        }

    }
}