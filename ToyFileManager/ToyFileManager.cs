using System;
using System.Collections;
using System.Text;


namespace ToyFileManager
{


    class FCB
    {
        public bool bUsed = false;
        public bool type = false;
        public string fileName;
        public int firstBlockNum;
        public int size;

        public FCB() { }
        
        public FCB(BitArray disk, int start)
        {
            if(disk[start] == true)
            {
                bUsed = disk[start];
                type = disk[1 + start];
                fileName = ToyFileManager.bitToString(disk, 2 + start, 12);
                firstBlockNum = ToyFileManager.bitToInt(disk, 98 + start, 14);
                size = ToyFileManager.bitToInt(disk, 112 + start, 14);
            }
            else
            {
                throw new System.InvalidOperationException("try to create invalid FCB!");
            }
        }

        public FCB(bool tbused, bool ttype, string tfileName, int tfirstBlockNum, int tsize)
        {
            bUsed = tbused;
            type = ttype;
            fileName = tfileName;
            firstBlockNum = tfirstBlockNum;
            size = tsize;
        }

        public void FCBWriteDisk(BitArray disk, int start)
        {
            disk[start] = bUsed;
            disk[start + 1] = type;
            ToyFileManager.stringToBit(disk, start + 2, 0, fileName.Length, fileName);
            ToyFileManager.IntToBit(disk, start + 98, firstBlockNum, 14);
            ToyFileManager.IntToBit(disk, start + 112, size, 14);
        }

    }
    class ToyFileManager
    {
        public static int blockSize = 8 * 1024;
        public static int blockNum = 16 * 1024;
        public static int FCBBlockNum = 256;
        public static int bitMapBlockNum = 2;

        ArrayList filePath = new ArrayList();
        public Stack FCBStack = new Stack();
        public BitArray disk = new BitArray(blockNum * blockSize + 100, false);
        FCB currentFCB;
        int currentFCBFirstIndex;


