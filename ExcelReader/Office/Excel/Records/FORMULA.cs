﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class FORMULA : Record
	{
		public FORMULA(Record record) : base(record) { }

		public UInt16 RowIndex;

		public UInt16 ColIndex;

		public UInt16 XFIndex;

		/// <summary>
		/// Result of the formula.
		/// </summary>
		public UInt64 Result;

		public UInt16 OptionFlags;

		public UInt32 Unused;

		/// <summary>
		/// Formula data (RPN token array)
		/// </summary>
		public Byte[] FormulaData;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.RowIndex = reader.ReadUInt16();
			this.ColIndex = reader.ReadUInt16();
			this.XFIndex = reader.ReadUInt16();
			this.Result = reader.ReadUInt64();
			this.OptionFlags = reader.ReadUInt16();
			this.Unused = reader.ReadUInt32();
			this.FormulaData = reader.ReadBytes((int)(stream.Length - stream.Position));
		}

	}
}
