using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Commons
{

    /// <summary>
    /// �x�����X�ഫ��Dataset���󪺺K�n�y�z
    /// </summary>
    public class ListObjToDataSet
    {

        /// <summary>
        /// �x�����X�ഫ��Dataset����
        /// </summary>
        /// <param name="ResList"></param>
        /// <returns></returns>
        public static DataSet ListToDataSet(IList ResList)
        {
            DataSet RDS = new DataSet();
            DataTable TempDT = new DataTable();

            //��?�M?IList��?�ۦ}�إߦP?��DataTable
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
                //?IList�����@????�JArrayList
                foreach (System.Reflection.PropertyInfo pi in p)
                {
                    object oo = pi.GetValue(ResList[i], null);
                    TempList.Add(oo);
                }

                object[] itm = new object[p.Length];
                //�M?ArrayList�Vobject[]����?�u
                for (int j = 0; j < TempList.Count; j++)
                {
                    itm.SetValue(TempList[j], j);
                }
                //?object[]��?�e��JDataTable
                TempDT.LoadDataRow(itm, true);
            }
            //?DateTable��JDataSet
            RDS.Tables.Add(TempDT);
            //��^DataSet
            return RDS;
        }

    }
}