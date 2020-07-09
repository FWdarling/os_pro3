using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyFileManager
{


    class FCB
    {
        public bool bUsed = false;
        public bool type = false;
        public string filename;
        public int firstBlockIndex;
        public int size;

        public FCB() { }
        
        public FCB(BitArray disk)
        {
            if(disk[0] == true)
            {
                type = disk[1];
                filename = ToyFileManager.bitToString(disk, 2, 12);
                firstBlockIndex = ToyFileManager.bitToInt(disk, 98, 14);
                size = ToyFileManager.bitToInt(disk, 112, 14);
            }
        }
    }
    class ToyFileManager
    {
        public static int blockSize = 8 * 1024;
        public static int blockNum = 16 * 1024;
        public static int FCBBlockNum = 261;
        public static int bitMapBlockNum = 2;

        public Stack filePath = new Stack();
        public Stack FCBStack = new Stack();
        public BitArray disk = new BitArray(blockNum * blockSize + 100, false);
        FCB currentFCB = new FCB();

        public static string bitToString(BitArray disk, int start, int stringLength)
        {
            Byte[] Asc = new byte[stringLength];
            for (int i = 0; i < stringLength; i++)
            {
                int binary = 0;
                int temp = 1;
                for (int j = 0; j < 8; j++)
                {
                    int currentBit = disk[start + i * 8 + j] ? 1 : 0;
                    binary |= (currentBit << (temp - 1));
                    temp++;
                }
                if (binary == 0) break;
                else
                {
                    Asc[i] = (byte)binary;
                }
            }
            string resString = Encoding.ASCII.GetString(Asc);
            return resString;
        }
        public void stringToBit(int start, string str)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(str);
            int len = str.Length;

            for (int i = 0; i < len; i++)
            {
                byte currentByte = byteArray[i];
                for(int j = 0; j < 8; j++)
                {
                    disk[start + i * 8 + j] = ((currentByte & 1) == 1);
                    currentByte >>= 1;
                }
            }
        }

        public static int bitToInt(BitArray disk, int start, int bitLength)
        {
            int res = 0;
            for(int i = 0; i < bitLength; i++)
            {
                int currentBit = disk[start + i] ? 1 : 0;
                res += (currentBit << i);
            }
            return res;
        }

        public void IntToBit(int start, int a, int bitLength)
        {
            BitArray res = new BitArray(bitLength);
            for(int i = 0; i < bitLength; i++)
            {
                disk[start + i] = ((a & 1) == 1);
                a >>= 1;
            }
        }


        public BitArray getSubBitArray(int start, int length)
        {
            BitArray subDisk = new BitArray(length);
            for(int i = 0; i < length; i++)
            {
                subDisk[i] = disk[start + i];
            }
            return subDisk;
        }

        public void setSubBitArray(int start, BitArray array)
        {
            int len = array.Length;
            for (int i = 0; i < len; i++)
            {
                disk[start + i] = array[i];
            }
        }
    }
}
