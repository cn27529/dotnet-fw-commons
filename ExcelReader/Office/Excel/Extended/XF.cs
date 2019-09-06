using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
    public partial class XF : Record
    {
        public int PatternColorIndex
        {
            get { return Background & 0x007F; }
        }

        public int PatternBackgroundColorIndex
        {
            get { return (Background & 0x3F80) >> 6; }
        }
    }
}
