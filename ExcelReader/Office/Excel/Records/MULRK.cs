using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class MULRK : Record
	{
		public MULRK(Record record) : base(record) { }

		/// <summary>
		/// Index to row
		/// </summary>
		public UInt16 RowIndex;

		/// <summary>
		/// Index to first column (fc)
		/// </summary>
		public UInt16 FirstColIndex;

		/// <summary>
		/// List of nc=lc-fc+1 XF/RK structures.
		/// </summary>
		public List<UInt32> XFRKList;

		/// <summary>
		/// Index to last column (lc)
		/// </summary>
		public Int16 LastColIndex;

		public void decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.RowIndex = reader.ReadUInt16();
			this.FirstColIndex = reader.ReadUInt16();
			reader.ReadUInt32();
			this.LastColIndex = reader.ReadInt16();
		}

	}
}
