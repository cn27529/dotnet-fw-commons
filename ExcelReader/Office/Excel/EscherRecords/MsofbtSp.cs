using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class MsofbtSp : EscherRecord
	{
		public MsofbtSp(EscherRecord record) : base(record) { }

		public Int32 ShapeId;

		public Int32 Flags;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.ShapeId = reader.ReadInt32();
			this.Flags = reader.ReadInt32();
		}

	}
}
