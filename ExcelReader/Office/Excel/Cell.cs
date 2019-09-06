using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace QiHe.Office.Excel
{
    public class Cell
    {
        public object Value;

        internal int XFIndex;

        internal Worksheet Sheet;

        public Cell(object value, int xfindex)
        {
            Value = value;
            XFIndex = xfindex;
        }

        public string StringValue
        {
            get { return Value.ToString(); }
        }

        public DateTime DateTimeValue
        {
            get
            {
                if (Value is double)
                {
                    double days = (double)Value;
                    if (days > 366) days--;
                    return Sheet.Book.BaseDate.AddDays(days);
                }
                else if (Value is string)
                {
                    return DateTime.Parse((string)Value);
                }
                else
                {
                    throw new Exception("Invalid DateTime Cell.");
                }
            }
        }

        public int BackColorIndex
        {
            get
            {
                return Sheet.Book.ExtendedFormats[XFIndex].PatternColorIndex;
            }
        }

        public Color BackColor
        {
            get
            {
                return Sheet.Book.ColorPalette[BackColorIndex];
            }
        }
    }
}
