using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static PcapParser.BitsOperations;

namespace PcapParser.Protocols
{
    class IPv4Packet : ProtocolPacket
    {
        public int HeaderLen { get; private set; }
        public long TotalLen { get; private set; }
        public int ID { get; private set; }
        public int TTL { get; private set; }
        public IPAddress SourceIP { get; private set; }
        public IPAddress DestIP { get; private set; }

        public IPv4Packet(byte[] data) : base(data)
        {
        }

       

        protected override RawPacketData Parse(byte[] data)
        {

            //  |Version(4bit),HeaderLen(4bit) x|DSCP(6bit),ECN(2bit) x|Total Length(2Byte) xx |ID(2Byte) xx|Flags(3bit),Fragment Offset(13bit) xx
            //  |TTL(1Byte) x|Protocol(1Byte) x|Header Checksum(2Byte) xx|SrcIP(4Byte) x.x.x.x|DestIP(4Byte) x.x.x.x|
            //------------------------------------------------------------------------------------------------------------------------------------//

            var dataList = data.ToList();

            //  |Version(4bit),HeaderLen(4bit) x|
            var version = BitsOperations.ReadBitsFromByteInt32(dataList, (int) FieldSizeBits.Version, Direction.Left);
            var headerLen = BitsOperations.ReadBitsFromByteInt32(dataList, (int) FieldSizeBits.HeaderLen, Direction.Right)*4;//mult by 4 because num represents how many 32bit data is in header.
            dataList = dataList.Skip(((int) FieldSizeBits.Version + (int) FieldSizeBits.HeaderLen) / BYTE_LEN).ToList();

            //  |DSCP(6bit),ECN(2bit) x|
            var dscp = BitsOperations.ReadBitsFromByteInt32(dataList, (int) FieldSizeBits.DSCP, Direction.Left);
            var ecn = BitsOperations.ReadBitsFromByteInt32(dataList, (int) FieldSizeBits.ECN, Direction.Right);
            dataList = dataList.Skip(((int) FieldSizeBits.DSCP + (int) FieldSizeBits.ECN) / BYTE_LEN).ToList();

            //  |Total Length(2Byte) xx |
            var totalLength = BitsOperations.ReadBitsFromByteInt64(dataList, (int)FieldSizeBits.TotalLength, Direction.Left);
            dataList = dataList.Skip(((int) FieldSizeBits.TotalLength) / BYTE_LEN).ToList();

            //  |ID(2Byte) xx|
            var id = BitsOperations.ReadBitsFromByteInt32(dataList, (int) FieldSizeBits.ID, Direction.Left);
            dataList = dataList.Skip(((int) FieldSizeBits.ID) / BYTE_LEN).ToList();

            //  |Flags(3bit),Fragment Offset(13bit) xx|
            var flags = BitsOperations.ReadBitsFromByteInt32(dataList, (int) FieldSizeBits.Flags, Direction.Left);
            var fragmentOffset = BitsOperations.ReadBitsFromByteInt32(dataList, (int) FieldSizeBits.FragmentOffset,
                Direction.Right);
            dataList =
                dataList.Skip(((int) FieldSizeBits.Flags + (int) FieldSizeBits.FragmentOffset) / BYTE_LEN).ToList();

            //  |TTL(1Byte) x|
            var ttl = BitsOperations.ReadBitsFromByteInt32(dataList, (int) FieldSizeBits.TTL, Direction.Left);
            dataList = dataList.Skip(((int) FieldSizeBits.TTL) / BYTE_LEN).ToList();

            //  |Protocol(1Byte) x|
            var protocol = BitsOperations.ReadBitsFromByteInt32(dataList, (int) FieldSizeBits.Protocol, Direction.Left);
            dataList = dataList.Skip(((int) FieldSizeBits.Protocol) / BYTE_LEN).ToList();

            //  |Header Checksum(2Byte) xx|
            var checksum = BitsOperations.ReadBitsFromByteInt32(dataList, (int) FieldSizeBits.Checksum, Direction.Left);
            dataList = dataList.Skip(((int) FieldSizeBits.Checksum) / BYTE_LEN).ToList();

            //  |SrcIP(4Byte) x.x.x.x|
            var srcIp = BitsOperations.ReadBitsFromByteInt32(dataList, (int) FieldSizeBits.SrcIP, Direction.Left);
            dataList = dataList.Skip(((int) FieldSizeBits.SrcIP) / BYTE_LEN).ToList();

            //  |DestIP(4Byte) x.x.x.x|
            var destIp = BitsOperations.ReadBitsFromByteInt32(dataList, (int) FieldSizeBits.DestIP, Direction.Left);
            dataList = dataList.Skip(((int) FieldSizeBits.DestIP) / BYTE_LEN).ToList();

            //------------------------------------------------------------------------------------------------------------------------------------//
            HeaderLen = headerLen;
            TotalLen = totalLength;
            ID = id;
            TTL = ttl;
            SourceIP = new IPAddress(IPAddress.HostToNetworkOrder(srcIp));
            DestIP = new IPAddress(IPAddress.HostToNetworkOrder(destIp));

            return new RawPacketData(dataList.ToArray(), ParseProtocolName(protocol.ToString()));
        }

        public override string GetAttributes()
        {
            var str = "<IPv4>\n" +
                      "Header Length: " + HeaderLen+
                      "\nTotal Length: " + TotalLen + 
                      "\nID: "+ID+
                      "\nTime To Live: "+TTL+
                      "\nSource IP: "+SourceIP.ToString()+
                      "\nDestination IP: "+DestIP.ToString()+
                      "\n\n";
            return str;
        }
        public override bool Valid { get; set; }

        protected override List<ProtocolEnum> GetSupportedPorotocls()
        {
            throw new NotImplementedException();
        }

        internal enum FieldSizeBits : int
        {
            Version = 4,
            HeaderLen = 4,
            DSCP = 6,
            ECN = 2,
            TotalLength = 16,
            ID = 16,
            Flags = 3,
            FragmentOffset = 13,
            TTL = 8,
            Protocol = 8,
            Checksum = 16,
            SrcIP = 32,
            DestIP = 32
        }

        private ProtocolEnum ParseProtocolName(string name)
        {
            return ProtocolEnum.TCP;
        }
    }
}