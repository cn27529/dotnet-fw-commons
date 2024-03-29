﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class CommonObjectData : SubRecord
	{
		public CommonObjectData(SubRecord record) : base(record) { }

		public UInt16 ObjectType;

		public UInt16 ObjectID;

		public UInt16 OptionFlags;

		public UInt32 Reserved1;

		public UInt32 Reserved2;

		public UInt32 Reserved3;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.ObjectType = reader.ReadUInt16();
			this.ObjectID = reader.ReadUInt16();
			this.OptionFlags = reader.ReadUInt16();
			this.Reserved1 = reader.ReadUInt32();
			this.Reserved2 = reader.ReadUInt32();
			this.Reserved3 = reader.ReadUInt32();
		}

	}
}
