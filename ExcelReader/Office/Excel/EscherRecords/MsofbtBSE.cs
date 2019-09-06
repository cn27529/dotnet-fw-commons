using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class MsofbtBSE : EscherRecord
	{
		public MsofbtBSE(EscherRecord record) : base(record) { }

		public Byte BlipTypeWin32;

		public Byte BlipTypeMacOS;

		public Guid UID;

		public UInt16 Tag;

		public Int32 Size;

		public Int32 Ref;

		public Int32 Offset ;

		public Byte Usage;

		public Byte NameLength;

		public Byte Unused2;

		public Byte Unused3;

		public Byte[] ExtraData;

		public void decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.BlipTypeWin32 = reader.ReadByte();
			this.BlipTypeMacOS = reader.ReadByte();
			this.UID = new Guid(reader.ReadBytes(16));
			this.Tag = reader.ReadUInt16();
			this.Size = reader.ReadInt32();
			this.Ref = reader.ReadInt32();
			this.Offset  = reader.ReadInt32();
			this.Usage = reader.ReadByte();
			this.NameLength = reader.ReadByte();
			this.Unused2 = reader.ReadByte();
			this.Unused3 = reader.ReadByte();
			this.ExtraData = reader.ReadBytes((int)(stream.Length - stream.Position));
		}

	}
}
