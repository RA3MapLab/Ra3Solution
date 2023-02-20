using System.IO;

namespace Compress
{
    public class RefpackComrpessor
    {
        public static void Decompress(BinaryReader input, BinaryWriter output)
		{
			long oldPos = 0L;
			long newPos = 0L;
			byte code = 0;
			byte code2 = 0;
			byte code3 = 0;
			byte code4 = 0;
			int count = 0;
			int repeatAvailable = 0;
			int size = GetUncompressedSize(input);
			long i = 0L;
			while (true)
			{
				if (output.BaseStream.Length * 100 / size > i)
				{
					i = output.BaseStream.Length * 100 / size;
				}
				code = input.ReadByte();
				if ((code & 0x80) == 0)
				{
					code2 = input.ReadByte();
					count = code & 3;
					output.Write(input.ReadBytes(count));
					oldPos = output.BaseStream.Position;
					newPos = oldPos - 1 - (code2 + (code & 0x60) * 8);
					repeatAvailable = (int)(oldPos - newPos);
					count = (code & 0x1C) / 4 + 3;
					output.BaseStream.Seek(-repeatAvailable, SeekOrigin.Current);
					byte[] temp3 = new BinaryReader(output.BaseStream).ReadBytes(count);
					output.BaseStream.Seek(0L, SeekOrigin.End);
					output.Write(temp3);
					if (count > repeatAvailable)
					{
						CopyRepeat(input, output, oldPos, newPos, count, repeatAvailable);
					}
				}
				else if ((code & 0x40) == 0)
				{
					code2 = input.ReadByte();
					code3 = input.ReadByte();
					count = code2 >> 6;
					output.Write(input.ReadBytes(count));
					oldPos = output.BaseStream.Position;
					newPos = oldPos - 1 - (((code2 & 0x3F) << 8) + code3);
					repeatAvailable = (int)(oldPos - newPos);
					count = (code & 0x3F) + 4;
					output.BaseStream.Seek(-repeatAvailable, SeekOrigin.Current);
					byte[] temp2 = new BinaryReader(output.BaseStream).ReadBytes(count);
					output.BaseStream.Seek(0L, SeekOrigin.End);
					output.Write(temp2);
					if (count > repeatAvailable)
					{
						CopyRepeat(input, output, oldPos, newPos, count, repeatAvailable);
					}
				}
				else if ((code & 0x20) == 0)
				{
					code2 = input.ReadByte();
					code3 = input.ReadByte();
					code4 = input.ReadByte();
					count = code & 3;
					output.Write(input.ReadBytes(count));
					oldPos = output.BaseStream.Position;
					newPos = oldPos - 1 - (((code & 0x10) >> 4 << 16) + (code2 << 8) + code3);
					repeatAvailable = (int)(oldPos - newPos);
					count = ((code & 0xC) >> 2 << 8) + code4 + 5;
					output.BaseStream.Seek(-repeatAvailable, SeekOrigin.Current);
					byte[] temp = new BinaryReader(output.BaseStream).ReadBytes(count);
					output.BaseStream.Seek(0L, SeekOrigin.End);
					output.Write(temp);
					if (count > repeatAvailable)
					{
						CopyRepeat(input, output, oldPos, newPos, count, repeatAvailable);
					}
				}
				else
				{
					count = (code & 0x1F) * 4 + 4;
					if (count > 112)
					{
						break;
					}
					output.Write(input.ReadBytes(count));
				}
			}
			count = code & 3;
			output.Write(input.ReadBytes(count));
			output.BaseStream.Seek(0L, SeekOrigin.Begin);
		}
        
        private static int GetUncompressedSize(BinaryReader br)
        {
	        int compressedsize = 0;

	        var flag = br.ReadByte();
	        if ((flag & 0x80) != 0)
	        {
		        //大文件
		        br.ReadByte();
		        var location = br.BaseStream.Position;
		        br.BaseStream.Position = 4L;
		        compressedsize =  br.ReadInt32();
		        br.BaseStream.Position = location;
		        br.ReadByte();
		        br.ReadByte();
		        br.ReadByte();
		        br.ReadByte();
	        }
	        else
	        {
		        br.ReadByte();
		        var location = br.BaseStream.Position;
		        br.BaseStream.Position = 4L;
		        compressedsize =  br.ReadInt32();
		        br.BaseStream.Position = location;
		        br.ReadByte();
		        br.ReadByte();
		        br.ReadByte();
	        }
			
	        return compressedsize;
        }

        private static void CopyRepeat(BinaryReader input, BinaryWriter output, long oldPos, long newPos, int count, int repeatAvailable)
        {
	        int copyFromEnd = count - repeatAvailable;
	        int i = 0;
	        while (i < copyFromEnd)
	        {
		        output.BaseStream.Seek(oldPos, SeekOrigin.Begin);
		        byte[] temp = new BinaryReader(output.BaseStream).ReadBytes(copyFromEnd - i);
		        oldPos += temp.Length;
		        i += temp.Length;
		        output.BaseStream.Seek(0L, SeekOrigin.End);
		        output.Write(temp);
	        }
        }

    }
}