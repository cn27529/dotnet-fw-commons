using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using QiHe.Office.CompoundDocumentFormat;

namespace QiHe.Office.Excel
{
    public class Workbook
    {
        public List<Record> Records = new List<Record>();

        public List<Worksheet> Worksheets = new List<Worksheet>();

        List<BOUNDSHEET> BoundSheets = new List<BOUNDSHEET>();

        public SST SharedStringTable;

        public MSODRAWINGGROUP DrawingGroup;

        public DateTime BaseDate;

        public ColorPalette ColorPalette = new ColorPalette();

        public List<XF> ExtendedFormats = new List<XF>();

        /// <summary>
        /// Open workbook from a file stream.
        /// </summary>
        /// <param name="file"></param>
        public void Open(Stream file)
        {
            CompoundDocument doc = CompoundDocument.Read(file);
            if (doc == null) throw new Exception("Invalid Excel file");
            byte[] bookdata = doc.GetStreamData("Workbook");
            this.Read(new MemoryStream(bookdata));
        }

        public void Read(Stream stream)
        {
            ReadRecords(stream);
            DecodeRecords();
            foreach (BOUNDSHEET boundSheet in BoundSheets)
            {
                stream.Position = boundSheet.StreamPosition;
                Worksheet sheet = Worksheet.Read(stream);
                sheet.Book = this;
                sheet.Name = boundSheet.SheetName;
                sheet.SheetType = (SheetType)boundSheet.SheetType;
                sheet.PopulateCells();
                Worksheets.Add(sheet);
            }
        }

        private void ReadRecords(Stream stream)
        {
            Record record = Record.Read(stream);
            record.Decode();
            Record last_record = record;
            if (record is BOF && ((BOF)record).StreamType == StreamType.WorkbookGlobals)
            {
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
                            case RecordType.MSODRAWINGGROUP:
                                if (DrawingGroup == null)
                                {
                                    DrawingGroup = record as MSODRAWINGGROUP;
                                    Records.Add(record);
                                }
                                else
                                {
                                    DrawingGroup.ContinuedRecords.Add(record);
                                }
                                break;
                            default:
                                Records.Add(record);
                                break;
                        }
                        last_record = record;
                    }
                    record = Record.Read(stream);
                }
            }
            else
            {
                throw new Exception("Invalid Workbook.");
            }
        }

        private void DecodeRecords()
        {
            foreach (Record record in Records)
            {
                record.Decode();
                switch (record.Type)
                {
                    case RecordType.BOUNDSHEET:
                        BoundSheets.Add(record as BOUNDSHEET);
                        break;
                    case RecordType.XF:
                        ExtendedFormats.Add(record as XF);
                        break;
                    case RecordType.SST:
                        SharedStringTable = record as SST;
                        break;
                    case RecordType.DATEMODE:
                        DATEMODE dateMode = record as DATEMODE;
                        switch (dateMode.Mode)
                        {
                            case 0:
                                BaseDate = DateTime.Parse("1899-12-31");
                                break;
                            case 1:
                                BaseDate = DateTime.Parse("1904-01-01");
                                break;
                        }
                        break;
                    case RecordType.PALETTE:
                        PALETTE palette = record as PALETTE;
                        int colorIndex = 8;
                        foreach (int color in palette.RGBColours)
                        {
                            ColorPalette[colorIndex] = Color.FromArgb(color);
                            colorIndex++;
                        }
                        break;
                }
            }
        }

        public List<byte[]> ExtractImages()
        {
            List<byte[]> Images = new List<byte[]>();
            if (DrawingGroup != null)
            {
                MsofbtDggContainer dggContainer = DrawingGroup.EscherRecords[0] as MsofbtDggContainer;
                foreach (MsofbtBSE blipStoreEntry in dggContainer.BstoreContainer.EscherRecords)
                {
                    if (blipStoreEntry.BlipRecord == null) continue;
                    Images.Add(blipStoreEntry.ImageData);
                }
            }
            return Images;
        }

        public byte[] ExtractImage(int index, out ushort type)
        {
            if (DrawingGroup != null)
            {
                MsofbtDggContainer dggContainer = DrawingGroup.EscherRecords[0] as MsofbtDggContainer;
                MsofbtBSE blipStoreEntry = dggContainer.BstoreContainer.EscherRecords[index] as MsofbtBSE;
                if (blipStoreEntry.BlipRecord != null)
                {
                    type = blipStoreEntry.BlipRecord.Type;
                    return blipStoreEntry.ImageData;
                }
            }
            type = 0;
            return null;
        }
    }
}
