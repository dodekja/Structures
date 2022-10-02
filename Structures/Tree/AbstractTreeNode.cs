namespace Structures.Tree
{
    public abstract class AbstractTreeNode<TKey, TData> where TKey : IComparable<TKey>
    {
        public TKey Key { get; set; }

        public TData? Data { get; set; }

        protected abstract AbstractTreeNode<TKey, TData>? Parent { get; set; }

        protected abstract AbstractTreeNode<TKey, TData>?[] Children { get; set; }

        protected abstract AbstractTreeNode<TKey,TData>? GetSon(int index);
    }
}
