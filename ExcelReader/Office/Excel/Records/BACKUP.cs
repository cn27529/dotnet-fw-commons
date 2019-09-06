using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class BACKUP : Record
	{
		public BACKUP(Record record) : base(record) { }

		/// <summary>
		/// whether Excel makes a backup of the file while saving
		/// </summary>
		public UInt16 CreateBackupOnSaving;

		public override void Decode()
		{
			MemoryStream stream = new MemoryStream(Data);
			BinaryReader reader = new BinaryReader(stream);
			this.CreateBackupOnSaving = reader.ReadUInt16();
		}

	}
}
