﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
	public partial class Record
	{
		public static Record Read(Stream stream)
		{
			Record record = Record.ReadBase(stream);
			switch (record.Type)
			{
				case RecordType.BOF:
					return new BOF(record);
				case RecordType.ARRAY:
					return new ARRAY(record);
				case RecordType.BACKUP:
					return new BACKUP(record);
				case RecordType.BLANK:
					return new BLANK(record);
				case RecordType.BOOKBOOL:
					return new BOOKBOOL(record);
				case RecordType.BOOLERR:
					return new BOOLERR(record);
				case RecordType.BOTTOMMARGIN:
					return new BOTTOMMARGIN(record);
				case RecordType.BOUNDSHEET:
					return new BOUNDSHEET(record);
				case RecordType.CALCCOUNT:
					return new CALCCOUNT(record);
				case RecordType.CALCMODE:
					return new CALCMODE(record);
				case RecordType.CODEPAGE:
					return new CODEPAGE(record);
				case RecordType.DIMENSIONS:
					return new DIMENSIONS(record);
				case RecordType.LABELSST:
					return new LABELSST(record);
				case RecordType.MULBLANK:
					return new MULBLANK(record);
				case RecordType.MULRK:
					return new MULRK(record);
				case RecordType.NUMBER:
					return new NUMBER(record);
				case RecordType.RK:
					return new RK(record);
				case RecordType.ROW:
					return new ROW(record);
				case RecordType.RSTRING:
					return new RSTRING(record);
				case RecordType.SST:
					return new SST(record);
				case RecordType.CONTINUE:
					return new CONTINUE(record);
				case RecordType.FORMULA:
					return new FORMULA(record);
				case RecordType.XF:
					return new XF(record);
				case RecordType.PALETTE:
					return new PALETTE(record);
				case RecordType.BITMAP:
					return new BITMAP(record);
				case RecordType.OBJ:
					return new OBJ(record);
				case RecordType.DATEMODE:
					return new DATEMODE(record);
				case RecordType.MSODRAWINGGROUP:
					return new MSODRAWINGGROUP(record);
				case RecordType.MSODRAWING:
					return new MSODRAWING(record);
				case RecordType.MSODRAWINGSELECTION:
					return new MSODRAWINGSELECTION(record);
				case RecordType.STRING:
					return new STRING(record);
				default:
					return record;
			}
		}

	}
}
