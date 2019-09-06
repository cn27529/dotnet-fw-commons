using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class CALCCOUNT : Record
	{
		public CALCCOUNT(Record record) : base(record) { }

		/// <summary>
		/// Maximum number of iterations allowed in circular references
		/// </summary>
		public UInt16 Value;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.Value = reader.ReadUInt16();
		}

	}
}
