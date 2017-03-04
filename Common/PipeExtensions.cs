using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class PipeExtensions
    {
        public static void WriteString(this PipeStream pipeStream, string str)
        {
            byte[] msgBuff = Encoding.UTF8.GetBytes(str);
            pipeStream.Write(msgBuff, 0, msgBuff.Length);
        }

        public static string ReadString(this PipeStream pipeStream)
        {
            int BUFFSIZE = 1024;
            int byteCount = 0;
            var msgBuff = new byte[BUFFSIZE];
            StringBuilder mb = new StringBuilder();
            do
            {
                byteCount = pipeStream.Read(msgBuff, 0, BUFFSIZE);
                mb.Append(Encoding.UTF8.GetString(msgBuff, 0, byteCount));
            } while (!(pipeStream.IsMessageComplete));

            return mb.ToString(); 
        }

        public static void WriteObject<T> (this PipeStream pipeStream, T obj) 
        {
            var stream = ObjectStreamer.SerializeToStream(obj);
            byte[] msgBuff = stream.ToArray();
            pipeStream.Write(msgBuff, 0, msgBuff.Length);
        }

        public static T ReadObject<T>(this PipeStream pipeStream) where T:class
        {
            int BUFFSIZE = 1024;
            int byteCount = 0;
            var msgBuff = new byte[BUFFSIZE];
            var fullMessage = new List<byte>();
           
            do
            {
                byteCount = pipeStream.Read(msgBuff, 0, BUFFSIZE);
                fullMessage.AddRange(msgBuff);
            } while (!(pipeStream.IsMessageComplete));


            MemoryStream stream = new MemoryStream(fullMessage.ToArray());
            return ObjectStreamer.DeserializeFromStream(stream) as T;
        }
    }
}
