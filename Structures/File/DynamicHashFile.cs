using Structures.Interface;
using Structures.Tree;
using System.Collections;
using System.Text;

namespace Structures.File
{
    public class DynamicHashFile<T> : AbstractHashFile<T> where T : IData<T>, new()
    {
        private Trie<T> _index;

        /// <summary>
        /// List of _blockAddresses indexes that point to empty blocks;
        /// </summary>
        private List<int> _emptyBlocks;

        private string _fileName;

        public DynamicHashFile(string fileName, int blockFactor) : base(fileName, blockFactor)
        {
            _index = new Trie<T>(_blockFactor);
            _emptyBlocks = new List<int>();
            Block<T> block = new Block<T>(_blockFactor);
            _blockSize = block.GetSize();
            SaveProperties();
            SaveBlock(block, sizeof(int));
            _numberOfBlocks = 1;
            _fileName = fileName;
        }

        public DynamicHashFile(string fileName, string indexFileName) : base(fileName)
        {
            _fileName = fileName;
            ReadProperties();
            _index = new Trie<T>(_blockFactor);
            _numberOfBlocks = LoadIndex(indexFileName);
            _emptyBlocks = new List<int>();
            Block<T> block = new Block<T>(_blockFactor);
            _blockSize = block.GetSize();
            _fileName = fileName;
        }

        public override void Insert(T data)
        {
            int address = _index.Find(data);
            Block<T> block;
            if (address == -1)
            {
                block = AddNewBlock();
                if (_emptyBlocks.Count == 0)
                {
                    address = ComputeBlockAddress(_numberOfBlocks, sizeof(int));
                }
                else
                {
                    address = _emptyBlocks.Min();
                    _emptyBlocks.Remove(address);
                }

                _numberOfBlocks++;
                _index.AddBlock(data, address, 1);
            }
            else
            {
                block = ReadBlock(address);
            }
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
                    newBlockAddress = ComputeBlockAddress(_numberOfBlocks, sizeof(int));
                }
                else
                {
                    newBlockAddress = _emptyBlocks.Min();
                    _emptyBlocks.Remove(newBlockAddress);
                }
                _numberOfBlocks++;

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

        public override T Delete(T data)
        {
            int address = _index.Find(data);
            Block<T> block = ReadBlock(address);
            T ret = block.RemoveRecord(data);
            (int, int)? addresses = _index.Remove(data);
            if (addresses != null)
            {
                int brotherAddress = addresses.Value.Item1 == address ? addresses.Value.Item2 : addresses.Value.Item1;
                Block<T> brotherBlock = ReadBlock(brotherAddress);
                var delete = JoinBlocks(block, address, brotherBlock, brotherAddress);
                SaveBlock(brotherBlock, brotherAddress);
                SaveBlock(block, address);

                if (delete.Item2 == ComputeBlockAddress(_numberOfBlocks - 1, sizeof(int)))
                {
                    TrimEmptyBlocks();
                }

                return ret;
            }
            SaveBlock(block, address);
            return ret;
        }

        private void TrimEmptyBlocks()
        {
            while (ReadBlock(ComputeBlockAddress(_numberOfBlocks - 1, sizeof(int))).ValidCount == 0)
            {
                _file.SetLength(_file.Length - _blockSize);
                _numberOfBlocks--;
            }
        }

        public (Block<T>, int) JoinBlocks(Block<T> firstBlock, int firstBlockAddress, Block<T> secondBlock, int secondBlockAddress)
        {
            int delAddress = firstBlockAddress < secondBlockAddress ? secondBlockAddress : firstBlockAddress;
            Block<T> delBlock = firstBlockAddress < secondBlockAddress ? secondBlock : firstBlock;
            Block<T> keepBlock = firstBlockAddress < secondBlockAddress ? firstBlock : secondBlock;
            var items = delBlock.GetValidItems();
            foreach (T item in items)
            {
                keepBlock.AddRecord(item);
            }
            delBlock.ClearRecords();
            return (delBlock, delAddress);
        }

        public override T? Find(T data)
        {
            int address = _index.Find(data);
            Block<T> block = ReadBlock(address);
            var ret = block.FindRecord(data);
            return ret;

        }

        protected override void ReadProperties()
        {
            base.ReadProperties();
        }

        protected override void SaveProperties()
        {
            base.SaveProperties();
        }

        public string GetAllBlockContents()
        {
            string contents = "";
            for (int i = 0; i < _numberOfBlocks; i++)
            {
                contents += GetValidBlockContents(i, sizeof(int));
                contents += "************************\n";
            }
            return contents;
        }

        public void SaveIndex(string filename)
        {
            _index.Save(filename);
            StringBuilder builder = new();
            foreach (int emptyBlock in _emptyBlocks)
            {
                builder.Append($"{emptyBlock}\n");
            }
            System.IO.File.WriteAllText($"{filename}_empty.txt",builder.ToString());
        }

        public int LoadIndex(string filename)
        {
            string[] emptyBlockAddresses = System.IO.File.ReadAllLines($"{filename}_empty.txt");
            foreach (string emptyBlockAddress in emptyBlockAddresses)
            {
                _emptyBlocks.Add(int.Parse(emptyBlockAddress));
            }

            return _index.Load(filename);
        }
    }
}
