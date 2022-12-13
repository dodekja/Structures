using System.Text;
using Structures.Interface;

namespace Structures.File
{
    public class Block<T> : IRecord<T> where T : IData<T>, new()
    {
        /// <summary>
        /// Capacity of the block.
        /// </summary>
        public int BlockFactor { get; }

        /// <summary>
        /// Number of valid records.
        /// </summary>
        public int ValidCount { get; private set; }

        /// <summary>
        /// Contains records with valid records on the indexes [0;_validCount].
        /// </summary>
        private List<T> _records;

        public Block(int blockFactor)
        {
            BlockFactor = blockFactor;
            _records = new List<T>(blockFactor);
            for (int i = 0; i < BlockFactor; i++)
            {
                var data = Activator.CreateInstance<T>();
                _records.Add(data);
            }

            ValidCount = 0;
        }

        public void AddRecord(T record)
        {
            for (var index = 0; index < ValidCount; index++)
            {
                if (!_records[index].IsEqual(record)) continue;
                throw new InvalidOperationException($"Record equal to {record} already exists.");
            }

            if (ValidCount == BlockFactor)
            {
                throw new IndexOutOfRangeException("Block capacity reached.");
            }

            _records[ValidCount] = record;
            ValidCount++;
        }

        public T RemoveRecord(T data)
        {
            for (var index = 0; index < ValidCount; index++)
            {
                if (!_records[index].IsEqual(data)) continue;
                var ret = _records[index];
                _records.RemoveAt(index);
                _records.Add(Activator.CreateInstance<T>());
                ValidCount--;
                return ret;
            }

            throw new IndexOutOfRangeException("Data not found in block.");
        }

        public (T record,int index)? GetRecordForUpdate(T data)
        {
            for (var index = 0; index < ValidCount; index++)
            {
                if (!_records[index].IsEqual(data)) continue;
                return (_records[index], index);
            }

            return null;
        }

        public void UpdateRecord(T data, int index)
        {
            if (index <= ValidCount - 1)
            {
                _records[index] = data;
            }
            else
            {
                throw new IndexOutOfRangeException("Index is invalid");
            }
        }

        public T? FindRecord(T data)
        {
            for (var index = 0; index < ValidCount; index++)
            {
                if (!_records[index].IsEqual(data)) continue;
                return _records[index];
            }

            return default;
        }

        public byte[] ToByteArray()
        {
            byte[] array = new byte[GetSize()];
            byte[] validCountBytes = BitConverter.GetBytes(ValidCount);
            int offset = 0;
            Buffer.BlockCopy(validCountBytes, 0, array, offset, validCountBytes.Length);
            offset = validCountBytes.Length;
            for (int index = 0; index < BlockFactor; index++)
            {
                int recordSize = _records[index].GetSize();
                Buffer.BlockCopy(_records[index].ToByteArray(), 0, array, offset, recordSize);
                offset += recordSize;
            }
            return array;
        }

        public void FromByteArray(byte[] array)
        {
            byte[] data = new byte[sizeof(int)];
            int srcOffset = 0;
            Buffer.BlockCopy(array, srcOffset, data, 0, sizeof(int));
            ValidCount = BitConverter.ToInt32(data);
            srcOffset += sizeof(int);

            for (int index = 0; index < BlockFactor; index++)
            {
                data = new byte[_records[index].GetSize()];
                Buffer.BlockCopy(array, srcOffset, data, 0, data.Length);
                _records[index].FromByteArray(data);
                srcOffset += data.Length;
            }
        }

        public int GetSize()
        {
            return Activator.CreateInstance<T>().GetSize() * BlockFactor + sizeof(int);
        }

        public string GetContentsString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var record in _records)
            {
                Console.WriteLine(record.ToString());
                builder.Append($"{record}\n");
            }
            return builder.ToString();
        }

        public string GetValidContentsString()
        {
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < ValidCount; index++)
            {
                Console.WriteLine(_records[index].ToString());
                builder.Append($"{_records[index]}\n");
            }
            return builder.ToString();
        }

        public List<T> GetValidItems()
        {
            List<T> validItems = new List<T>();
            for (int index = 0; index < ValidCount; index++)
            {
                validItems.Add(_records[index]);
            }
            return validItems;
        }

        public void ClearRecords()
        {
            _records = new List<T>(BlockFactor);
            for (int i = 0; i < BlockFactor; i++)
            {
                var data = Activator.CreateInstance<T>();
                _records.Add(data);
            }
            ValidCount = 0;
        }

        public T GetFirstItem()
        {
            return _records[0];
        }
    }
}
