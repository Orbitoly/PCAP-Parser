using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using PcapParser.Protocols;

// ReSharper disable All

namespace PcapParser
{
    public static class PcapParser
    {
        private static List<RawPacketData> StreamToRawPackets(Stream pcapStream)
        {
            PcapHeader head;
            PcapPacketHeader tempPacketHeader;
            RawPacketData tempRawPacket;
            List<RawPacketData> lstRaw = new List<RawPacketData>();
            using (var reader = new MyBinaryReader(pcapStream, EndianEnum.BigEndian))
            {
                head = HeaderReader(reader);
                if (head != null) Console.WriteLine(head);
                try
                {
                    while (reader.CanRead(PcapPacketHeader.HeaderLength))
                    {
                        tempPacketHeader = PacketHeaderReader(head, reader);
                        if (reader.CanRead((int)tempPacketHeader.InclLen))
                        {
                            tempRawPacket = PacketDataReader(tempPacketHeader, head.LinkLayerProtocol, reader);
                            lstRaw.Add(tempRawPacket);
                        }
                        else
                        {
                            break;
                        }

                    }
                }
                catch (EndOfStreamException e)
                {
                    Console.WriteLine("Exception: " + e + "\n\nTried to read Stream and Failed.");
                }
            }
            return lstRaw;
        }

        public static string Parse(Stream pcapStream)
        {
            var lstRaw = StreamToRawPackets(pcapStream);
            var etherPac = new EthernetPacket(lstRaw.First().RawBytes);

            return etherPac.ToString();
        }

        private static PcapHeader HeaderReader(MyBinaryReader reader)
        {

            //---------------------- HEADER DATA ---------------------//
            // |magicNumber|versionMajor|versionMinor|thiszone|sigfigs|snaplen|network|DATA...
            //---------------- MAGIC NUMBER ENDIANNESS ---------------//
            uint BIG_ENDIAN_VAL = Convert.ToUInt32("a1b2c3d4", 16);
            uint LITTLE_ENDIAN_VAL = Convert.ToUInt32("d4c3b2a1", 16);
            //--------------------------------------------------------//

            uint magicNumber = BitConverter.ToUInt32(reader.ReadBytes(sizeof(uint)), 0);

            reader.Endian = magicNumber == BIG_ENDIAN_VAL ? EndianEnum.BigEndian : EndianEnum.LittleEndian;

            ushort versionMajor = BitConverter.ToUInt16(reader.ReadBytes(sizeof(ushort)), 0);
            ushort versionMinor = BitConverter.ToUInt16(reader.ReadBytes(sizeof(ushort)), 0);
            int thiszone = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)), 0);
            uint sigfigs = BitConverter.ToUInt32(reader.ReadBytes(sizeof(uint)), 0);
            uint snaplen = BitConverter.ToUInt32(reader.ReadBytes(sizeof(uint)), 0);
            uint network = BitConverter.ToUInt32(reader.ReadBytes(sizeof(uint)), 0);

            return new PcapHeader(magicNumber, versionMajor, versionMinor, thiszone, sigfigs, snaplen, network);
        }

        private static PcapPacketHeader PacketHeaderReader(PcapHeader header,MyBinaryReader reader)
        {
            
            uint ts_sec = BitConverter.ToUInt32(reader.ReadBytes(sizeof(uint)), 0);
            uint ts_usec = BitConverter.ToUInt32(reader.ReadBytes(sizeof(uint)), 0);
            uint incl_len = BitConverter.ToUInt32(reader.ReadBytes(sizeof(uint)), 0);
            uint orig_len = BitConverter.ToUInt32(reader.ReadBytes(sizeof(uint)), 0);

            return new PcapPacketHeader(ts_sec,ts_usec,incl_len,orig_len);
        }

        private static RawPacketData PacketDataReader(PcapPacketHeader packetHeader,ProtocolEnum etherType, MyBinaryReader reader)
        {
            //---------------------- TCP IP Packet ---------------------//
            var rawPacketDataBytes = reader.ReadBytes((int)packetHeader.InclLen);

            return new RawPacketData(rawPacketDataBytes,etherType);
        }


    }

}
