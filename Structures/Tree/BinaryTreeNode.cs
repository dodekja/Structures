namespace Structures.Tree
{
    public class BinaryTreeNode<TKey,TData> : AbstractTreeNode<TKey,TData?> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        protected internal override AbstractTreeNode<TKey, TData?>? Parent { get; set; }
        protected internal override AbstractTreeNode<TKey, TData?>?[] Children { get; set; }

        public BinaryTreeNode(TKey key, TData data)
        {
            Key = key;
            Data = data;
            Parent = null;
            Children = new BinaryTreeNode<TKey, TData?>?[2];
        }

        protected internal override AbstractTreeNode<TKey, TData?>? GetSon(int index)
        {
            if (index is >= 2 or < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index out of range");
            }

            return Children[index];
        }

        public BinaryTreeNode<TKey, TData?>? GetLeftSon()
        {
            return (BinaryTreeNode<TKey, TData?>?)Children[0];
        }

        public BinaryTreeNode<TKey, TData?>? GetRightSon()
        {
            return (BinaryTreeNode<TKey, TData?>?)Children[1];
        }

        public void SetLeftSon(BinaryTreeNode<TKey, TData?>? leftSon)
        {
            Children[0] = leftSon;
            if (leftSon != null)
            {
                leftSon.Parent = this;
            }
        }

        public void SetRightSon(BinaryTreeNode<TKey, TData?>? rightSon)
        {
            Children[1] = rightSon;
            if (rightSon != null)
            {
                rightSon.Parent = this;
            }
        }

        public int GetSonsCount()
        {
            return (Children[0] != null ? 1 : 0) + (Children[1] != null ? 1 : 0);
        }

        public bool IsLeftSon()
        {
            var parent = Parent as BinaryTreeNode<TKey, TData?>;
            return parent != null && parent.GetLeftSon() == this;
        }

        public bool HasLeftSon()
        {
            return Children[0] != null;
        }

        public bool IsRightSon()
        {
            var parent = Parent as BinaryTreeNode<TKey, TData?>;
            return parent != null && parent.GetLeftSon() == this;
        }

        public bool HasRightSon()
        {
            return Children[1] != null;
        }
    }
}
