﻿using Structures.Interface;

namespace Structures.File
{
    public class HashFile<T> : IDisposable where T : IData<T>, new()
    {
        private int _blockFactor;

        private int _blockSize;

        private int _numberOfBlocks;

        private FileStream _file;

        private int[] _blockAddresses;

        public HashFile(string fileName, int blockFactor, int numberOfBlocks)
        {
            _blockFactor = blockFactor;
            _numberOfBlocks = numberOfBlocks;
            _file = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            _blockAddresses = new int[numberOfBlocks];
            Block<T> block = new Block<T>(blockFactor);
            _blockSize = block.GetSize();

            int address = SaveProperties();

            for (int index = 0; index < numberOfBlocks; index++)
            {
                _blockAddresses[index] = address;
                SaveBlock(block,address);
                address += _blockSize;
            }
        }

        /// <summary>
        /// Initializes the HashFile based on information from the binary file.
        /// </summary>
        /// <param name="fileName">Path to the file with data</param>
        public HashFile(string fileName)
        {
            _file = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            int address = ReadProperties();
            _blockAddresses = new int[_numberOfBlocks];
            Block<T> block = new Block<T>(_blockFactor);
            _blockSize = block.GetSize();
            for (int index = 0; index < _numberOfBlocks; index++)
            {
                _blockAddresses[index] = address;
                address += _blockSize;
            }
        }

        public void Insert(T data)
        {
            int address = _blockAddresses[data.GetHash() % _numberOfBlocks];
            Block<T> block = ReadBlock(address);
            block.AddRecord(data);
            SaveBlock(block, address);
        }

        public void Delete(T data)
        {
            int address = _blockAddresses[data.GetHash() % _numberOfBlocks];
            Block<T> block = ReadBlock(address);
            block.RemoveRecord(data);
            SaveBlock(block, address);
        }

        public T? Find(T data)
        {
            int address = _blockAddresses[data.GetHash() % _numberOfBlocks];
            Block<T> block = ReadBlock(address);
            var ret = block.FindRecord(data);
            return ret;
        }

        public string GetBlockContents(T data)
        {
            int address = _blockAddresses[data.GetHash() % _numberOfBlocks];
            Block<T> block = ReadBlock(address);
            return block.GetContentsString();
        }

        public string GetBlockContents(int addressIndex)
        {
            int address = _blockAddresses[addressIndex];
            Block<T> block = ReadBlock(address);
            return block.GetContentsString();
        }

        private void SaveBlock(Block<T> block, int blockAddress)
        {
            _file.Seek(blockAddress, SeekOrigin.Begin);
            byte[] blockBytes = block.ToByteArray();
            _file.Write(blockBytes, 0,blockBytes.Length);
        }

        private Block<T> ReadBlock(int blockAddress)
        {
            _file.Seek(blockAddress, SeekOrigin.Begin);
            byte[] blockBytes = new byte[_blockSize];
            _ = _file.Read(blockBytes, 0, _blockSize);
            Block<T> block = new Block<T>(_blockFactor);
            block.FromByteArray(blockBytes);
            return block;
        }

        private int SaveProperties()
        {
            int address = 0;
            _file.Seek(address, SeekOrigin.Begin);
            byte[] bytes = BitConverter.GetBytes(_blockFactor);
            _file.Write(bytes, 0, bytes.Length);

            address += sizeof(int);
            bytes = BitConverter.GetBytes(_numberOfBlocks);
            _file.Write(bytes, 0, bytes.Length);
            address += sizeof(int);

            return address;
        }

        /// <summary>
        /// Reads block factor and number of blocks from binary file.
        /// </summary>
        /// <returns>Current position in the file after reading property bytes.</returns>
        private int ReadProperties()
        {
            int address = 0;
            _file.Seek(address, SeekOrigin.Begin);
            byte[] bytes = new byte[sizeof(int)];
            _ = _file.Read(bytes, 0, sizeof(int));
            _blockFactor = BitConverter.ToInt32(bytes, 0);

            address += sizeof(int);
            bytes = new byte[sizeof(int)];
            _ = _file.Read(bytes, 0, sizeof(int));
            _numberOfBlocks = BitConverter.ToInt32(bytes, 0);
            address += sizeof(int);

            return address;
        }

        public void Dispose()
        {
            _file.Dispose();
        }
    }
}
