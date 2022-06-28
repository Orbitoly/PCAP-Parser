using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PcapParser.Interfaces;

namespace PcapParser.Protocols
{
    class IPv4Protocol:Protocol,IInternetDataLayer
    {


        public ILinkLayer LowerLayer { get; private set; }
        public RawPacketData WrappedProtocolRaw { get; }
        public short HeaderLength { get; private set; }
        public IPv4Protocol(RawPacketData rawPacket,ILinkLayer lower)
        {
            LowerLayer = lower;
            Parse(rawPacket.RawBytes);
        }
        protected override void Parse(byte[] rawData)
        {
            //  |Version(4bit),HeaderLen(4bit) x|DSCP(6bit),ECN(2bit) x|Total Length(2Byte) xx |ID(2Byte) xx|Flags(3bit),Fragment Offset(13bit) xx
            //  |TTL(1Byte) x|Protocol(1Byte) x|Header Checksum(2Byte) xx|SrcIP(4Byte) x.x.x.x|DestIP(4Byte) x.x.x.x
            //var ver = BitsOperations.ReadBitsFromByte(rawData[0], 4, false);
            //var hedLen = BitsOperations.ReadBitsFromByte(rawData[0], 4, true);

            //var version = (rawData[0] & 0xF0)>>4;
            //var headerLen = rawData[0] & 0x0F;
            //var dscp=(rawData[1]& Convert.ToInt32("11111100", 2))>>2;
            //var ecn= rawData[1] & Convert.ToInt32("11", 2);
            //var totalLength = rawData[3];
            //var id = rawData[4] << 8 | rawData[5];
            //var idd = BitsOperations.ReadBitsFromByte(new List<byte>() {rawData[4],rawData[5]},16, false);

            //var flags = (rawData[6] & Convert.ToInt32("11100000", 2))>>5;
            //var fragmentOffset= rawData[6] & Convert.ToInt32("00011111", 2)<<8+ rawData[7];
            //var ttl = rawData[8];
            //var protocol = rawData[9];
            //var headerChecksum = (rawData[10] << 8) + rawData[11];


            //HeaderLength = (short)(headerLen * 4);

            //string type = BitConverter.ToString(rawData.ToList().GetRange(0, 1).ToArray());

            throw new NotImplementedException();
        }
    }
}
