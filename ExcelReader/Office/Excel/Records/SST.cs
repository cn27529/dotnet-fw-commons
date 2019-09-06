using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class SST : Record
	{
		public SST(Record record) : base(record) { }

		/// <summary>
		/// Total number of strings in the workbook
		/// </summary>
		public Int32 TotalOccurance;

		/// <summary>
		/// Number of following strings (nm)
		/// </summary>
		public Int32 NumStrings;

		/// <summary>
		/// List of nm Unicode strings, 16-bit string length
		/// </summary>
		public List<String> StringList;

		public void decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.TotalOccurance = reader.ReadInt32();
			this.NumStrings = reader.ReadInt32();
			reader.ReadString();
		}

	}
}
