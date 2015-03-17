using System;
using System.IO;

namespace School
{
    public static class StringExtension
    {
        public static StreamReader ToStreamReader(this String str)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            return new StreamReader(stream);
        }
    }
}

