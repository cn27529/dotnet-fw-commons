using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QiHe.CodeLib;

namespace QiHe.Office.CompoundDocumentFormat
{
    public class CompoundDocument
    {
        private BinaryReader Reader;
        private FileHeader Header;
        private int SectorSize;
        private int ShortSectorSize;
        private List<Int32> MasterSectorAllocationTable;
        private List<Int32> SectorAllocationTable;
        private List<Int32> ShortSectorAllocationTable;

        private Stream ShortStreamContainer;
        private Stream DirectoryStream;
        private Dictionary<int, DirectoryEntry> DirectoryEntries;

        public DirectoryEntry RootStorage
        {
            get { return DirectoryEntries[0]; }
        }

        public static CompoundDocument Read(string file)
        {
            using (FileStream stream = File.OpenRead(file))
            {
                return Read(stream);
            }
        }

        public static CompoundDocument Read(Stream stream)
        {
            CompoundDocument doc = new CompoundDocument();
            doc.Reader = new BinaryReader(stream);

            doc.Header = ReadHeader(doc.Reader);
            if (!doc.CheckHeader()) return null;

            doc.BuildFileStructor();

            return doc;
        }

        private static FileHeader ReadHeader(BinaryReader reader)
        {
            FileHeader header = new FileHeader();
            header.FileTypeIdentifier = reader.ReadBytes(8);
            header.FileIdentifier = new Guid(reader.ReadBytes(16));
            header.RevisionNumber = reader.ReadUInt16();
            header.VersionNumber = reader.ReadUInt16();
            header.ByteOrderMark = reader.ReadBytes(2);
            header.SectorSizeInPot = reader.ReadUInt16();
            header.ShortSectorSizeInPot = reader.ReadUInt16();
            header.UnUsed10 = reader.ReadBytes(10);
            header.NumberOfSATSectors = reader.ReadInt32();
            header.FirstSectorIDofDirectoryStream = reader.ReadInt32();
            header.UnUsed4 = reader.ReadBytes(4);
            header.MinimumStreamSize = reader.ReadInt32();
            header.FirstSectorIDofShortSectorAllocationTable = reader.ReadInt32();
            header.NumberOfShortSectors = reader.ReadInt32();
            header.FirstSectorIDofMasterSectorAllocationTable = reader.ReadInt32();
            header.NumberOfMasterSectors = reader.ReadInt32();
            header.MasterSectorAllocationTable = ReadArrayOfInt32(reader, 109);
            return header;
        }

        private static DirectoryEntry ReadDirectoryEntry(BinaryReader reader)
        {
            DirectoryEntry entry = new DirectoryEntry();
            entry.NameBuffer = reader.ReadChars(32);
            entry.NameDataSize = reader.ReadUInt16();
            entry.EntryType = reader.ReadByte();
            entry.NodeColor = reader.ReadByte();
            entry.LeftChildDID = reader.ReadInt32();
            entry.RightChildDID = reader.ReadInt32();
            entry.MembersTreeNodeDID = reader.ReadInt32();
            entry.UniqueIdentifier = new Guid(reader.ReadBytes(16));
            entry.UserFlags = reader.ReadInt32();
            entry.CreationTime = DateTime.FromFileTime(reader.ReadInt64());
            entry.LastModificationTime = DateTime.FromFileTime(reader.ReadInt64());
            entry.FirstSectorID = reader.ReadInt32();
            entry.StreamLength = reader.ReadInt32();
            entry.UnUsed = reader.ReadInt32();
            return entry;
        }

        static Int32[] ReadArrayOfInt32(BinaryReader reader, int count)
        {
            Int32[] data = new Int32[count];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = reader.ReadInt32();
            }
            return data;
        }

        static bool ArrayEqual(byte[] bytes1, byte[] bytes2)
        {
            if (bytes1.Length != bytes2.Length) return false;
            for (int i = 0; i < bytes1.Length; i++)
            {
                if (bytes1[i] != bytes2[i]) return false;
            }
            return true;
        }

        bool CheckHeader()
        {
            if (!ArrayEqual(Header.FileTypeIdentifier, CompoundFileHeader.FileTypeIdentifier)) return false;
            if (!ArrayEqual(Header.ByteOrderMark, Endianness.LittleEndian)) throw new Exception("Endian not implemented.");
            return true;
        }

        void BuildFileStructor()
        {
            SectorSize = (int)Math.Pow(2, Header.SectorSizeInPot);
            ShortSectorSize = (int)Math.Pow(2, Header.ShortSectorSizeInPot);

            MasterSectorAllocationTable = new List<int>();
            SelectSIDs(Header.MasterSectorAllocationTable);
            int msid = Header.FirstSectorIDofMasterSectorAllocationTable;
            while (msid != SID.EOC)
            {
                int[] SIDs = ReadSectorDataAsIntegers(msid);
                SelectSIDs(SIDs);
                msid = SIDs[SIDs.Length - 1];
            }

            SectorAllocationTable = new List<int>(Header.NumberOfSATSectors * SectorSize / 4);
            foreach (int sid in MasterSectorAllocationTable)
            {
                SectorAllocationTable.AddRange(ReadSectorDataAsIntegers(sid));
            }

            ShortSectorAllocationTable = GetStreamDataAsIntegers(Header.FirstSectorIDofShortSectorAllocationTable);

            DirectoryStream = new MemoryStream(GetStreamDataAsBytes(Header.FirstSectorIDofDirectoryStream));
            ReadDirectoryEntries();
        }

