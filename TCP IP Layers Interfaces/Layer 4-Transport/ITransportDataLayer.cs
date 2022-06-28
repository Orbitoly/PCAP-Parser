using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcapParser.Interfaces
{
    interface ITransportDataLayer:ITransportLayer
    {
        IInternetLayer LowerLayer { get;}
        RawPacketData WrappedProtocolRaw { get; }


    }
}
    