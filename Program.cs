using System;
using System.IO;
namespace PcapParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"./snifsnif.pcap";

            using (FileStream pcap_file = File.OpenRead(filePath))
            {
                var parsedPacket = PcapParser.Parse(pcap_file);
                Console.WriteLine(parsedPacket);
            }
        }
    }
}