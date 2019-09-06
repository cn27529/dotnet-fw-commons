using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace QiHe.CodeLib
{
    /// <summary>
    /// XmlFile load and save object from and to xml file;
    /// </summary>	
    public class XmlData<DataType>
    {
        public static DataType Load(string xmlfile)
        {
            DataType data;
            Type datatype = typeof(DataType);
            XmlSerializer mySerializer = new XmlSerializer(datatype);
            if (File.Exists(xmlfile))
            {
                using (XmlTextReader reader = new XmlTextReader(xmlfile))
                {
                    data = (DataType)mySerializer.Deserialize(reader);
                }
            }
            else
            {
                //throw new FileNotFoundException(xmlfile);
                return default(DataType);
            }
            return data;
        }
        public static DataType Load(Stream xmldata)
        {
            using (XmlTextReader reader = new XmlTextReader(xmldata))
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(DataType));
                return (DataType)mySerializer.Deserialize(reader);
            }
        }
        public static DataType Load(string xmlfile, string root)
        {
            DataType data;
            Type datatype = typeof(DataType);
            XmlRootAttribute rootattr = new XmlRootAttribute(root);
            XmlSerializer mySerializer = new XmlSerializer(datatype, rootattr);
            if (File.Exists(xmlfile))
            {
                XmlTextReader reader = new XmlTextReader(xmlfile);
                data = (DataType)mySerializer.Deserialize(reader);
                reader.Close();
            }
            else
            {
                //throw new FileNotFoundException(xmlfile);
                return default(DataType);
            }
            return data;
        }
        public static void Save(string xmlfile, DataType data)
        {
            XmlSerializer mySerializer = new XmlSerializer(data.GetType());
            XmlTextWriter writer = new XmlTextWriter(xmlfile, System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            mySerializer.Serialize(writer, data);
            writer.Close();
        }
    }
    /// <summary>
    /// XmlFile load and save object from and to xml file;
    /// </summary>	
    public class XmlFile
    {
        public static object Load(string xmlfile, Type datatype)
        {
            object data = null;
            XmlSerializer mySerializer = new XmlSerializer(datatype);
            if (File.Exists(xmlfile))
            {
                XmlTextReader reader = new XmlTextReader(xmlfile);
                data = mySerializer.Deserialize(reader);
                reader.Close();
            }
            else
            {
                //throw new FileNotFoundException(xmlfile);
                return null;
            }
            return data;
        }
        public static void Save(string xmlfile, object data)
        {
            XmlSerializer mySerializer = new XmlSerializer(data.GetType());
            XmlTextWriter writer = new XmlTextWriter(xmlfile, System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            mySerializer.Serialize(writer, data);
            writer.Close();
        }
        public static void Save(string xmlfile, string root, object data)
        {
            XmlRootAttribute rootattr = new XmlRootAttribute(root);
            XmlSerializer mySerializer = new XmlSerializer(data.GetType(), rootattr);
            XmlTextWriter writer = new XmlTextWriter(xmlfile, System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            mySerializer.Serialize(writer, data);
            writer.Close();
        }
    }
    public class TxtFile
    {
        public static void Create(string file)
        {
            StreamWriter sw = File.CreateText(file);
            sw.Close();
        }
        public static string Read(string file)
        {
            StreamReader sr = File.OpenText(file);
            string text = sr.ReadToEnd();
            sr.Close();
            return text;
        }
        //Encodings: "ASCII","UTF-8","Unicode","GB2312","GB18030"
        public static string Read(string file, string encoding)
        {
            return Read(file, Encoding.GetEncoding(encoding));
        }
        public static List<string> ReadLines(string file, string encoding)
        {
            return ReadLines(file, Encoding.GetEncoding(encoding));
        }
        public static string Read(string file, Encoding encoding)
        {
            StreamReader sr = new StreamReader(file, encoding);
            string text = sr.ReadToEnd();
            sr.Close();
            return text;
        }
        public static List<string> ReadLines(string file, Encoding encoding)
        {
            StreamReader reader = new StreamReader(file, encoding);
            List<string> lines = new List<string>();
            string line = reader.ReadLine();
            while (line != null)
            {
                lines.Add(line);
                line = reader.ReadLine();
            }
            reader.Close();
            return lines;
        }
        public static void Write(string file, string text)
        {
            StreamWriter sw = new StreamWriter(file, false, Encoding.UTF8);
            sw.Write(text);
            sw.Close();
        }
        public static void Append(string file, string text)
        {
            StreamWriter sw = File.AppendText(file);
            sw.WriteLine(text);
            sw.Close();
        }
        public static void Append(string file, string text, Encoding encoding)
        {
            StreamWriter sw = new StreamWriter(file, true, encoding);
            sw.Write(text);
            sw.Close();
        }
        /// <summary>
        /// 返回从reader的当前位置到tag之间的内容
        /// 要求tag独占一行
        /// 如果tag为null,返回当前位置到末尾的内容
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="tag"></param>
        /// <returns>reader的当前位置到tag之间的内容,不包括tag</returns>
        public static string GetText(TextReader reader, string tag)
        {
            if (tag == null)
            {
                return reader.ReadToEnd();
            }
            StringBuilder text = new StringBuilder();
            string line = reader.ReadLine();
            while (line != null)
            {
                if (line == tag)
                {
                    int n = Environment.NewLine.Length;
                    if (text.Length > n)
                    {
                        text.Remove(text.Length - n, n);
                    }
                    return text.ToString();
                }
                text.Append(line + Environment.NewLine);
                line = reader.ReadLine();
            }
            return null;
        }

        /// <summary>
        /// 取文件中两个tag之间的内容
        /// 要求tag独占一行，null为文件尾
        /// </summary>
        /// <param name="file">UTF8文本文件</param>
        /// <param name="startTag">启始Tag</param>
        /// <param name="endTag">结束Tag</param>
        /// <returns>两个tag之间的内容，如果未找到startTag返回null</returns>
        public static string GetText(string file, string startTag, string endTag)
        {
            StreamReader reader = File.OpenText(file);
            StringBuilder text = new StringBuilder();
            bool found = false;
            string line = reader.ReadLine();
            while (line != null)
            {
                if (line == endTag) { break; }
                if (found) { text.AppendLine(line); }
                if (line == startTag) { found = true; }
                line = reader.ReadLine();
            }
            reader.Close();
            if (found) return text.ToString(); return null;
        }

        public static string GetText(string file, string startTag, string endTag, out int lineNum)
        {
            StreamReader reader = File.OpenText(file);
            StringBuilder text = new StringBuilder();
            bool found = false;
            int linecounter = 0; lineNum = -1;
            string line = reader.ReadLine();
            while (line != null)
            {
                linecounter++;
                if (line == endTag) { break; }
                if (found) { text.AppendLine(line); }
                if (line == startTag) { found = true; lineNum = linecounter + 1; }
                line = reader.ReadLine();
            }
            reader.Close();
            if (found) return text.ToString(); return null;
        }

        public static void CountLinesAndChars(string file, out int lines, out int chars)
        {
            StreamReader reader = File.OpenText(file);
            lines = 0;
            chars = 0;
            string line = reader.ReadLine();
            while (line != null)
            {
                lines++;
                chars += line.Length;
                line = reader.ReadLine();
            }
            reader.Close();
        }
    }
    public class BinFile
    {
        public static object Read(string datafile)
        {
            if (File.Exists(datafile))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(datafile, FileMode.Open, FileAccess.Read, FileShare.Read);
                object obj = formatter.Deserialize(stream);
                stream.Close();
                return obj;
            }
            else
            {
                throw new FileNotFoundException(datafile);
            }
        }
        public static void Write(string datafile, object obj)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(datafile, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, obj);
            stream.Close();
        }
    }
}
