using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class BOOLERR : Record
	{
		public BOOLERR(Record record) : base(record) { }

		public UInt16 RowIndex;

		public UInt16 ColIndex;

		public UInt16 XFIndex;

		/// <summary>
		/// Boolean or error value (type depends on the following byte)
		/// </summary>
		public Byte Value;

		/// <summary>
		/// 0 = Boolean value; 1 = Error code
		/// </summary>
		public Byte ValueType;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.RowIndex = reader.ReadUInt16();
			this.ColIndex = reader.ReadUInt16();
			this.XFIndex = reader.ReadUInt16();
			this.Value = reader.ReadByte();
			this.ValueType = reader.ReadByte();
		}

	}
}
