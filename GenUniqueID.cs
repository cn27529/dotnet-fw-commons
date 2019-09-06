using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commons
{
    public class GenUniqueID
    {   /*
                The System.Guid is used whenever we need to generate a unique key, but it is very long. 
                That’s in many cases not an issue, but in a web scenario where it is part of the URL we need to use its string representation which is 36 characters long. 
                It clutters up the URL and is just basically ugly.
                It is not possible to shorten it without loosing some of the uniqueness of the GUID, but we can come a long way if we can accept a 16 character string instead.
                We can change the standard GUID string representation:
                21726045-e8f7-4b09-abd8-4bcc926e9e28
                Into a shorter string:
                3c4ebc5f5f2c4edc
                The following method creates the shorter string and it is actually very unique. 
                An iteration of 10 million didn’t create a duplicate. It uses the uniqueness of a GUID to create the string. 
             */
        public static string GenSID()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /*
            If you instead want numbers instead of a string, you can do that to but then you need to go up to 19 characters. 
            The following method converts a GUID to an Int64.
         */
        public static long GenID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        //取得縮短版 GUID
        public static string GetShortGuid(Guid uid)
        {
            //if (useBase32)
            //    return Base32Encoding.ToString(uid.ToByteArray());
            //else
            return Convert.ToBase64String(uid.ToByteArray());
        }

        //解析縮短版 GUID
        public static Guid ParseShortGuid(string s)
        {
            //if (useBase32)
            //    return new Guid(Base32Encoding.ToBytes(s));
            //else
            return new Guid(Convert.FromBase64String(s));
        }
    }
}
