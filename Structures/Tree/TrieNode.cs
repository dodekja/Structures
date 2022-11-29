using System.Collections;
using System.Xml.Linq;
using Structures.Interface;

namespace Structures.Tree
{
    internal class TrieNode<T> where T : IData<T>
    {
        public int Depth { get; set; }

        internal TrieNode<T>? Parent { get; set; }

        private TrieNode<T>?[] Children { get; set; }
        public int? NumberOfItems { get; set; }

        public int? BlockAddress { get; set; }

        public TrieNode()
        {
            Parent = null;
            Depth = 0;
            Children = new TrieNode<T>?[2];
            BlockAddress = null;
            NumberOfItems = null;
        }

        public TrieNode<T>? GetLeftSon()
        {
            return Children[0];
        }

        public TrieNode<T>? GetRightSon()
        {
            return Children[1];
        }

        public void SetLeftSon(TrieNode<T>? leftSon)
        {
            if (leftSon != null)
            {
                leftSon.Depth = Depth + 1;
                leftSon.Parent = this;
            }

            Children[0] = leftSon;
        }

        public void SetRightSon(TrieNode<T>? rightSon)
        {
            if (rightSon != null)
            {
                rightSon.Depth = Depth + 1;
                rightSon.Parent = this;
            }

            Children[1] = rightSon;
        }

        public int GetSonsCount()
        {
            return (Children[0] != null ? 1 : 0) + (Children[1] != null ? 1 : 0);
        }

        public bool IsLeftSon()
        {
            return Parent != null && Parent.GetLeftSon() == this;
        }

        public bool HasLeftSon()
        {
            return Children[0] != null;
        }

        public bool IsRightSon()
        {
            return Parent != null && Parent.GetRightSon() == this;
        }

        public bool HasRightSon()
        {
            return Children[1] != null;
        }

        public bool IsInternal()
        {
            return BlockAddress == null;
        }

        public void MakeExternal(int blockAddress, int numberOfItems)
        {
            BlockAddress = blockAddress;
            NumberOfItems = numberOfItems;

        }

        public void MakeInternal()
        {
            BlockAddress = null;
            NumberOfItems = null;
        }
    }
}
