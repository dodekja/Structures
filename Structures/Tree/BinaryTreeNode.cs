namespace Structures.Tree
{
    public class BinaryTreeNode<TKey,TData> : AbstractTreeNode<TKey,TData> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        protected internal override AbstractTreeNode<TKey, TData>? Parent { get; set; }
        protected internal override AbstractTreeNode<TKey, TData>?[] Children { get; set; }

        public BinaryTreeNode(TKey key, TData data)
        {
            Key = key;
            Data = data;
            Parent = null;
            Children = new BinaryTreeNode<TKey, TData>?[2];
        }

        protected internal override AbstractTreeNode<TKey, TData>? GetSon(int index)
        {
            if (index is >= 2 or < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index out of range");
            }

            return Children[index];
        }

        public BinaryTreeNode<TKey, TData>? GetLeftSon()
        {
            return (BinaryTreeNode<TKey, TData>?)Children[0];
        }

        public BinaryTreeNode<TKey, TData>? GetRightSon()
        {
            return (BinaryTreeNode<TKey, TData>?)Children[1];
        }
    }
}
