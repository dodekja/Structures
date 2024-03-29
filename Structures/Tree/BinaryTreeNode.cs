﻿namespace Structures.Tree
{
    public class BinaryTreeNode<TKey,TData> : AbstractTreeNode<TKey,TData?> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        protected internal override AbstractTreeNode<TKey, TData?>? Parent { get; set; }
        protected internal override AbstractTreeNode<TKey, TData?>?[] Children { get; set; }

        /// <summary>
        /// Height of left subtree.
        /// </summary>
        protected internal int LeftSubtreeSize { get; set; }

        /// <summary>
        /// Height of right subtree.
        /// </summary>
        protected internal int RightSubtreeSize { get; set; }
        
        public BinaryTreeNode(TKey key, TData data)
        {
            Key = key;
            Data = data;
            Parent = null;
            Children = new BinaryTreeNode<TKey, TData?>?[2];
            LeftSubtreeSize = 0;
            RightSubtreeSize = 0;
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
            if (Children[0] != null)
            {
                LeftSubtreeSize = 0;
            }
            if (leftSon != null)
            {
                leftSon.Parent = this;
                LeftSubtreeSize = leftSon.LeftSubtreeSize + leftSon.RightSubtreeSize + 1;
            }
            else
            {
                LeftSubtreeSize = 0;
            }
            Children[0] = leftSon;

        }

        public void SetRightSon(BinaryTreeNode<TKey, TData?>? rightSon)
        {
            if (Children[1] != null)
            {
                RightSubtreeSize = 0;
            }
            if (rightSon != null)
            {
                rightSon.Parent = this;
                RightSubtreeSize = rightSon.LeftSubtreeSize + rightSon.RightSubtreeSize + 1;
            }
            else
            {
                RightSubtreeSize = 0;
            }
            Children[1] = rightSon;

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
            return parent != null && parent.GetRightSon() == this;
        }

        public bool HasRightSon()
        {
            return Children[1] != null;
        }

        /// <summary>
        /// Get the difference between the left and right subtree.
        /// </summary>
        /// <returns>Negative value if left subtree is bigger, 0 if subtrees are equal and positive value if right subtree is bigger.</returns>
        public int GetSubtreeDifference()
        {
            return  RightSubtreeSize - LeftSubtreeSize;
        }

        public void IncrementLeftSubtree()
        {
            LeftSubtreeSize++;
        }

        public void IncrementRightSubtree()
        {
            RightSubtreeSize++;
        }
    }
}
