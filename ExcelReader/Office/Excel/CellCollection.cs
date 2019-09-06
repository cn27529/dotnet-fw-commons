using System;
using System.Collections.Generic;
using System.Text;
using QiHe.CodeLib;

namespace QiHe.Office.Excel
{
    public class CellCollection
    {
        Dictionary<Pair<int, int>, Cell> Cells = new Dictionary<Pair<int, int>, Cell>();

        public Cell this[int row, int col]
        {
            get
            {
                Pair<int, int> pos = new Pair<int, int>(row, col);
                if (Cells.ContainsKey(pos))
                {
                    return Cells[pos];
                }
                return null;
            }
            set
            {
                Cells[new Pair<int, int>(row, col)] = value;
            }
        }

        public Dictionary<Pair<int, int>, Cell>.Enumerator GetEnumerator()
        {
            return Cells.GetEnumerator();
        }
    }
}
