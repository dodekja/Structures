namespace Structures.Interface
{
    public interface IRecord<T>
    {
        public byte[] ToByteArray();

        public void FromByteArray(byte[] array);

        public int GetSize();
    }
}