using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class MULBLANK : Record
	{
		public MULBLANK(Record record) : base(record) { }

		/// <summary>
		/// Index to row
		/// </summary>
		public UInt16 RowIndex;

		/// <summary>
		/// Index to first column (fc)
		/// </summary>
		public UInt16 FirstColIndex;

		/// <summary>
		/// List of nc=lc-fc+1 16-bit indexes to XF records
		/// </summary>
		public UInt16 XFIndice;

		/// <summary>
		/// Index to last column (lc)
		/// </summary>
		public Int16 LastColIndex;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.RowIndex = reader.ReadUInt16();
			this.FirstColIndex = reader.ReadUInt16();
			this.XFIndice = reader.ReadUInt16();
			this.LastColIndex = reader.ReadInt16();
		}

	}
}
