using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class PALETTE : Record
	{
		public PALETTE(Record record) : base(record) { }

		/// <summary>
		/// Number of following colours (nm).
		/// </summary>
		public UInt16 NumColors;

		/// <summary>
		/// List of nm RGB colours
		/// </summary>
		public List<Int32> RGBColours;

		public void decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.NumColors = reader.ReadUInt16();
			reader.ReadInt32();
		}

	}
}
