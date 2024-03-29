﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class ROW : Record
	{
		public ROW(Record record) : base(record) { }

		/// <summary>
		/// Index of this row
		/// </summary>
		public UInt16 RowIndex;

		/// <summary>
		/// Index to column of the first cell which is described by a cell record
		/// </summary>
		public UInt16 FirstColIndex;

		/// <summary>
		/// Index to column of the last cell which is described by a cell record, increased by 1
		/// </summary>
		public Int16 LastColIndex;

		public UInt16 RowHeight;

		public UInt16 UnUsed;

		public UInt16 UnUsed2;

		/// <summary>
		/// Option flags and default row formatting
		/// </summary>
		public UInt32 Flags;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.RowIndex = reader.ReadUInt16();
			this.FirstColIndex = reader.ReadUInt16();
			this.LastColIndex = reader.ReadInt16();
			this.RowHeight = reader.ReadUInt16();
			this.UnUsed = reader.ReadUInt16();
			this.UnUsed2 = reader.ReadUInt16();
			this.Flags = reader.ReadUInt32();
		}

	}
}
