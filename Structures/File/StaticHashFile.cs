﻿using Structures.Interface;

namespace Structures.File
{
    public class StaticHashFile<T> : AbstractHashFile<T> where T : IData<T>, new()
    {
        public StaticHashFile(string fileName, int blockFactor, int numberOfBlocks) : base(fileName, blockFactor)
        {
            _numberOfBlocks = numberOfBlocks;
            Block<T> block = new Block<T>(blockFactor);
            _blockSize = block.GetSize();

            SaveProperties();

            int address = 2 * sizeof(int);

            for (int index = 0; index < numberOfBlocks; index++)
            {
                SaveBlock(block,address);
                address += _blockSize;
            }
        }

        /// <summary>
        /// Initializes the StaticHashFile based on information from the binary file.
        /// </summary>
        /// <param name="fileName">Path to the file with data</param>
        public StaticHashFile(string fileName) : base(fileName)
        {
            ReadProperties();
            Block<T> block = new Block<T>(_blockFactor);
            _blockSize = block.GetSize();
        }

        public override void Insert(T data)
        {
            int address = ComputeBlockAddress(data);
            Block<T> block = ReadBlock(address);
            block.AddRecord(data);
            SaveBlock(block, address);
        }

        public override T Delete(T data)
        {
            int address = ComputeBlockAddress(data);
            Block<T> block = ReadBlock(address);
            T ret = block.RemoveRecord(data);
            SaveBlock(block, address);
            return ret;
        }

        public override T? Find(T data)
        {
            Block<T> block = ReadBlock(ComputeBlockAddress(data));
            var ret = block.FindRecord(data);
            return ret;
        }

        public string GetBlockContents(T data)
        {
            Block<T> block = ReadBlock(ComputeBlockAddress(data));
            return block.GetContentsString();
        }

        protected override void SaveProperties()
        {
            base.SaveProperties();

            int address = sizeof(int);
            byte[] bytes = BitConverter.GetBytes(_numberOfBlocks);
            _file.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Reads block factor and number of blocks from binary file.
        /// </summary>
        /// <returns>Current position in the file after reading property bytes.</returns>
        protected override void ReadProperties()
        {
            base.ReadProperties();

            byte[] bytes = new byte[sizeof(int)];
            _ = _file.Read(bytes, 0, sizeof(int));
            _numberOfBlocks = BitConverter.ToInt32(bytes, 0);
        }

        protected override int ComputeBlockAddress(T data)
        {
            return 2 * sizeof(int) + data.GetHash() % _numberOfBlocks * _blockSize;
        }

        public override (T record, int index, int blockAddress) GetRecordForUpdate(T data)
        {
            int blockAddress = ComputeBlockAddress(data);
            Block<T> block = ReadBlock(blockAddress);
            var blockUpdateInfo = block.GetRecordForUpdate(data);
            
            if (blockUpdateInfo == null)
            {
                throw new InvalidOperationException($"Item {data} was not found.");
            }

            return (blockUpdateInfo.Value.record, blockUpdateInfo.Value.index, blockAddress);

        }

        public override void UpdateRecord(T data, int index, int blockAddress)
        {
            int address = ComputeBlockAddress(data);
            if (blockAddress == address)
            {
                Block<T> block = ReadBlock(blockAddress);
                block.UpdateRecord(data,index);
                SaveBlock(block,address);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Actual block address {address} for record {data} is different from the original address {blockAddress}.");
            }
        }

        public override string GetAllBlockContents()
        {
            string contents = "";
            for (int i = 0; i < _numberOfBlocks; i++)
            {
                contents += GetValidBlockContents(i, sizeof(int)*2);
                contents += "************************\n";
            }
            return contents;
        }
    }
}
