using System;
using System.Collections.Generic;
using System.Text;

namespace QiHe.Office.CompoundDocumentFormat
{
    /// <summary>
    /// Directory Entry Structure 
    /// The size of each directory entry is exactly 128 bytes.
    /// </summary>
    public class DirectoryEntry
    {
        /// <summary>
        /// Character array of the name of the entry, always 16-bit Unicode characters,
        /// with trailing zero character (results in a maximum name length of 31 characters)
        /// </summary>
        public char[] NameBuffer;

        /// <summary>
        /// Size of the used area of the character buffer of the name 
        /// (not character count), including the trailing zero character
        /// </summary>
        public UInt16 NameDataSize;

        /// <summary>
        /// Type of the entry: 
        /// 00H = Empty 03H = LockBytes (unknown)
        /// 01H = User storage 04H = Property (unknown)
        /// 02H = User stream 05H = Root storage
        /// </summary>
        public byte EntryType;

        /// <summary>
        /// Node colour of the entry: 00H = Red 01H = Black
        /// </summary>
        public byte NodeColor;

        /// <summary>
        /// DID of the left child node inside the red-black tree of all direct members of the parent storage 
        /// (if this entry is a user storage or stream), �C1 if there is no left child
        /// </summary>
        public int LeftChildDID;

        /// <summary>
        /// DID of the right child node inside the red-black tree of all direct members of the parent storage
        /// (if this entry is a user storage or stream), �C1 if there is no right child
        /// </summary>
        public int RightChildDID;

        /// <summary>
        /// The directory organises direct members (storages and streams) of each storage in a separate red-black tree.
        /// DID of the root node entry of the red-black tree of all storage members
        /// (if this entry is a storage), �C1 otherwise
        /// </summary>
        public int MembersTreeNodeDID;

        /// <summary>
        /// Unique identifier, if this is a storage (not of interest in the following, may be all 0)
        /// </summary>
        public Guid UniqueIdentifier;

        /// <summary>
        /// User flags (not of interest in the following, may be all 0)
        /// </summary>
        public int UserFlags;

        /// <summary>
        /// Time stamp of creation of this entry.
        /// Most implementations do not write a valid time stamp, but fill up this space with zero bytes.
        /// </summary>
        public DateTime CreationTime;

        /// <summary>
        /// Time stamp of last modification of this entry. 
        /// Most implementations do not write a valid time stamp, but fill up this space with zero bytes.
        /// </summary>
        public DateTime LastModificationTime;

        /// <summary>
        /// SID of first sector or short-sector, if this entry refers to a stream,
        /// SID of first sector of the short-stream container stream, if this is the root storage entry,
        /// 0 otherwise
        /// </summary>
        public int FirstSectorID;

        /// <summary>
        /// Total stream size in bytes, if this entry refers to a stream,
        /// total size of the shortstream container stream, if this is the root storage entry, 
        /// 0 otherwise
        /// </summary>
        public int StreamLength;

        /// <summary>
        /// Not used
        /// </summary>
        public int UnUsed;

        public CompoundDocument Document;
        byte[] data;
        public byte[] Data
        {
            get
            {
                if (data == null)
                {
                    data = Document.GetStreamData(this);
                }
                return data;
            }
            set
            {
                data = value;
            }
        }
        public Dictionary<string, DirectoryEntry> Members = new Dictionary<string, DirectoryEntry>();

        public override string ToString()
        {
            return Name;
        }

        string name;
        public string Name
        {
            get
            {
                if (name == null)
                {
                    int NameLength = NameDataSize / 2 - 1;
                    //NameLength = 1;
                    //foreach (char ch in NameBuffer)
                    //{
                    //    if (ch != '\0')
                    //    {
                    //        NameLength++;
                    //    }
                    //    else
                    //    {
                    //        break;
                    //    }
                    //}
                    if (NameLength == 1)
                    {
                        name = String.Empty;
                    }
                    else
                    {
                        name = new string(NameBuffer, 0, NameLength);
                    }
                }
                return name;
            }
            set
            {
                if (value.Length > 31)
                {
                    throw new Exception("Directory Entry Name exceeds 31 chars.");
                }
                value.ToCharArray().CopyTo(NameBuffer, 0);
                NameBuffer[value.Length] = '\0';
                NameDataSize = (UInt16)((value.Length + 1) * 2);
                name = value;
            }
        }
    }
}
