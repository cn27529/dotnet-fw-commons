using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class CALCMODE : Record
	{
		public CALCMODE(Record record) : base(record) { }

		/// <summary>
		/// whether to calculate formulas manually,automatically or automatically except for multiple table operations.
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
