using Structures.Interface;
using System.Xml.Linq;

namespace Structures.File
{
    public class AbstractHashFile<T> : IDisposable where T : IData<T>, new()
    {
        public AbstractHashFile(string fileName, int blockFactor)
        {
            _blockFactor = blockFactor;
            _file = new FileStream($"{fileName}.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            _file.SetLength(0);
            Block<T> block = new Block<T>(_blockFactor);
            _blockSize = block.GetSize();
        }

        public AbstractHashFile(string fileName)
        {
            _file = new FileStream($"{fileName}.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Block<T> block = new Block<T>(_blockFactor);
            _blockSize = block.GetSize();
        }

        /// <summary>
        /// Max number of items in a block.
        /// </summary>
        protected int _blockFactor;

        /// <summary>
        /// Size of a block in bytes.
        /// </summary>
        protected int _blockSize;

        protected int _numberOfBlocks;

        protected FileStream _file;

        public virtual void Insert(T data)
        {
            throw new NotSupportedException();
        }

        public virtual void Delete(T data)
        {
            throw new NotSupportedException();
        }

        public virtual T? Find(T data)
        {
            throw new NotSupportedException();
        }

        protected virtual void ReadProperties()
        {
            _file.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[sizeof(int)];
            _ = _file.Read(bytes, 0, sizeof(int));
            _blockFactor = BitConverter.ToInt32(bytes, 0);
        }

        protected virtual void SaveProperties()
        {
            _file.Seek(0, SeekOrigin.Begin);
            byte[] bytes = BitConverter.GetBytes(_blockFactor);
            _file.Write(bytes, 0, bytes.Length);
        }

        protected void SaveBlock(Block<T> block, int blockAddress)
        {
            _file.Seek(blockAddress, SeekOrigin.Begin);
            byte[] blockBytes = block.ToByteArray();
            _file.Write(blockBytes, 0, blockBytes.Length);
        }

        protected Block<T> ReadBlock(int blockAddress)
        {
            _file.Seek(blockAddress, SeekOrigin.Begin);
            byte[] blockBytes = new byte[_blockSize];
            _ = _file.Read(blockBytes, 0, _blockSize);
            Block<T> block = new Block<T>(_blockFactor);
            block.FromByteArray(blockBytes);
            return block;
        }

        public string GetValidBlockContents(int addressIndex, int propertiesOffset)
        {
            int address = ComputeBlockAddress(addressIndex, propertiesOffset);
            Block<T> block = ReadBlock(address);
            string contents = $"Address: {address}\n" +
                              $"Valid count: {block.ValidCount}\n" + 
                              $"Block factor: {block.BlockFactor}\n";
            return contents + block.GetValidContentsString();
        }

        public virtual int ComputeBlockAddress(T data)
        {
            throw new NotImplementedException();
        }

        public int ComputeBlockAddress(int addressIndex, int propertiesOffset)
        {
            return propertiesOffset + addressIndex * _blockSize;
        }

        public virtual void Dispose()
        {
            _file.Dispose();
            GC.WaitForPendingFinalizers();
        }
    }
}
