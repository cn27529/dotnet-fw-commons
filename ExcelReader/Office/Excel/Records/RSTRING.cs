using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class RSTRING : Record
	{
		public RSTRING(Record record) : base(record) { }

		/// <summary>
		/// List of rt formatting runs
		/// </summary>
		public UInt32 FormattingRuns;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.FormattingRuns = reader.ReadUInt32();
		}

	}
}
