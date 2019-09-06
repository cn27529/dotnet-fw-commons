using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class CODEPAGE : Record
	{
		public CODEPAGE(Record record) : base(record) { }

		/// <summary>
		/// text encoding used to write byte strings
		/// </summary>
		public UInt16 CodePageIdentifier;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.CodePageIdentifier = reader.ReadUInt16();
		}

	}
}
