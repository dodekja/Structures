using Structures.Interface;

namespace TestStructures.Wrappers
{
    /// <summary>
    /// Simple int wrapper to use for hash file tests.
    /// </summary>
    internal class IntData : IData<IntData>
    {
        public int Integer { get; set; }

        public IntData()
        {
            Integer = 0;
        }

        public IntData(int integer)
        {
            Integer = integer;
        }

        public byte[] ToByteArray()
        {
            return BitConverter.GetBytes(Integer);
        }

        public void FromByteArray(byte[] array)
        {
            Integer = BitConverter.ToInt32(array);
        }

        public int GetSize()
        {
            return sizeof(int);
        }

        public int GetHash()
        {
            return Integer.GetHashCode();
        }

        public bool IsEqual(IntData data)
        {
            return (data.Integer == Integer);
        }

        public IntData CreateClass()
        {
            return new IntData();
        }

        public override string ToString()
        {
            return Integer.ToString();
        }
    }
}
