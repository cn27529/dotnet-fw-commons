using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class MsofbtDg : EscherRecord
	{
		public MsofbtDg(EscherRecord record) : base(record) { }

		public Int32 NumShapes;

		public Int32 LastShapeID;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.NumShapes = reader.ReadInt32();
			this.LastShapeID = reader.ReadInt32();
		}

	}
}
