using Structures.Interface;

namespace Structures.Tree
{
    internal class ExternalTrieNode<T> : TrieNode<T> where T : IData<T>
    {
        public int NumberOfItems { get; set; }

        public int BlockAddress { get; set; }

        public ExternalTrieNode(int numberOfItems, int blockAddress,int depth, InternalTrieNode<T>? parent) : base(depth, parent)
        {
            NumberOfItems = numberOfItems;
            BlockAddress = blockAddress;
        }

        public override string ToString()
        {
            return $"{(Mask == "" ? "*" : Mask)} {NumberOfItems} {BlockAddress} {Depth}";
        }
    }
}
