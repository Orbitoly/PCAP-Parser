using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PcapParser.BitsOperations;

namespace PcapParser.Protocols
{
    class HttpPacket : ProtocolPacket
    {
        public HttpPacket(byte[] data) : base(data)
        {
        }

    

        protected override RawPacketData Parse(byte[] data)
        {
            //  |Version(4bit),HeaderLen(4bit) x|DSCP(6bit),ECN(2bit) x|Total Length(2Byte) xx |ID(2Byte) xx|Flags(3bit),Fragment Offset(13bit) xx
            //  |TTL(1Byte) x|Protocol(1Byte) x|Header Checksum(2Byte) xx|SrcIP(4Byte) x.x.x.x|DestIP(4Byte) x.x.x.x|
            //------------------------------------------------------------------------------------------------------------------------------------//
            
            var dataList = data.ToList();
            var b= BitConverter.ToString(data);
            var c = Encoding.ASCII.GetString(data);
            string[] lines =c.Split(new string[] { "\r\n"}, StringSplitOptions.None);
            
            
            return null;
        }

        public override bool Valid { get; set; }
        protected override List<ProtocolEnum> GetSupportedPorotocls()
        {
            return new List<ProtocolEnum>() {ProtocolEnum.NULL};
        }

        public override string GetAttributes()
        {
            var str = "<Http>\n"
                      //"+Destination Address: " + DestinationAddress.ToString() +
                      //"\nSource Address: " + SourceAddress.ToString()
                      + "\n\n";
            return str;
        }
    }
}
