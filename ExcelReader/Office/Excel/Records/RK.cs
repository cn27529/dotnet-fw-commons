using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class RK : Record
	{
		public RK(Record record) : base(record) { }

		public UInt16 RowIndex;

		public UInt16 ColIndex;

		public UInt16 XFIndex;

		/// <summary>
		/// RK value
		/// </summary>
		public UInt32 Value;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.RowIndex = reader.ReadUInt16();
			this.ColIndex = reader.ReadUInt16();
			this.XFIndex = reader.ReadUInt16();
			this.Value = reader.ReadUInt32();
		}

	}
}
