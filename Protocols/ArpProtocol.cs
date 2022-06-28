using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PcapParser.Interfaces;
using PcapParser.Protocols;
using PcapParser.TCP_IP_Layers_Interfaces;

namespace PcapParser
{
    class ArpProtocol :Protocol, IInternetLayer,IControlLayer
    {
        public ILayer LowerLayer { get; }

        public ArpProtocol(byte[] rawData,ILinkLayer layerBelow)
        {
            LowerLayer = layerBelow;
            Parse(rawData);
        }
        protected override void Parse(byte[] rawData)
        {
            Console.WriteLine("This is ARP protocol!");
        }
    }
}
