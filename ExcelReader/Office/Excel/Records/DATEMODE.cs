﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class DATEMODE : Record
	{
		public DATEMODE(Record record) : base(record) { }

		/// <summary>
		/// 0 = Base date is 1899-Dec-31; 1 = Base date is 1904-Jan-01
		/// </summary>
		public Int16 Mode;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.Mode = reader.ReadInt16();
		}

	}
}
