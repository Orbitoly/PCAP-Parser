using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcapParser.Interfaces
{
    interface IInternetDataLayer:IInternetLayer
    {
        ILinkLayer LowerLayer { get;}
        RawPacketData WrappedProtocolRaw { get; }


    }
}
