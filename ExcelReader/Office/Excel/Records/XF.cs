using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class XF : Record
	{
		public XF(Record record) : base(record) { }

		public UInt16 FontIndex;

		public UInt16 FormatIndex;

		public UInt16 CellProtection;

		public Byte Alignment;

		public Byte Rotation;

		public Byte Indent;

		public Byte Attributes;

		public UInt32 LineStyle;

		public UInt32 LineColor;

		public UInt16 Background;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.FontIndex = reader.ReadUInt16();
			this.FormatIndex = reader.ReadUInt16();
			this.CellProtection = reader.ReadUInt16();
			this.Alignment = reader.ReadByte();
			this.Rotation = reader.ReadByte();
			this.Indent = reader.ReadByte();
			this.Attributes = reader.ReadByte();
			this.LineStyle = reader.ReadUInt32();
			this.LineColor = reader.ReadUInt32();
			this.Background = reader.ReadUInt16();
		}

	}
}
