using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PcapParser.Protocols;
using PcapParser.TCP_IP_Layers_Interfaces;

namespace PcapParser
{
    interface IControlLayer: ILayer
    {
        ILayer LowerLayer { get; }
    }
}
