using Structures.Interface;

namespace Structures.Tree
{
    internal class InternalTrieNode<T>: TrieNode<T> where T : IData<T>
    {
        internal TrieNode<T>?[] Children { get; set; }

        public InternalTrieNode(int depth, InternalTrieNode<T>? parent) : base(depth,parent)
        {
            Children = new TrieNode<T>?[2];
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
                leftSon.Mask = Mask + "0";
            }

            Children[0] = leftSon;
        }

        public void SetRightSon(TrieNode<T>? rightSon)
        {
            if (rightSon != null)
            {
                rightSon.Depth = Depth + 1;
                rightSon.Parent = this;
                rightSon.Mask = Mask + "1";
            }

            Children[1] = rightSon;
        }

        public int GetSonsCount()
        {
            return (Children[0] != null ? 1 : 0) + (Children[1] != null ? 1 : 0);
        }

        public bool HasLeftSon()
        {
            return Children[0] != null;
        }

        public bool HasRightSon()
        {
            return Children[1] != null;
        }

        public override string ToString()
        {
            return $"{Mask}";
        }
    }
}
