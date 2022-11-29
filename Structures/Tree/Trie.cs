using Structures.Interface;
using System.Collections;

namespace Structures.Tree
{
    internal class Trie<T> where T : IData<T>
    {
        public TrieNode<T> Root;

        public int BlockFactor { get; private set; }

        public Trie(int blockFactor)
        {
            Root = new TrieNode<T>();
            BlockFactor = blockFactor;
        }

        private int Add(T data)
        {
            var node = FindNode(data);
            node.SetLeftSon(new TrieNode<T>());
            node.SetRightSon(new TrieNode<T>());
            var address = node.BlockAddress;
            node.MakeInternal();
            return node.Depth;
        }

        private void Add(TrieNode<T> node)
        {
            node.SetLeftSon(new TrieNode<T>());
            node.SetRightSon(new TrieNode<T>());
            node.MakeInternal();
        }

        public int? AddItem(T data)
        {
            var node = FindNode(data);
            node.NumberOfItems++;
            if (node.NumberOfItems == BlockFactor)
            {
                return Add(data);
            }

            return null;
        }

        public void Split(T left, T right, int leftNumberOfItems, int leftAddress, int rightNumberOfItems, int rightAddress)
        {
            TrieNode<T>? leftNode = FindNode(left);
            TrieNode<T>? rightNode = FindNode(right);
            leftNode.MakeExternal(leftAddress, leftNumberOfItems);
            rightNode.MakeExternal(rightAddress, rightNumberOfItems);
            if (leftNumberOfItems == 0)
            {
                leftNode.Parent.SetLeftSon(null);
                Add(rightNode);
            }
            else if (rightNumberOfItems == 0)
            {
                rightNode.Parent.SetRightSon(null);
                Add(rightNode);
            }
        }

        public (int, int) Join(TrieNode<T> parent)
        {
            var leftSon = parent.GetLeftSon();
            var rightSon = parent.GetRightSon();
            var tuple = (leftSon.BlockAddress.Value, rightSon.BlockAddress.Value);
            int address = leftSon.BlockAddress.Value < rightSon.BlockAddress.Value ? leftSon.BlockAddress.Value : rightSon.BlockAddress.Value;
            parent.MakeExternal(address, leftSon.NumberOfItems.Value + rightSon.NumberOfItems.Value);
            parent.SetLeftSon(null);
            parent.SetRightSon(null);
            return tuple;
        }

        public TrieNode<T>? FindNode(T data)
        {
            var actual = Root;
            BitArray keyBits = new BitArray(BitConverter.GetBytes(data.GetHash()));
            while (actual != null && actual.GetLeftSon() != null && actual.GetRightSon() != null)
            {
                if (keyBits[actual.Depth] == false)
                {
                    actual = actual.GetLeftSon();
                }
                else
                {
                    actual = actual.GetRightSon();
                }
            }
            return actual;
        }

        public (int, int)? Remove(T data)
        {
            var node = FindNode(data);
            
            node.NumberOfItems--;

            if (node != Root)
            {
                var brother = node.IsLeftSon() ? node.Parent.GetRightSon() : node.Parent.GetLeftSon();

                if (node.NumberOfItems + brother.NumberOfItems < BlockFactor)
                {
                    return Join(node.Parent);
                }
            }

            return null;
        }

        public int Find(T data)
        {
            if (Root != null)
            {
                TrieNode<T>? node = FindNode(data);

                if (node != null)
                {
                    return node.BlockAddress.Value;
                }
            }

            throw new ArgumentException($"Tree does not contain the node for {data}");
        }

    }
}
