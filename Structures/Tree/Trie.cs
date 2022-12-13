using Structures.Interface;
using System.Collections;
using System.Text;

namespace Structures.Tree
{
    internal class Trie<T> where T : IData<T>
    {
        public TrieNode<T> Root;

        public int BlockFactor { get; }

        public Trie(int blockFactor)
        {
            Root = new ExternalTrieNode<T>(0, sizeof(int), 0, null);
            BlockFactor = blockFactor;
        }

        private int Add(T data)
        {
            var node = FindNode(data) as ExternalTrieNode<T>;
            if (node != Root)
            {
                var parent = node.Parent as InternalTrieNode<T>;
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

            newInternalNode.SetLeftSon(new ExternalTrieNode<T>(0, -1, newInternalNode.Depth + 1, newInternalNode));
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
            while (actualInternal != null && actualInternal.GetLeftSon() != null && actualInternal.GetRightSon() != null)
            {
                BitArray keyBits = new(BitConverter.GetBytes(data.GetHash()));
                if (keyBits[actual.Depth] == false)
                {
                    actual = actualInternal.GetLeftSon();
                }
                else
                {
                    actual = actualInternal.GetRightSon();
                }

                if (actual is InternalTrieNode<T> node)
                {
                    actualInternal = node;
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

        /// <summary>
        /// Get address of the block in dynamic hash file.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Address of the dynamic hash file block as an offset of bytes from the beginning of file.</returns>
        /// <exception cref="ArgumentException">Thrown when there is no node for the given data.</exception>
        public int Find(T data)
        {
            if (FindNode(data) is ExternalTrieNode<T> node)
            {
                return node.BlockAddress;
            }

            throw new ArgumentException($"Tree does not contain the node for {data}");
        }

        public void AddBlock(T data, int address, int numberOfItems)
        {
            var node = FindNode(data) as ExternalTrieNode<T>;
            node.BlockAddress = address;
            node.NumberOfItems = numberOfItems;
        }

        public void Save(string filename)
        {
            StringBuilder builder = new();

            Queue<TrieNode<T>> level = new();
            if (Root is ExternalTrieNode<T> externalRoot)
            {
                builder.Append($"{externalRoot.ToString()}\n");
            }
            else
            {
                TrieNode<T> node = Root;
                level.Enqueue(node as InternalTrieNode<T>);
                while (level.Count > 0)
                {
                    node = level.Dequeue();
                    if (node is InternalTrieNode<T> internalNode)
                    {
                        //builder.Append($"{internalNode.ToString()}\n");
                        foreach (TrieNode<T> child in internalNode.Children)
                        {
                            if (child != null)
                            {
                                level.Enqueue(child);
                            }
                        }
                    }
                    else
                    {
                        builder.Append($"{(node as ExternalTrieNode<T>)!.ToString()}\n");
                    }
                }
            }

            System.IO.File.WriteAllText($"{filename}.txt", builder.ToString());
        }
        /// <summary>
        /// Loads the index Trie from a text file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>Number of blocks read</returns>
        public int Load(string filename)
        {
            string[] lines = System.IO.File.ReadAllLines($"{filename}.txt");
            string[] line;
            if (lines.Length == 1)
            {
                line = lines[0].Split(null);
                Root = new ExternalTrieNode<T>(int.Parse(line[1]), int.Parse(line[2]), int.Parse(line[3]), null);
            }
            else
            {
                Root = new InternalTrieNode<T>(0, null);
                line = lines[0].Split(null);
                string mask = line[0];
                InternalTrieNode<T> actualNode = Root as InternalTrieNode<T>;

                for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
                {
                    line = lines[lineIndex].Split(null);
                    mask = line[0];
                    actualNode = Root as InternalTrieNode<T>;

                    for (int characterIndex = 0; characterIndex < mask.Length; characterIndex++)
                    {
                        if (characterIndex != mask.Length - 1)
                        {
                            if (mask[characterIndex] == '0')
                            {
                                if (!actualNode.HasLeftSon())
                                {
                                    actualNode.SetLeftSon(new InternalTrieNode<T>(actualNode.Depth + 1, actualNode));
                                }
                                actualNode = actualNode.GetLeftSon() as InternalTrieNode<T>;
                            }
                            else
                            {
                                if (!actualNode.HasRightSon())
                                {
                                    actualNode.SetRightSon(new InternalTrieNode<T>(actualNode.Depth + 1, actualNode));
                                }
                                actualNode = actualNode.GetRightSon() as InternalTrieNode<T>;
                            }
                        }
                        else
                        {
                            if (mask[characterIndex] == '0')
                            {
                                actualNode.SetLeftSon(new ExternalTrieNode<T>(int.Parse(line[1]), int.Parse(line[2]), int.Parse(line[3]), actualNode));
                            }
                            else
                            {
                                actualNode.SetRightSon(new ExternalTrieNode<T>(int.Parse(line[1]), int.Parse(line[2]), int.Parse(line[3]), actualNode));
                            }
                        }

                    }
                }

            }
            return lines.Length;
        }
    }
}
