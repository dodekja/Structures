namespace Structures.Tree
{
    public abstract class AbstractTree<TKey,TData> where TKey: IComparable<TKey>
    {
        protected abstract AbstractTreeNode<TKey, TData>? Root { get; set; }

        protected abstract int Size { get; set; }

        public abstract void Add(AbstractTreeNode<TKey, TData> newNode);

        public abstract void Remove(AbstractTreeNode<TKey, TData> newNode);
        
        public abstract AbstractTreeNode<TKey, TData> Find(AbstractTreeNode<TKey, TData> newNode);
    }
}