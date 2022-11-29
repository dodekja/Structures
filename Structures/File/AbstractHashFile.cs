using Structures.Interface;

namespace Structures.File
{
    public abstract class AbstractHashFile<T> : IDisposable where T : IData<T>, new()
    {
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

        protected IList<int> _blockAddresses;

        public abstract void Insert(T data);

        public abstract void Delete(T data);

        public abstract T? Find(T data);

        protected abstract int ReadProperties();
        protected abstract int SaveProperties();

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
        public abstract void Dispose();
    }
}
