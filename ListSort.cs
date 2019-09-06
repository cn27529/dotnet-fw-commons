using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Commons
{
    /// <summary>
    /// 泛型集合排序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListSort<T>
    {
        /// <summary>
        /// 取得泛型集合的排序
        /// </summary>
        /// <param name="oIList"></param>
        /// <param name="tSort">排序名稱</param>
        /// <param name="isDesc">是否逆向排序</param>
        /// <returns></returns>
        public static IList<T> GetList(ref IList<T> oIList, string tSort, bool isDesc)
        {
            List<T> oList = (List<T>)oIList;
            ListSort<T> oListSort = new ListSort<T>();
            oList.Sort(
                delegate(T info1, T info2)
                {
                    object obj1 = info2.GetType().GetProperty(tSort).GetValue(info2, null);
                    object obj2 = info1.GetType().GetProperty(tSort).GetValue(info1, null);
                    string type = info1.GetType().GetProperty(tSort).PropertyType.ToString();

                    if (isDesc == false)
                        return oListSort.Compare(ref obj1, ref obj2, ref type);
                    else
                        return oListSort.Compare(ref obj2, ref obj1, ref type);
                });
            return oIList;
        }

        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="obj1">類型1</param>
        /// <param name="obj2">類型2</param>
        /// <param name="type">"System.Decimal" / "System.Single" / "System.String" / "System.Int16" / "System.Int32" / "System.Int64" / "System.DateTime" / "System.Boolean"</param>
        /// <returns></returns>
        private int Compare(ref object obj1, ref object obj2, ref string type)
        {
            switch (type)
            {
                case "System.Decimal":
                    return decimal.Parse(obj1.ToString()).CompareTo(decimal.Parse(obj2.ToString()));
                    break;
                case "System.Single":
                    return Single.Parse(obj1.ToString()).CompareTo(Single.Parse(obj2.ToString()));
                    break;
                case "System.String":
                    return obj1.ToString().CompareTo(obj2.ToString());
                    break;
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                    return int.Parse(obj1.ToString()).CompareTo(int.Parse(obj2.ToString()));
                    break;
                case "System.DateTime":
                    return DateTime.Parse(obj1.ToString()).CompareTo(DateTime.Parse(obj2.ToString()));
                    break;
                case "System.Boolean":
                    return Boolean.Parse(obj1.ToString()).CompareTo(Boolean.Parse(obj2.ToString()));
                    break;
                default:
                    return 0;
                    break;
            }
        }
    }
}
