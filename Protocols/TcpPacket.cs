using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PcapParser.BitsOperations;

namespace PcapParser.Protocols
{
    class TcpPacket:ProtocolPacket
    {
        public int SourcePort { get; private set; }
        public int DestinationPort { get; private set; }
        public int SeqNumber { get; private set; }
        public int AckNumber { get; private set; }
        public int HeaderLength { get; private set; }
        public int WindowSize { get; private set; }
        private int Checksum { get; set; }

        public TcpPacket(byte[] data):base(data)
        {
        }

        

        protected override List<ProtocolEnum> GetSupportedPorotocls()
        {
            return new List<ProtocolEnum>() { ProtocolEnum.HTTP, ProtocolEnum.HTTPS };
        }

        protected override RawPacketData Parse(byte[] data)
        {
            //  |SourcePort(2 Bytes) xx|DestinationPort(2 Bytes) xx|Sequence Number(4 Bytes) xxxx|ACK Number(4 Bytes) xxxx|Header Length(Byte) x |Flag(1Byte) x|
            //  |Window Size(2 Bytes) xx|Checksum(2 Bytes) xx|Urgent(2 Bytes) xx|Options(? Bits) ?|Pading(? Bits) ?|
            //------------------------------------------------------------------------------------------------------------------------------------//

            var dataList = data.ToList();

            //  |SourcePort(2 Bytes) xx|
            var srcPort = BitsOperations.ReadBitsFromByteInt32(dataList, (int)FieldSizeBits.SrcPort, Direction.Left);
            dataList = dataList.Skip(((int)FieldSizeBits.SrcPort) / BYTE_LEN).ToList();

            //  |DestinationPort(2 Bytes) xx|
            var dstPort = BitsOperations.ReadBitsFromByteInt32(dataList, (int)FieldSizeBits.DestPort, Direction.Left);
            dataList = dataList.Skip(((int)FieldSizeBits.DestPort) / BYTE_LEN).ToList();

            //  |Sequence Number(4 Bytes) xxxx|
            var seqNum = BitsOperations.ReadBitsFromByteInt32(dataList, (int)FieldSizeBits.SeqNum, Direction.Left);
            dataList = dataList.Skip(((int)FieldSizeBits.SeqNum) / BYTE_LEN).ToList();

            //  |ACK Number(4 Bytes) xxxx|
            var ackNum = BitsOperations.ReadBitsFromByteInt32(dataList, (int)FieldSizeBits.AckNum, Direction.Left);
            dataList = dataList.Skip(((int)FieldSizeBits.AckNum) / BYTE_LEN).ToList();

            //  |Header Length(4 bits) Saved(4 bits) x |
            var headerLen = BitsOperations.ReadBitsFromByteInt32(dataList, (int)FieldSizeBits.HeaderLen, Direction.Left);
            var saved = BitsOperations.ReadBitsFromByteInt32(dataList, (int)FieldSizeBits.Saved, Direction.Right);
            dataList = dataList.Skip(((int)FieldSizeBits.HeaderLen+(int)FieldSizeBits.Saved) / BYTE_LEN).ToList();

            //  |Flag(1Byte) x|
            var flags = BitsOperations.ReadBitsFromByteInt32(dataList, (int)FieldSizeBits.Flags, Direction.Left);
            dataList = dataList.Skip(((int)FieldSizeBits.Flags ) / BYTE_LEN).ToList();

            //  |Window Size(2 Bytes)|
            var windowSize = BitsOperations.ReadBitsFromByteInt32(dataList, (int)FieldSizeBits.WindowSize, Direction.Left);
            dataList = dataList.Skip(((int)FieldSizeBits.WindowSize) / BYTE_LEN).ToList();


            //  |Checksum(2 Bytes) xx|
            var checksum = BitsOperations.ReadBitsFromByteInt32(dataList, (int)FieldSizeBits.Checksum, Direction.Left);
            dataList = dataList.Skip(((int)FieldSizeBits.Checksum) / BYTE_LEN).ToList();

            //  |Urgent(2 Bytes) xx|
            var urgent = BitsOperations.ReadBitsFromByteInt32(dataList, (int)FieldSizeBits.Urgent, Direction.Left);
            dataList = dataList.Skip(((int)FieldSizeBits.Urgent) / BYTE_LEN).ToList();

            //////////////////| Options(? Bits) ?| Pading(? Bits) ?|
            SourcePort = srcPort;
            DestinationPort = dstPort;
            SeqNumber = seqNum;
            AckNumber = ackNum;
            HeaderLength = headerLen;
            WindowSize = windowSize;
            Checksum = checksum; 
             
            
            return new RawPacketData(dataList.ToArray(),ParseProtocolName(dataList.ToArray()));
        }


        public override bool Valid { get; set; }
        private ProtocolEnum ParseProtocolName(string name)
        {
            throw new NotImplementedException();

        }
        private ProtocolEnum ParseProtocolName(byte[] nextPacketData)
        {
            return ProtocolEnum.HTTP;

        }
        internal enum FieldSizeBits:int
        {
            SrcPort = 16, DestPort = 16, SeqNum =32,AckNum=32, HeaderLen =4, Saved =4, Flags = 8, FragmentOffset = 13, WindowSize =16, Checksum =16, Urgent = 16
        }

        public override string GetAttributes()
        {
            var str = "<TCP>\n" +
                      "Source Port: " + SourcePort +
                      "\nDestinationPort: " + DestinationPort +
                      "\nSequence Number: " + SeqNumber +
                      "\nAcknowledgment Number: " + AckNumber +
                      "\nHeader Length: " + HeaderLength.ToString() +
                      "\nWindow Size: " + WindowSize.ToString() +
                      "\n\n";
            return str;
        }
    }
}
