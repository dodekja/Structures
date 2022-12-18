namespace Structures.Interface;

public interface IData<T> : IRecord<T>
{
    public int GetHash();

    public bool IsEqual(T data);

    public T CreateClass();
}