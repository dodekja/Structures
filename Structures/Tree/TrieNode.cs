using Structures.Interface;

namespace Structures.Tree
{
    internal class TrieNode<T> where T : IData<T>
    {
        public int Depth { get; set; }

        internal TrieNode<T>? Parent { get; set; }

        internal string Mask { get; set; }

        public TrieNode(int depth, TrieNode<T>? parent)
        {
            Depth = depth;
            Parent = parent;
            Mask = "";
        }
        
        public bool IsLeftSon()
        {
            return Parent != null && (Parent as InternalTrieNode<T>)?.GetLeftSon() == this;
        }

        public bool IsRightSon()
        {
            return Parent != null && (Parent as InternalTrieNode<T>)?.GetRightSon() == this;
        }

        public virtual string ToString()
        {
            return $"{Depth}";
        }
    }
}