        public ToyFileManager()
        {
            disk[0] = disk[1] = true; //前两个块存位图, 标记为被占用
            currentFCB = new FCB(true, true, "root", bitMapBlockNum + 1, 0);//根目录FCB赋值给当前文件
            currentFCBFirstIndex = bitMapBlockNum * blockSize;
            disk[bitMapBlockNum] = true; //根目录FCB在位图之后的块, 标记为被占用
            currentFCB.FCBWriteDisk(disk, currentFCBFirstIndex);//将根目录FCB写入对应的区域
            disk[bitMapBlockNum + 1] = true;//根目录的内容存到下一个空闲的系统区块中, 标记为被占用
            filePath.Add("root");
            FCBStack.Push(currentFCBFirstIndex);

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
        public static void stringToBit(BitArray disk, int diskStart, int stringStart, int len, string str)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(str);

            for (int i = 0; i < len; i++)
            {
                byte currentByte = byteArray[stringStart + i];
                for(int j = 0; j < 8; j++)
                {
                    disk[diskStart + i * 8 + j] = ((currentByte & 1) == 1);
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
            int startIndex = currFCB.firstBlockNum * blockSize;
            for(int i = 0; i < currFCB.size; i++)
            {
                string FCBName = bitToString(disk, startIndex + 2, 12);
                if (name == FCBName) return startIndex;
                //当前块已经搜完并且还有FCB存在另一个块中
                if (i >= 63)
                {
                    i -= 64;
                    //得到下一个该目录下的FCB
                    int nextBlockNum = bitToInt(disk, startIndex + 128 - 14, 14);
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
            
            if(currentFCB.type == true)
            {
                string[] allContengName = new string[currentFCB.size];
                int startIndex = currentFCB.firstBlockNum * blockSize;
                for (int i = 0; i < currentFCB.size; i++)
                {
                    string FCBName = bitToString(disk, startIndex + 2, 12);
                    allContengName[i] = FCBName;
                    //当前块已经搜完并且还有FCB存在另一个块中
                    if (i >= 64)
                    {
                        i -= 64;
                        //得到下一个该目录下的FCB
                        int nextBlockNum = bitToInt(disk, startIndex + 128 - 14, 14);
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
            if (currentFCB.type == false)
            {
                int blockFirstIndex = currentFCB.firstBlockNum * blockSize;
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
                return text;
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



        public bool createFolder(string fileName)
        {
            if (fileName.Length > 12 || !currentFCB.type) return false;
            int findFCBFirstIndex = findFCB(currentFCBFirstIndex, fileName);
            if (findFCBFirstIndex != -1) return false; //出现重名, 拒绝创建请求
            int size = currentFCB.size;

            int firstIndexToWrite = currentFCB.firstBlockNum * blockSize;
            int newFolderBlock;
            //当前块已满且还有下一块存有当前目录FCL
            while (size > 64)
            {
                size -= 64;
                int nextBlockNum = bitToInt(disk, firstIndexToWrite + blockSize - 14, 14);
                firstIndexToWrite = nextBlockNum * blockSize;
            }
            //当前块已满且没有下一块
            if(size == 64)
            {
                int nextBlockNum = getNextFreeFCBBlock();
                if (nextBlockNum == -1) return false; //无空闲FCB块装入新目录的FCB
                disk[nextBlockNum] = true;
                newFolderBlock = getNextFreeFCBBlock();
                if (newFolderBlock == -1)
                {
                    disk[nextBlockNum] = false;
                    return false; //无空闲FCB块装入新目录的内容
                }
                firstIndexToWrite = nextBlockNum * blockSize;
                size = 0;
            }

            firstIndexToWrite += size * 126;
            newFolderBlock = getNextFreeFCBBlock();
            if (newFolderBlock == -1) return false; //无空闲FCB块装入新目录的内容
            disk[newFolderBlock] = true;
            FCB newFolderFCB = new FCB(true, true, fileName, newFolderBlock, 0);//创建新目录的FCB
            newFolderFCB.FCBWriteDisk(disk, firstIndexToWrite);
            currentFCB.size++;
            currentFCB.FCBWriteDisk(disk, currentFCBFirstIndex);
            return true;
        }
        public void delete(int FCBFirstIndex)
        {
            FCB FCBToDelete = new FCB(disk, FCBFirstIndex);
            if(FCBToDelete.type == true)
            {
                //如果是目录, 需要递归删除目录中所有子目录和文件
                int subFCBFirstIndex = FCBToDelete.firstBlockNum * blockSize;
                for (int i = 0; i < FCBToDelete.size; i++)
                {
                    delete(subFCBFirstIndex);
                    subFCBFirstIndex += 126;
                    if(((i + 1) % 64) == 0)
                    {
                        int nextBlockNum = bitToInt(disk, subFCBFirstIndex + 128 - 14, 14);
                        subFCBFirstIndex = nextBlockNum * blockSize;
                    }
                }
                //对FCB宣称占有的空间进行清除
                int size = FCBToDelete.size;
                int subFCBBlockNum = FCBToDelete.firstBlockNum;
                while (size > 0)
                {
                    //先记录下一个块的序号
                    int nextBlockNum = bitToInt(disk, (subFCBBlockNum + 1) * blockSize - 14, 14);
                    setDiskInit(subFCBBlockNum * blockSize, blockSize);
                    disk[subFCBBlockNum] = false; //维护位示图
                    subFCBBlockNum = nextBlockNum;
                    size -= 64;
                }

                setDiskInit(FCBFirstIndex + 112, 14);//将父目录的size置为0
            }
            else
            {
                //同上
                int fileSize = FCBToDelete.size;
                int fileBlockNum = FCBToDelete.firstBlockNum;
                while(fileSize > 0)
                {
                    int nextBlockNum = bitToInt(disk, (fileBlockNum + 1) * blockSize - 14, 14);
                    setDiskInit(fileBlockNum * blockSize, blockSize);
                    disk[fileBlockNum] = false;
                    fileBlockNum = nextBlockNum;
                    fileSize--;
                }
                setDiskInit(FCBFirstIndex + 112, 14);
            }
        }
        public bool delete(string FCBName)
        {
            if (FCBName.Length > 12 || !currentFCB.type) return false;
            int findFCBFirstIndex = findFCB(currentFCBFirstIndex, FCBName);
            if (findFCBFirstIndex == -1) return false;//未找到要删除的文件
            delete(findFCBFirstIndex);
            int FCBMoveToFirstIndex = findFCBFirstIndex;
            while (disk[FCBMoveToFirstIndex])
            {
                int FCBToMoveIndex = FCBMoveToFirstIndex + 126;
                if(isFCBEnd(FCBToMoveIndex)){
                    int nextBlockNum = bitToInt(disk, FCBToMoveIndex + 128 - 14, 14);
                    FCBToMoveIndex = nextBlockNum * blockSize;
                }
                BitArray FCBToMove = getSubBitArray(FCBToMoveIndex, 126);
                setSubBitArray(FCBMoveToFirstIndex, FCBToMove);
                FCBMoveToFirstIndex = FCBToMoveIndex;
            }
            return true;
        }
        public void backUp()
        {
            if (FCBStack.Count <= 1) return;
            FCBStack.Pop();
            currentFCBFirstIndex = (int)FCBStack.Peek();
            currentFCB = new FCB(disk, currentFCBFirstIndex);
            filePath.RemoveAt(filePath.Count - 1);
        }
        public bool rename(string name, string newName)
        {
            if (newName.Length > 12 || !currentFCB.type) return false;
            int newNameFCBIndex = findFCB(currentFCBFirstIndex, newName);
            if (newNameFCBIndex != -1) return false; //出现重名
            int nameFcbIndex = findFCB(currentFCBFirstIndex, name);
            if (nameFcbIndex == -1) return false; //未找到文件
            stringToBit(disk, nameFcbIndex + 2 , 0, newName.Length, newName);
            return true;
        }
        public bool openFile(string fileName)
        {
            if (fileName.Length > 12 || !currentFCB.type) return false;
            int findFCBFirstIndex = findFCB(currentFCBFirstIndex, fileName);
            if (findFCBFirstIndex == -1) return false; //未找到文件
            currentFCBFirstIndex = findFCBFirstIndex;
            currentFCB = new FCB(disk, currentFCBFirstIndex);
            filePath.Add(currentFCB.fileName);
            FCBStack.Push(currentFCBFirstIndex);
            return true;
        }
        public void clearFileBlock()
        {
            if (currentFCB.type) return;
            int blockNumToClear = currentFCB.firstBlockNum;
            int size = currentFCB.size;

            while(size > 0)
            {
                int nextBlockNum = 0;
                if(size > 1021)
                {
                    nextBlockNum = bitToInt(disk, (blockNumToClear + 1) * blockSize - 14, 14);
                    disk[nextBlockNum] = false;
                }
                setDiskInit(blockNumToClear * blockSize, blockSize);
                blockNumToClear = nextBlockNum;
                size -= 1021;
            }
            currentFCB.size = 0;
            currentFCB.FCBWriteDisk(disk, currentFCBFirstIndex);
        }
        public bool writeFile(string content)
        {
            if (currentFCB.type) return false;
            clearFileBlock();

            int contentByte = content.Length;//剩余未写入磁盘的字节数
            const int blockFilebyte = 1021;//一个数据块最多能写入的字节数
            int stringstart = 0;//下一次要写入的数据在原字符串位置
            int blockIndexToWrite = currentFCB.firstBlockNum * blockSize;
            disk[currentFCB.firstBlockNum] = true;

            while (contentByte > 0)
            {
                
                int blockLength = contentByte > blockFilebyte ? blockFilebyte : contentByte; //要写入该数据块的字节数
                
                IntToBit(disk, blockIndexToWrite, blockLength, 10);
                stringToBit(disk, blockIndexToWrite + 10, stringstart, blockLength, content);

                contentByte -= 1021;
                stringstart += 1021;

                currentFCB.size++;
                currentFCB.FCBWriteDisk(disk, currentFCBFirstIndex);

                if(contentByte > 0)
                {
                    int nextBlockNum = getNextFreeFileBlock();
                    if (nextBlockNum == -1) return false;//可用空间不足
                    IntToBit(disk, blockIndexToWrite + blockSize - 14, nextBlockNum, 14);
                    blockIndexToWrite = nextBlockNum * blockSize;
                    disk[nextBlockNum] = true;
                }
            }
            return true;
        }

        public void format()
        {
            while(FCBStack.Count > 1)
            {
                FCBStack.Pop();
            }
            int rootFCBFirstIndex = (int)FCBStack.Peek();
            delete(rootFCBFirstIndex);
            disk[bitMapBlockNum + 1] = true;
            currentFCBFirstIndex = rootFCBFirstIndex;
            currentFCB = new FCB(disk, currentFCBFirstIndex);
            filePath.Clear();
            filePath.Add("root");
        }
    }
}