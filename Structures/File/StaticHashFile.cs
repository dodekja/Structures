using Structures.Interface;

namespace Structures.File
{
    public class StaticHashFile<T> : AbstractHashFile<T> where T : IData<T>, new()
    {
        public StaticHashFile(string fileName, int blockFactor, int numberOfBlocks)
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
        /// Initializes the StaticHashFile based on information from the binary file.
        /// </summary>
        /// <param name="fileName">Path to the file with data</param>
        public StaticHashFile(string fileName)
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

        public override void Insert(T data)
        {
            int address = _blockAddresses[data.GetHash() % _numberOfBlocks];
            Block<T> block = ReadBlock(address);
            block.AddRecord(data);
            SaveBlock(block, address);
        }

        public override void Delete(T data)
        {
            int address = _blockAddresses[data.GetHash() % _numberOfBlocks];
            Block<T> block = ReadBlock(address);
            block.RemoveRecord(data);
            SaveBlock(block, address);
        }

        public override T? Find(T data)
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

        protected override int SaveProperties()
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
        protected override int ReadProperties()
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

        public override void Dispose()
        {
            _file.Dispose();
        }
    }
}
