using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class BOF : Record
	{
		public BOF(Record record) : base(record) { }

		public UInt16 BIFFversion;

		public UInt16 StreamType;

		public UInt16 BuildID;

		public UInt16 BuildYear;

		public UInt32 FileHistoryFlags;

		/// <summary>
		/// Lowest Excel version that can read all records in this file
		/// </summary>
		public UInt32 RequiredExcelVersion;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.BIFFversion = reader.ReadUInt16();
			this.StreamType = reader.ReadUInt16();
			this.BuildID = reader.ReadUInt16();
			this.BuildYear = reader.ReadUInt16();
			this.FileHistoryFlags = reader.ReadUInt32();
			this.RequiredExcelVersion = reader.ReadUInt32();
		}

	}
}
