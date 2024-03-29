﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class BOTTOMMARGIN : Record
	{
		public BOTTOMMARGIN(Record record) : base(record) { }

		/// <summary>
		/// Bottom page margin in inches (IEEE 754 floating-point value, 64-bit double precision)
		/// </summary>
		public Double Value;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.Value = reader.ReadDouble();
		}

	}
}
