﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class BOOLERR : Record
	{
        public object GetValue()
        {
            if (ValueType == 0) // Boolean value
            {
                return Value == 1;
            }
            else // Error code
            {
                return ErrorCode.ErrorCodes[Value];
            }
        }
	}
}
