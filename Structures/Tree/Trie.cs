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
            Root = new ExternalTrieNode<T>(0,sizeof(int),0,null);
            BlockFactor = blockFactor;
        }

        private int Add(T data)
        {
            var node = FindNode(data) as ExternalTrieNode<T>;
            if (node != Root)
            {
                var parent = node.Parent as  InternalTrieNode<T>;
                var newInternalNode = new InternalTrieNode<T>(parent.Depth + 1, parent);
                if (node.IsLeftSon())
                {
                    parent.SetLeftSon(newInternalNode);
                }
                else
                {
                    parent.SetRightSon(newInternalNode);
                }
                newInternalNode.SetLeftSon(new ExternalTrieNode<T>(0, -1, newInternalNode.Depth + 1, newInternalNode));
                newInternalNode.SetRightSon(new ExternalTrieNode<T>(0, -1, newInternalNode.Depth + 1, newInternalNode));
            }
            else
            {
                Root = new InternalTrieNode<T>(0, null);
                var internalRoot = Root as InternalTrieNode<T>;
                internalRoot.SetLeftSon(new ExternalTrieNode<T>(0, -1, 1, internalRoot));
                internalRoot.SetRightSon(new ExternalTrieNode<T>(0, -1, 1, internalRoot));
            }

            return node.Depth;
        }

        private void Add(ExternalTrieNode<T> node)
        {
            var parent = node.Parent as InternalTrieNode<T>;
            var newInternalNode = new InternalTrieNode<T>(node.Depth, parent);
            if (parent != null && node.IsLeftSon())
            {
                parent.SetLeftSon(newInternalNode);
            }
            else
            {
                parent.SetRightSon(newInternalNode);
            }

            newInternalNode.SetLeftSon(new ExternalTrieNode<T>(0,-1,newInternalNode.Depth+1,newInternalNode));
            newInternalNode.SetRightSon(new ExternalTrieNode<T>(0, -1, newInternalNode.Depth + 1, newInternalNode));
        }

        public int? AddItem(T data)
        {
            var node = FindNode(data) as ExternalTrieNode<T>;
            node.NumberOfItems++;
            if (node.NumberOfItems == BlockFactor)
            {
                return Add(data);
            }

            return null;
        }

        public void Split(T left, T right, int leftNumberOfItems, int leftAddress, int rightNumberOfItems, int rightAddress)
        {
            var leftNode = FindNode(left) as ExternalTrieNode<T>;
            var rightNode = FindNode(right) as ExternalTrieNode<T>;
            leftNode.BlockAddress = leftAddress;
            leftNode.NumberOfItems = leftNumberOfItems;
            rightNode.BlockAddress = rightAddress;
            rightNode.NumberOfItems = rightNumberOfItems;
            if (leftNumberOfItems == 0)
            {
                leftNode.BlockAddress = -1;
                Add(leftNode);
            }
            else if (rightNumberOfItems == 0)
            {
                rightNode.BlockAddress = -1;
                Add(rightNode);
            }
        }

        public (int, int) Join(InternalTrieNode<T> parent)
        {
            var leftSon = parent.GetLeftSon() as ExternalTrieNode<T>;
            var rightSon = parent.GetRightSon() as ExternalTrieNode<T>;
            var blockAddresses = (leftSon.BlockAddress, rightSon.BlockAddress);
            int address = leftSon.BlockAddress < rightSon.BlockAddress ? leftSon.BlockAddress : rightSon.BlockAddress;
            var grandparent = parent.Parent as InternalTrieNode<T>;
            var newNode = new ExternalTrieNode<T>(leftSon.NumberOfItems + rightSon.NumberOfItems, address,
                parent.Depth, grandparent);
            if (parent != Root && parent.IsLeftSon())
            {
                grandparent.SetLeftSon(newNode);
            }
            else if (parent != Root && parent.IsRightSon())
            {
                grandparent.SetRightSon(newNode);
            }
            else
            {
                Root = newNode;
            }
            return blockAddresses;
        }

        public TrieNode<T>? FindNode(T data)
        {
            var actual = Root;
            var actualInternal = actual as InternalTrieNode<T>;
            BitArray keyBits = new BitArray(BitConverter.GetBytes(data.GetHash()));
            while (actualInternal != null && actualInternal.GetLeftSon() != null && actualInternal.GetRightSon() != null)
            {
                if (keyBits[actual.Depth] == false)
                {
                    actual = actualInternal.GetLeftSon() ;
                }
                else
                {
                    actual = actualInternal.GetRightSon();
                }

                if (actual is InternalTrieNode<T>)
                {
                    actualInternal = (InternalTrieNode<T>)actual;
                }
                else
                {
                    return actual as ExternalTrieNode<T>;
                }
            }
            return actual;
        }

        public (int, int)? Remove(T data)
        {
            var node = FindNode(data) as ExternalTrieNode<T>;
            
            node.NumberOfItems--;

            if (node != Root)
            {
                var parent = (node.Parent as InternalTrieNode<T>);
                var brother = (node.IsLeftSon() ? parent.GetRightSon() : parent.GetLeftSon()) as ExternalTrieNode<T>;

                if (brother != null && node.NumberOfItems + brother.NumberOfItems < BlockFactor)
                {
                    return Join(parent);
                }
            }

            return null;
        }

        public int Find(T data)
        {
            if (Root != null)
            {
                ExternalTrieNode<T>? node = FindNode(data) as ExternalTrieNode<T>;

                if (node != null)
                {
                    return node.BlockAddress;
                }
            }

            throw new ArgumentException($"Tree does not contain the node for {data}");
        }

        public void AddBlock(T data, int address, int numberOfItems)
        {
            var node = FindNode(data) as ExternalTrieNode<T>;
            node.BlockAddress = address;
            node.NumberOfItems = numberOfItems;
        }
    }
}
