using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcapParser
{
    public class PcapPacket
    {
        private PcapPacketHeader _header;
        private PcapPacketData _data;

        public PcapPacket(PcapPacketHeader header,PcapPacketData data)
        {
            _header = header;
            _data = data;
        }
    }
    public class PcapPacketHeader
    {
        public static int HeaderLength = sizeof(uint) * 4;
        public uint TsSec { get;}
        public uint TsUSec { get; }
        public uint InclLen { get; }
        public uint OrigLen { get; }
        public bool PacketInterrupted => OrigLen > InclLen;

        public PcapPacketHeader(uint tsSec, uint tsUSec, uint inclLen, uint origLen)
        {
            TsSec = tsSec;
            TsUSec = tsUSec;
            InclLen = inclLen;
            OrigLen = origLen;
        }
    }

    public class PcapPacketData
    {
        private byte[] _data;

        public PcapPacketData(byte[] data )
        {
            _data = data;
        }
    }
}
