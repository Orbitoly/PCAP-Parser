using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace PcapParser
{
    class MyBinaryReader : BinaryReader
    {
        public BinaryReader Reader { get; private set; }
        public EndianEnum Endian { set; get; }

        public MyBinaryReader(Stream st, EndianEnum endianVal = EndianEnum.BigEndian) : base(st)
        {
            Reader = new BinaryReader(st);
            Endian = endianVal;


        }
        public override byte[] ReadBytes(int count)
        {
            if(!CanRead(count))
                throw new EndOfStreamException();
            var bytes = base.ReadBytes(count);
            
            if (Endian == EndianEnum.BigEndian)
                Array.Reverse(bytes, 0, bytes.Length);
            return bytes;
        }

        public bool CanRead(int count)
        {
            return Reader.BaseStream.Position + count <= Reader.BaseStream.Length;
        }
    }
    public enum EndianEnum
    {
        LittleEndian, BigEndian
    }


}
