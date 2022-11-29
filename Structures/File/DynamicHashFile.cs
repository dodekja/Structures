using Structures.Interface;
using Structures.Tree;
using System.Collections;

namespace Structures.File
{
    public class DynamicHashFile<T> : AbstractHashFile<T> where T : IData<T>, new()
    {
        private Trie<T> _index;

        private List<int> _blockAddresses;

        /// <summary>
        /// List of _blockAddresses indexes that point to empty blocks;
        /// </summary>
        private List<int> _emptyBlocks;

        public DynamicHashFile(string fileName, int blockFactor)
        {
            _blockFactor = blockFactor;
            _index = new Trie<T>(_blockFactor);
            _blockAddresses = new List<int>();
            _emptyBlocks = new List<int>();
            _numberOfBlocks = 1;
            _file = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Block<T> block = new Block<T>(_blockFactor);
            _blockSize = block.GetSize();
            int address = SaveProperties();
            _blockAddresses.Add(address);
            _index.Root.BlockAddress = address;
            _index.Root.NumberOfItems = 0;
            SaveBlock(block, address);
        }

        public override void Insert(T data)
        {
            int address = _index.Find(data);
            Block<T> block = ReadBlock(address);
            block.AddRecord(data);
            int? depthOfSons = _index.AddItem(data);
            if (depthOfSons != null)
            {
                Block<T> newBlock = AddNewBlock();
                int depth = depthOfSons.Value;
                bool splitSuccessFully = false;
                int newBlockAddress;
                if (_emptyBlocks.Count == 0)
                {
                    newBlockAddress = _blockAddresses.Last() + _blockSize;
                    _blockAddresses.Add(_blockAddresses.Last() + _blockSize);
                }
                else
                {
                    newBlockAddress = _emptyBlocks.Min();
                }
                while (!splitSuccessFully)
                {
                    depth++;
                    splitSuccessFully = SplitBlock(block, newBlock, depth);
                    _index.Split(block.GetFirstItem(), newBlock.GetFirstItem(), block.ValidCount, address, newBlock.ValidCount, newBlockAddress);
                }

                SaveBlock(newBlock, newBlockAddress);
            }

            SaveBlock(block, address);
        }

        private bool SplitBlock(Block<T> block, Block<T> newBlock, int depth)
        {
            bool success = false;

            List<T> validItems;
            if (block.ValidCount == 0)
            {
                validItems = newBlock.GetValidItems();
                newBlock.ClearRecords();
            }
            else
            {
                validItems = block.GetValidItems();
                block.ClearRecords();
            }

            foreach (var record in validItems)
            {
                BitArray keyBits = new BitArray(BitConverter.GetBytes(record.GetHash()));
                if (keyBits[depth - 1])
                {
                    newBlock.AddRecord(record);
                    _index.AddItem(record);
                }
                else
                {
                    block.AddRecord(record);
                    _index.AddItem(record);
                }
            }

            if (block.ValidCount != 0 || newBlock.ValidCount != 0)
            {
                success = true;
            }

            return success;
        }

        private Block<T> AddNewBlock()
        {
            if (_emptyBlocks.Count == 0)
            {
                return new Block<T>(_blockFactor);
            }

            return ReadBlock(_emptyBlocks.Min());
        }

        public override void Delete(T data)
        {
            int address = _index.Find(data);
            Block<T> block = ReadBlock(address);
            block.RemoveRecord(data);
            (int, int)? addresses = _index.Remove(data);
            if (addresses != null)
            {
                int brotherAddress = addresses.Value.Item1 == address ? addresses.Value.Item2 : addresses.Value.Item1;
                Block<T> brotherBlock = ReadBlock(brotherAddress);
                var save = JoinBlocks(block, address, brotherBlock, brotherAddress);
                SaveBlock(save.Item1, save.Item2);
            }
            else
            {
                SaveBlock(block, address);
            }
        }

        public (Block<T>, int) JoinBlocks(Block<T> firstBlock, int firstBlockAddress, Block<T> secondBlock, int secondBlockAddress)
        {
            int keepAddress = firstBlockAddress < secondBlockAddress ? firstBlockAddress : secondBlockAddress;
            Block<T> delBlock = firstBlockAddress < secondBlockAddress ? secondBlock : firstBlock;
            Block<T> keepBlock = firstBlockAddress < secondBlockAddress ? firstBlock : secondBlock;
            var items = delBlock.GetValidItems();
            foreach (T item in items)
            {
                keepBlock.AddRecord(item);
            }
            return (keepBlock, keepAddress);
        }

        public override T? Find(T data)
        {
            int address = _index.Find(data);
            Block<T> block = ReadBlock(address);
            var ret = block.FindRecord(data);
            return ret;

        }

        protected override int ReadProperties()
        {
            int address = 0;
            _file.Seek(address, SeekOrigin.Begin);
            byte[] bytes = new byte[sizeof(int)];
            _ = _file.Read(bytes, 0, sizeof(int));
            _blockFactor = BitConverter.ToInt32(bytes, 0);
            return address;
        }

        protected override int SaveProperties()
        {
            int address = 0;
            _file.Seek(address, SeekOrigin.Begin);
            byte[] bytes = BitConverter.GetBytes(_blockFactor);
            _file.Write(bytes, 0, bytes.Length);
            return address;
        }

        public override void Dispose()
        {
            _file.Dispose();
        }
    }
}
