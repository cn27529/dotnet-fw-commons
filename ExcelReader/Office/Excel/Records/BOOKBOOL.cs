using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class BOOKBOOL : Record
	{
		public BOOKBOOL(Record record) : base(record) { }

		/// <summary>
		/// 0 = Save external linked values; 1 = Do not save external linked values
		/// </summary>
		public UInt16 NotSaveExternalLinkedValues;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.NotSaveExternalLinkedValues = reader.ReadUInt16();
		}

	}
}
