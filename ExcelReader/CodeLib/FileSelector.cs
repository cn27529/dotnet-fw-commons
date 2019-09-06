using System;
using System.Windows.Forms;

namespace QiHe.CodeLib
{
    public enum FileType { Txt, Xml, PDF, Bin, Zip, All, Img }
    public class FileSelector
    {
        public static string Title = "ÇëÑ¡ÔñÎÄ¼þ";
        public static string Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
        public static FileType FileExtension
        {
            set
            {
                switch (value)
                {
                    case FileType.Txt:
                        Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                        break;
                    case FileType.Xml:
                        Filter = "XML files (*.xml)|*.xml|Config files (*.config)|*.config|All files (*.*)|*.*";
                        break;
                    case FileType.PDF:
                        Filter = "PDF files (*.pdf)|*.pdf|PDF form files (*.fdf)|*.fdf|All files (*.*)|*.*";
                        break;
                    case FileType.Bin:
                        Filter = "Binary files (*.bin)|*.bin|Application files(*.exe;*.dll)|*.exe;*.dll|All files (*.*)|*.*";
                        break;
                    case FileType.Zip:
                        Filter = "Zip files (*.zip)|*.zip|All files (*.*)|*.*";
                        break;
                    case FileType.Img:
                        Filter = "Gif(*.gif)|*.gif|Jpeg(*.jpg)|*.jpg|Emf(*.emf)|*.emf|Bmp(*.bmp)|*.bmp|Png(*.png)|*.png";
                        break;
                    case FileType.All:
                        Filter = "All files (*.*)|*.*";
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the initial directory displayed by the file dialog box.
        /// </summary>
        public static string InitialPath
        {
            set
            {
                OFD.InitialDirectory = value;
                SFD.InitialDirectory = value;
            }
        }
        public static OpenFileDialog OFD = new OpenFileDialog();
        public static SaveFileDialog SFD = new SaveFileDialog();

        public static string BrowseFile()
        {
            OFD.Title = Title;
            OFD.Filter = Filter;
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                return OFD.FileName;
            }
            else
            {
                return null;
            }
        }
        public static string BrowseFileForSave()
        {
            SFD.Title = Title;
            SFD.Filter = Filter;
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                return SFD.FileName;
            }
            else
            {
                return null;
            }
        }
        public static string BrowseFile(FileType type)
        {
            FileExtension = type;
            return BrowseFile();
        }
        public static string BrowseFile(string filter)
        {
            Filter = filter;
            return BrowseFile();
        }
        public static string BrowseFileForSave(FileType type)
        {
            FileExtension = type;
            return BrowseFileForSave();
        }
    }
}
