using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using wbInject.Core;

namespace WbInject
{
    public class PipeReaderWriter : IDisposable
    {
        private Stream ioStream;

        public PipeReaderWriter(Stream ioStream)
        {
            this.ioStream = ioStream;
        }

        public IpcData ReceiveCommand()
        {
            int len = 0;

            len = ioStream.ReadByte() * 256;
            len += ioStream.ReadByte();
            byte[] inBuffer = new byte[len];
            ioStream.Read(inBuffer, 0, len);
            Logger.log($"ReceiveCommand | len: {len}");

            if (len == 0)
            {
                return null;
            }

            var msg = Encoding.UTF8.GetString(inBuffer);

            return JsonConvert.DeserializeObject<IpcData>(msg);
        }

        public void SendCommand(IpcData data)
        {
            var outString = JsonConvert.SerializeObject(data);
            byte[] outBuffer = Encoding.UTF8.GetBytes(outString);
            int len = outString.Length;
            if (len > UInt16.MaxValue)
            {
                len = (int)UInt16.MaxValue;
            }
            ioStream.WriteByte((byte)(len / 256));
            ioStream.WriteByte((byte)(len & 255));
            ioStream.Write(outBuffer, 0, outBuffer.Length);
            ioStream.Flush();
        }

        public void Dispose()
        {
            ioStream.Close();
        }
    }
}