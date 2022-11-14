namespace Structures.Tree
{
    public abstract class AbstractTree<TKey,TData> where TKey: IComparable<TKey>, IEquatable<TKey>
    {
        protected abstract AbstractTreeNode<TKey, TData?>? Root { get; set; }

        public abstract int Count { get; set; }

        public abstract void Add(TKey key, TData data);

        public abstract TData? Remove(TKey key);

        public abstract TData? Find(TKey key);

    }
}