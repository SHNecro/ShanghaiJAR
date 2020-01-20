using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NSGame
{
    internal static class Text
    {
        private const string Encode = "Shift_JIS";

        public static string[] Read(string Resource)
        {
            if (Resource == null)
                return null;
            List<string> stringList = new List<string>(0);
            string[] strArray = Resource.Replace("\r", "").Split('\n');
            for (int index = 0; index < strArray.Length; ++index)
            {
                if (strArray[index].IndexOf("\n") != 0 && strArray[index].IndexOf("//") != 0 && !(strArray[index] == ""))
                    stringList.Add(strArray[index]);
            }
            return stringList.ToArray();
        }

        public static string[] Load(string FileName, string Resource)
        {
            List<string> stringList = new List<string>(0);
            if (!File.Exists(FileName))
                return NSGame.Text.Read(Resource);
            using (StreamReader streamReader = new StreamReader(FileName, Encoding.GetEncoding("Shift_JIS")))
            {
                string str;
                while ((str = streamReader.ReadLine()) != null)
                {
                    if (str.IndexOf("\n") != 0 && str.IndexOf("//") != 0 && !(str == ""))
                        stringList.Add(str);
                }
            }
            return stringList.ToArray();
        }

        public static void Write(string[] s, string FileName)
        {
            using (StreamWriter streamWriter = new StreamWriter(FileName, false, Encoding.GetEncoding("Shift_JIS")))
            {
                for (int index = 0; index < s.Length; ++index)
                    streamWriter.WriteLine(s[index]);
                streamWriter.Close();
            }
        }

        public static string Split(string s, int Count)
        {
            string[] strArray = s.Split('=');
            if (Count > strArray.Length)
                Count = strArray.Length - 1;
            return strArray[Count];
        }

        public static int Parse(string s)
        {
            if (s == null)
                return 0;
            if (Regex.Match(s, "^[0-9]").Success)
            {
                try
                {
                    return int.Parse(s);
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                if (!Regex.IsMatch(s, "[0-9]"))
                    return 0;
                Regex.Match(s, "[0-9]");
                try
                {
                    return int.Parse(NSGame.Text.Split(s, 1));
                }
                catch
                {
                    return 0;
                }
            }
        }
    }
}
