using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class MsofbtDgg : EscherRecord
	{
		public MsofbtDgg(EscherRecord record) : base(record) { }

		public Int32 MaxShapeID;

		public Int32 NumIDClusters;

		public Int32 NumSavedShapes;

		public Int32 NumSavedDrawings;

		public List<Int64> IDClusters;

		public void decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.MaxShapeID = reader.ReadInt32();
			this.NumIDClusters = reader.ReadInt32();
			this.NumSavedShapes = reader.ReadInt32();
			this.NumSavedDrawings = reader.ReadInt32();
			reader.ReadInt64();
		}

	}
}
