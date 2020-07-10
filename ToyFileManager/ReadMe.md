## OS Project : FileManagerSystem

### 文件管理系统

#### 项目分析

##### 项目描述

在内存中开辟一块区域来模拟磁盘,做一个文件管理系统, 管理该区域的已用空间和空闲空间

##### 开发环境

- 开发工具: VS  .NET
- 开发语言:C# 

##### 需求分析

- 能够对文件进行创建, 删除, 重命名等操作
- 提供文件操作接口, 在可视化界面上进行文件打开, 文件关闭, 文件写入等操作
- 对磁盘空闲空间进行管理
- 利用多级文件目录检索文件
- 能够进行磁盘格式化
- 能够将数据可持久化, 在下次打开时恢复原状

#### 实现方法

##### 文件物理结构

文件采用隐式链接方式存储, 即在每个磁盘块后面都留有一段空间, 用于存储下一个磁盘块的序列号, 以达到消除外碎片, 支持文件动态增长的功能

![image-20200710235739931](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200710235739931.png)

![image-20200710235755363](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200710235755363.png)

##### 空间管理

磁盘整体分为两部分: 系统区和用户区, 系统区包括位示图和FCB块, 用户区包括数据块, 本项目在设计过程中对各部分的大小进行了合理的取舍,最终结果如下:

- 磁盘每个块blocksize为1KB, 共有16*1024个块
- 位示图占据2KB, 即两个块, 对应16*1024个块
- FCB块占据256KB, 即256个块, 每个FCB大小为16B, 对应16*1024个FCB
- 数据块占据剩下的所有内存, 共有16*1024 - 258个块

![image-20200710235630114](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200710235630114.png)

![image-20200710235640834](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200710235640834.png)

###### 文件空间管理

文件空间管理采用FCB和数据块分离的方法, 提高搜索速度的同时不会浪费空间, 下面以创建文件为例, 

1. 首先查找是否有可用空间\
2. 判断当前目录对应的FCB块是否已满
3. 如果满了则需要分配一个新的FCB块, 装入该文件的FCB, 如果没满则直接创建FCB并填入
4. 为该文件分配数据块
5. 将数据写入数据块,如果一个块装不下需要向后延伸
6. 如果空间不足以装下整个数据, 则从中截断并返回

![image-20200711000529988](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200711000529988.png)

![image-20200711000009429](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200711000009429.png)

###### 空闲空间管理

空闲空间管理采用位示图方法, 当需要寻找一个空的磁盘块时, 会遍历位示图得到最前面的块,当释放某文件或目录时,也会归还空闲空间, 提高利用效率

![image-20200711000357068](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200711000357068.png)

##### 目录结构

目录采用多级目录结构, 高级目录的FCB所指向的内容还是FCB, 从而可以创建同名文件, 也为多用户提供了基础.

![image-20200711000549971](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200711000549971.png)

#### 成果展示

初始界面,

![image-20200711000633428](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200711000633428.png)

创建

![image-20200711000644730](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200711000644730.png)

打开文件/文件夹

![image-20200711000706660](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200711000706660.png)

![image-20200711003615959](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200711003615959.png)

![image-20200711003637115](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200711003637115.png)

写入文件

![image-20200711003650981](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200711003650981.png)

![image-20200711003709734](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200711003709734.png)

回退

![image-20200711003733365](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200711003733365.png)

删除文件

![image-20200711003749958](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200711003749958.png)

![image-20200711003755414](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200711003755414.png)

格式化

![image-20200711003810192](C:\Users\root\AppData\Roaming\Typora\typora-user-images\image-20200711003810192.png)