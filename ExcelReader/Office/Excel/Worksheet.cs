using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QiHe.CodeLib;

namespace QiHe.Office.Excel
{
    public class Worksheet
    {
        public List<Record> Records = new List<Record>();

        public Workbook Book;

        public string Name;
        public SheetType SheetType;
        public MSODRAWING Drawing;

        public static Worksheet Read(Stream stream)
        {
            Record record = Record.Read(stream);
            Record last_record = record;
            last_record.Decode();
            if (record is BOF && ((BOF)record).StreamType == StreamType.Worksheet)
            {
                Worksheet sheet = new Worksheet();
                while (record.Type != RecordType.EOF)
                {
                    if (record.Type == RecordType.CONTINUE)
                    {
                        last_record.ContinuedRecords.Add(record);
                    }
                    else
                    {
                        switch (record.Type)
                        {
                            case RecordType.STRING:
                                if (last_record is FORMULA)
                                {
                                    record.Decode();
                                    (last_record as FORMULA).StringRecord = record as STRING;
                                }
                                break;
                            case RecordType.MSODRAWING:
                                if (sheet.Drawing == null)
                                {
                                    sheet.Drawing = record as MSODRAWING;
                                    sheet.Records.Add(record);
                                }
                                else
                                {
                                    sheet.Drawing.ContinuedRecords.Add(record);
                                }
                                break;
                            default:
                                sheet.Records.Add(record);
                                break;
                        }
                        last_record = record;
                    }
                    record = Record.Read(stream);
                }
                return sheet;
            }
            else
            {
                return null;
            }
        }

        public CellCollection Cells = new CellCollection();

        public int MaxRowIndex;
        public int MaxColIndex;

        public void PopulateCells()
        {
            foreach (Record record in Records)
            {
                record.Decode();
                switch (record.Type)
                {
                    case RecordType.BOOLERR:
                        BOOLERR boolerr = record as BOOLERR;
                        CreateCell(boolerr.RowIndex, boolerr.ColIndex, boolerr.GetValue(), boolerr.XFIndex);
                        break;
                    case RecordType.LABELSST:
                        LABELSST label = record as LABELSST;
                        CreateCell(label.RowIndex, label.ColIndex, GetStringFromSST(label.SSTIndex), label.XFIndex);
                        break;
                    case RecordType.NUMBER:
                        NUMBER number = record as NUMBER;
                        CreateCell(number.RowIndex, number.ColIndex, number.Value, number.XFIndex);
                        break;
                    case RecordType.RK:
                        RK rk = record as RK;
                        CreateCell(rk.RowIndex, rk.ColIndex, Record.DecodeRK(rk.Value), rk.XFIndex);
                        break;
                    case RecordType.MULRK:
                        MULRK mulrk = record as MULRK;
                        int row = mulrk.RowIndex;
                        for (int col = mulrk.FirstColIndex; col <= mulrk.LastColIndex; col++)
                        {
                            int index = col - mulrk.FirstColIndex;
                            object value = Record.DecodeRK(mulrk.RKList[index]);
                            int XFindex = mulrk.XFList[index];
                            CreateCell(row, col, value, XFindex);
                        }
                        break;
                    case RecordType.FORMULA:
                        FORMULA formula = record as FORMULA;
                        CreateCell(formula.RowIndex, formula.ColIndex, formula.DecodeResult(), formula.XFIndex);
                        break;
                }
            }
        }

        void CreateCell(int row, int col, object value, int XFindex)
        {
            MaxRowIndex = Math.Max(MaxRowIndex, row);
            MaxColIndex = Math.Max(MaxColIndex, col);
            Cell cell = new Cell(value, XFindex);
            cell.Sheet = this;
            Cells[row, col] = cell;
        }

        XF GetCellXF(int row, int col)
        {
            int XFindex = 15;
            Cell cell = Cells[row, col];
            if (cell != null)
            {
                XFindex = cell.XFIndex;
            }
            return Book.ExtendedFormats[XFindex];
        }

        string GetStringFromSST(int index)
        {
            if (Book != null && Book.SharedStringTable != null)
            {
                return Book.SharedStringTable.StringList[index];
            }
            return null;
        }

        bool extracted = false;
        Dictionary<Pair<int, int>, Picture> images;
        public Dictionary<Pair<int, int>, Picture> Pictures
        {
            get
            {
                if (!extracted)
                {
                    images = ExtractPictures();
                    extracted = true;
                }
                return images;
            }
        }

        public Picture ExtractPicture(int row, int col)
        {
            Pair<int, int> pos = new Pair<int, int>(row, col);
            if (Pictures.ContainsKey(pos))
            {
                return Pictures[pos];
            }
            else
            {
                return null;
            }
        }

        public Dictionary<Pair<int, int>, Picture> ExtractPictures()
        {
            Dictionary<Pair<int, int>, Picture> images = new Dictionary<Pair<int, int>, Picture>();
           
            if (Drawing != null)
            {
                MsofbtDgContainer dgContainer = Drawing.FindChild<MsofbtDgContainer>();
                if (dgContainer != null)
                {
                    MsofbtSpgrContainer spgrContainer = dgContainer.FindChild<MsofbtSpgrContainer>();

                    List<MsofbtSpContainer> spContainers = spgrContainer.FindChildren<MsofbtSpContainer>();

                    foreach (MsofbtSpContainer spContainer in spContainers)
                    {
                        MsofbtOPT opt = spContainer.FindChild<MsofbtOPT>();
                        MsofbtClientAnchor anchor = spContainer.FindChild<MsofbtClientAnchor>();

                        if (opt != null && anchor != null)
                        {
                            foreach (ShapeProperty prop in opt.Properties)
                            {
                                if (prop.PropertyID == PropertyIDs.BlipId)
                                {
                                    int imageIndex = (int)prop.PropertyValue - 1;

                                    Pair<int, int> cell = new Pair<int, int>(anchor.Row1, anchor.Col1);

                                    Picture pic = new Picture();
                                    pic.UpperRow = anchor.Row1;
                                    pic.BottomRow = anchor.Row2;
                                    pic.LeftCol = anchor.Col1;
                                    pic.RightCol = anchor.Col2;
                                    pic.ImageData = Book.ExtractImage(imageIndex, out pic.ImageFormat);
                                    images[cell] = pic;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return images;
        }
    }
}
