using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace QiHe.Office.Excel
{
    public partial class SST : Record
    {
        public override void Decode()
        {
            MemoryStream stream = new MemoryStream(Data);
            BinaryReader reader = new BinaryReader(stream);
            TotalOccurance = reader.ReadInt32();
            NumStrings = reader.ReadInt32();
            StringList = new List<string>(NumStrings);
            BinaryReader continuedReader = reader;
            for (int i = 0; i < NumStrings; i++)
            {
                StringList.Add(this.ReadString(continuedReader, 16, out continuedReader));
            }
        }

    }
}
