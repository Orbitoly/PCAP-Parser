using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//using PcapParser.Protocols;

namespace PcapParser
{
    public class PcapHeader
    {
        private uint _magicNumber;
        private ushort _versionMajor;
        private ushort _versionMinor;
        private int _thiszone;
        private uint _sigfigs;
        private uint _snaplen;
        private uint _network;
        public ProtocolEnum LinkLayerProtocol { get; set; }
        public EndianEnum Endian { get; set; }

        public PcapHeader(uint magicNumber,ushort versionMajor,ushort versionMinor,int thiszone,uint sigfigs,uint snaplen,uint network)
        {
            _magicNumber = magicNumber;
            _versionMajor = versionMajor;
            _versionMinor = versionMinor;
            _thiszone = thiszone;
            _sigfigs = sigfigs;
            _snaplen = snaplen;
            _network = network;
            ParseLinkType(_network);
        }

        private void ParseLinkType(uint network)
        {
            
            switch (network)
            {
                case 0:
                    LinkLayerProtocol = ProtocolEnum.NULL;
                    break;
                case 1:
                    LinkLayerProtocol = ProtocolEnum.Ethernet;
                    break;
                default:
                    LinkLayerProtocol = ProtocolEnum.NULL;
                    throw new Exception("Unknown Link Layer protocol");
                    //break;//FILL MORE TYPES HERE!!!
            }
        }
        public override string ToString()
        {
            return "Header: \n{\n\tMagic Number: "+this._magicNumber+"\n\tVersion Major: "+this._versionMajor+ " Version Minor: "+ this._versionMinor+"\n\tZone: "+this._thiszone+ " Sigfigs: "+this._sigfigs+" Snap Len: "+this._snaplen+
                " Network: "+this._network+"\n}";
        }
    }

}
