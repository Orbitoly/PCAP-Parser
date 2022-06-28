using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcapParser
{
    static class BitsOperations
    {
        public static readonly int BYTE_LEN = 8;

        public enum Direction
        {
            Right,
            Left
        }

        public static byte[] ReadBytesRange(byte[] data, int startIndex, int len)
        {
            return data.ToList().GetRange(startIndex, len).ToArray();
        }

        public static byte[] ReadBytesRange(List<byte> data, int len)
        {
            return data.GetRange(0, len).ToArray();
        }

        public static int ReadBitsFromByte(byte data, int len, Direction dir) //,bool fromLeft = true)
        {
            var mask = 0;
            if (len > BYTE_LEN)
                throw new Exception("len is too big.");
            if (dir == Direction.Right)
            {
                for (var i = 0; i < len; i++)
                {
                    mask += (int) Math.Pow(2, i);
                }
                return mask & data;
            }
            for (var i = BYTE_LEN - 1; i >= BYTE_LEN - len; i--)
            {
//Direction.Left
                mask += (int) Math.Pow(2, i);
            }
            return (mask & data) >> (BYTE_LEN - len);
        }

        /*
                 * 11101111 01001000
                 * [0]: 11101111
                 * [1]: 01001000
                 * 
                 */

        public static int ReadBitsFromByteInt32(List<byte> data, int len, Direction dir) // bool fromLeft = true)
        {
            if (len > data.Count * BYTE_LEN)
                throw new Exception("len is too big.");
            if (len > sizeof(int) * BYTE_LEN)
                throw new Exception("len is bigger than int");

            if (len < 0)
                throw new Exception("LEN IS TOO SMALL");

            data = data.Take((len + ((BYTE_LEN - len) % BYTE_LEN)) / BYTE_LEN).ToList();
                //work with only the necessary bytes
            int result = 0;
            if (dir == Direction.Right)
            {
                data.Reverse();
                int countShift = 0;
                foreach (var b in data)
                {
                    var lenToRead = len > BYTE_LEN ? BYTE_LEN : len;

                    result += (ReadBitsFromByte(b, lenToRead, dir)) << (BYTE_LEN * countShift);
                    len -= lenToRead;
                    countShift++;
                }
            }
            else
            {
//Direction.Left
                var temp = 0;
                foreach (var b in data)
                {
                    var lenToRead = len > BYTE_LEN ? BYTE_LEN : len;
                    temp = (ReadBitsFromByte(b, lenToRead, dir));

                    if (len > lenToRead)
                        temp = temp << (len - lenToRead);
                    result += temp;
                    len -= lenToRead;
                }
            }
            return result;
        }

        public static long ReadBitsFromByteInt64(List<byte> data, int len, Direction dir) // bool fromLeft = true)
        {
            if (len > data.Count * BYTE_LEN)
                throw new Exception("len is too big.");
            if (len > sizeof(long) * BYTE_LEN)
                throw new Exception("len is bigger than long");

            if (len < 0)
                throw new Exception("LEN IS TOO SMALL");

            data = data.Take((len + ((BYTE_LEN - len) % BYTE_LEN)) / BYTE_LEN).ToList();
                //work with only the necessary bytes
            long result = 0;
            if (dir == Direction.Right)
            {
                data.Reverse();
                int countShift = 0;
                foreach (var b in data)
                {
                    var lenToRead = len > BYTE_LEN ? BYTE_LEN : len;

                    result += (ReadBitsFromByte(b, lenToRead, dir)) << (BYTE_LEN * countShift);
                    len -= lenToRead;
                    countShift++;
                }
            }
            else
            {
//Direction.Left
                long temp = 0;
                foreach (var b in data)
                {
                    var lenToRead = len > BYTE_LEN ? BYTE_LEN : len;
                    temp = (ReadBitsFromByte(b, lenToRead, dir));

                    if (len > lenToRead)
                        temp = temp << (len - lenToRead);
                    result += temp;
                    len -= lenToRead;
                }
            }
            return result;
        }
    }
}