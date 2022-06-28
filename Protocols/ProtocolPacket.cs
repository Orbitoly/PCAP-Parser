using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PcapParser.Protocols;

namespace PcapParser
{
    abstract class ProtocolPacket
    {
        protected ProtocolPacket(byte[] data)
        {
            RawPacketData dataNext = Parse(data);
            if (dataNext != null)
                EncapsulatedPacket = ProtocolPacketFactory.BuildProtocolPacket(dataNext);
        }
        //public abstract override string ToString();

        /// <summary>
        /// When overriding - extract data to your properties, and then use the factory to return the encapsulated packet.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected abstract RawPacketData Parse(byte[] data);
        public abstract bool Valid { get; set; }
        public ProtocolPacket EncapsulatedPacket { get; }
        public bool HasEncapsulatedPacket => EncapsulatedPacket != null;

        public sealed override string ToString()
        {
            var str = "\n\n" + this.GetAttributes()+"\n";
            var next = this;
            while (next.HasEncapsulatedPacket)
            {
                next=next.EncapsulatedPacket;
                str += next.GetAttributes()+"\n";

            }
            str += "\n\n}\n";
            return str;

        }

        public abstract string GetAttributes();
        public bool IsAllowedEncapsulated(ProtocolEnum name)
        {
            return GetSupportedPorotocls().Contains(name);
        }
        protected abstract List<ProtocolEnum> GetSupportedPorotocls();
    }
}