        private void ReadDirectoryEntries()
        {
            BinaryReader reader = new BinaryReader(DirectoryStream, Encoding.Unicode);
            DirectoryEntries = new Dictionary<int, DirectoryEntry>();
            DirectoryEntry root = ReadDirectoryEntry(reader);
            root.Document = this;
            DirectoryEntries.Add(0, root);

            ShortStreamContainer = new MemoryStream(GetStreamDataAsBytes(root.FirstSectorID, root.StreamLength));

            ReadDirectoryEntry(reader, root.MembersTreeNodeDID, root);
        }

        private void ReadDirectoryEntry(BinaryReader reader, int DID, DirectoryEntry parent)
        {
            if (DID != -1 && !DirectoryEntries.ContainsKey(DID))
            {
                reader.BaseStream.Position = DID * 128;
                DirectoryEntry entry = ReadDirectoryEntry(reader);
                entry.Document = this;
                entry.Data = GetStreamData(entry);
                DirectoryEntries[DID] = entry;
                parent.Members.Add(entry.Name, entry);
                ReadDirectoryEntry(reader, entry.LeftChildDID, parent);
                ReadDirectoryEntry(reader, entry.RightChildDID, parent);
                ReadDirectoryEntry(reader, entry.MembersTreeNodeDID, entry);
            }
        }

        private void SelectSIDs(int[] SIDs)
        {
            for (int i = 0; i < SIDs.Length; i++)
            {
                int sid = SIDs[i];
                if (sid != SID.Free && sid != SID.EOC)
                {
                    MasterSectorAllocationTable.Add(sid);
                }
            }
        }

        private int GetSectorOffset(int SID)
        {
            return 512 + SectorSize * SID;
        }

        private int GetShortSectorOffset(int SSID)
        {
            return ShortSectorSize * SSID;
        }

        private int[] ReadSectorDataAsIntegers(int SID)
        {
            int offset = GetSectorOffset(SID);
            Reader.BaseStream.Position = offset;
            return ReadArrayOfInt32(Reader, SectorSize / 4);
        }

        private byte[] ReadSectorDataAsBytes(int SID)
        {
            int offset = GetSectorOffset(SID);
            Reader.BaseStream.Position = offset;
            return Reader.ReadBytes(SectorSize);
        }

        private byte[] ReadShortSectorDataAsBytes(int SSID)
        {
            int offset = GetShortSectorOffset(SSID);
            ShortStreamContainer.Seek(offset, SeekOrigin.Begin);
            return StreamHelper.ReadBytes(ShortStreamContainer, ShortSectorSize);
        }

        private static List<int> GetSIDChain(List<int> SAT, int StartSID)
        {
            List<int> chain = new List<int>();
            int sid = StartSID;
            while (sid != SID.EOC)
            {
                chain.Add(sid);
                sid = SAT[sid];
            }
            return chain;
        }

        private byte[] GetStreamDataAsBytes(int StartSID)
        {
            List<int> chain = GetSIDChain(SectorAllocationTable, StartSID);
            List<byte> data = new List<byte>();
            foreach (int sid in chain)
            {
                data.AddRange(ReadSectorDataAsBytes(sid));
            }
            return data.ToArray();
        }

        private byte[] GetStreamDataAsBytes(int StartSID, int length)
        {
            List<int> chain = GetSIDChain(SectorAllocationTable, StartSID);
            List<byte> data = new List<byte>();
            foreach (int sid in chain)
            {
                data.AddRange(ReadSectorDataAsBytes(sid));
            }
            if (data.Count > length)
            {
                data.RemoveRange(length, data.Count - length);
            }
            return data.ToArray();
        }

        private List<int> GetStreamDataAsIntegers(int StartSID)
        {
            List<int> chain = GetSIDChain(SectorAllocationTable, StartSID);
            List<int> data = new List<int>();
            foreach (int sid in chain)
            {
                data.AddRange(ReadSectorDataAsIntegers(sid));
            }
            return data;
        }

        private byte[] GetShortStreamDataAsBytes(int StartSSID)
        {
            List<int> chain = GetSIDChain(ShortSectorAllocationTable, StartSSID);
            List<byte> data = new List<byte>();
            foreach (int sid in chain)
            {
                data.AddRange(ReadShortSectorDataAsBytes(sid));
            }
            return data.ToArray();
        }

        public byte[] GetStreamData(DirectoryEntry entry)
        {
            if (entry.EntryType == EntryType.Stream)
            {
                if (entry.StreamLength < Header.MinimumStreamSize)
                {
                    return GetShortStreamDataAsBytes(entry.FirstSectorID);
                }
                else
                {
                    return GetStreamDataAsBytes(entry.FirstSectorID);
                }
            }
            return null;
        }

        public DirectoryEntry FindDirectoryEntry(DirectoryEntry entry, string entryName)
        {
            if (entry.Members.ContainsKey(entryName)) return entry.Members[entryName];
            foreach (DirectoryEntry subentry in entry.Members.Values)
            {
                return FindDirectoryEntry(subentry, entryName);
            }
            return null;
        }

        public byte[] GetStreamData(string streamName)
        {
            DirectoryEntry userstream = FindDirectoryEntry(RootStorage, streamName);
            if (userstream != null)
            {
                return userstream.Data;
            }
            return null;
        }
    }
}
