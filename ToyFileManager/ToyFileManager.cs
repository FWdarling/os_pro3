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
        public string fileName;
        public int firstBlockIndex;
        public int size;

        public FCB() { }
        
        public FCB(BitArray disk, int start)
        {
            if(disk[start] == true)
            {
                bUsed = disk[start];
                type = disk[1 + start];
                fileName = ToyFileManager.bitToString(disk, 2 + start, 12);
                firstBlockIndex = ToyFileManager.bitToInt(disk, 98 + start, 14);
                size = ToyFileManager.bitToInt(disk, 112 + start, 14);
            }
            else
            {
                throw new System.InvalidOperationException("try to create invalid FCB!");
            }
        }

        public FCB(bool tbused, bool ttype, string tfileName, int tfirstBlockIndex, int tsize)
        {
            bUsed = tbused;
            type = ttype;
            fileName = tfileName;
            firstBlockIndex = tfirstBlockIndex;
            size = tsize;
        }

        public void FCBWriteDisk(BitArray disk, int start)
        {
            disk[start] = bUsed;
            disk[start + 1] = type;
            ToyFileManager.stringToBit(disk, start + 2, fileName);
            ToyFileManager.IntToBit(disk, start + 98, firstBlockIndex, 14);
            ToyFileManager.IntToBit(disk, start + 112, size, 14);
        }

    }
    class ToyFileManager
    {
        public static int blockSize = 8 * 1024;
        public static int blockNum = 16 * 1024;
        public static int FCBBlockNum = 256;
        public static int bitMapBlockNum = 2;

        public Stack filePath = new Stack();
        public Stack FCBStack = new Stack();
        public BitArray disk = new BitArray(blockNum * blockSize + 100, false);
        FCB currentFCB;


        public ToyFileManager()
        {
            disk[0] = disk[1] = true; //前两个块存位图, 标记为被占用
            currentFCB = new FCB(true, true, "root", bitMapBlockNum + 1, 0);//根目录FCB赋值给当前文件
            disk[bitMapBlockNum] = true; //根目录FCB在位图之后的块, 标记为被占用
            currentFCB.FCBWriteDisk(disk, bitMapBlockNum * blockSize);//将根目录FCB写入对应的区域
            disk[bitMapBlockNum + 1] = true;//根目录的内容存到下一个空闲的系统区块中, 标记为被占用
            filePath.Push("root");
            FCBStack.Push(bitMapBlockNum * blockSize);

        }

        public void setDiskInit(int start, int length)
        {
            for(int i = 0; i < length; i++)
            {
                disk[i + start] = false;
            }
        }

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
        public static void stringToBit(BitArray disk, int start, string str)
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

        public static void IntToBit(BitArray disk, int start, int a, int bitLength)
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

        public bool isFCBEnd(int index)
        {
            int res = (index + 128) % blockSize;
            if (res == 0) return true;
            else return false;
        }

        public int findFCB(int FCBStart, string name)
        {
            if(disk[FCBStart] == false || disk[FCBStart + 1] == false)
                throw new System.InvalidOperationException("can't find a non existent floder!");
            FCB currFCB = new FCB(disk, FCBStart);
            int startIndex = currFCB.firstBlockIndex * blockSize;
            for(int i = 0; i < currFCB.size; i++)
            {
                string FCBName = bitToString(disk, startIndex + 2, 12);
                if (name == FCBName) return startIndex;
                //当前块已经搜完并且还有FCB存在另一个块中
                if (i >= 63)
                {
                    i -= 64;
                    //得到下一个该目录下的FCB
                    int nextBlockNum = bitToInt(disk, startIndex + blockSize - 14, 14);
                    startIndex = nextBlockNum * blockSize;
                }
                else {
                    startIndex += 126;
                }
            }
            //如果找不到 则返回-1
            return -1;
        }

        public string[] getFolderContent()
        {
            
            if(currentFCB.type = true)
            {
                string[] allContengName = new string[currentFCB.size];
                int startIndex = currentFCB.firstBlockIndex * blockSize;
                for (int i = 0; i < currentFCB.size; i++)
                {
                    string FCBName = bitToString(disk, startIndex + 2, 12);
                    allContengName[i] = FCBName;
                    //当前块已经搜完并且还有FCB存在另一个块中
                    if (i >= 64)
                    {
                        i -= 64;
                        //得到下一个该目录下的FCB
                        int nextBlockNum = bitToInt(disk, startIndex + blockSize - 14, 14);
                        startIndex = nextBlockNum * blockSize;
                    }
                    else
                    {
                        startIndex += 126;
                    }
                    
                }
                return allContengName;
            }
            else
            {
                throw new System.InvalidOperationException("can't read a non existent floder!");
            }
        }
        public string getFileContent()
        {
            if (currentFCB.type = false)
            {
                int blockFirstIndex = currentFCB.firstBlockIndex * blockSize;
                int size = currentFCB.size;
                string text = "";
                while (size > 0)
                {
                    int byteLen = bitToInt(disk, blockFirstIndex, 10);
                    string blockText = bitToString(disk, blockFirstIndex + 10, byteLen);
                    text += blockText;
                    int nextBlockNum = bitToInt(disk, blockFirstIndex + blockSize - 14, 14);
                    blockFirstIndex = nextBlockNum * blockSize;
                    size--;
                }
            }
            else
            {
                throw new System.InvalidOperationException("can't read a non existent file!");
            }
        }

        public int getNextFreeFCBBlock()
        {
            for(int i = 3; i < 258; i++)
            {
                if (disk[i] == false) return i;
            }
            return -1;
        }
        public int getNextFreeFileBlock()
        {
            for (int i = 259; i < 16 * 1024 - 1; i++)
            {
                if (disk[i] == false) return i;
            }
            return -1;
        }
    }
}
