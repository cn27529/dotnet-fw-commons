using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class STRING : Record
	{
		public STRING(Record record) : base(record) { }

		/// <summary>
		/// Non-empty Unicode string, 16-bit string length
		/// </summary>
		public String Value;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.Value = this.ReadString(reader,16);
		}

	}
}
