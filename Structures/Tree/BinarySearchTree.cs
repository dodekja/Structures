namespace Structures.Tree
{
    public class BinarySearchTree<TKey, TData> : AbstractTree<TKey, TData> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// Number of items in the tree.
        /// </summary>
        public override int Count { get; set; }

        /// <summary>
        /// Root of the tree.
        /// </summary>
        protected override AbstractTreeNode<TKey, TData?>? Root { get; set; }

        /// <summary>
        /// Create an empty Binary Search Tree.
        /// </summary>
        public BinarySearchTree()
        {
            Root = null;
            Count = 0;
        }

        /// <summary>
        /// Add an item to the tree containing the data given identified by key.
        /// </summary>
        /// <param name="key">Key of the new item.</param>
        /// <param name="data">Data.</param>
        /// <exception cref="ArgumentException">Thrown when the tree already contains an element with the given key.</exception>
        public override void Add(TKey key, TData? data)
        {
            BinaryTreeNode<TKey, TData?> newNode = new(key, data);

            BinaryTreeNode<TKey, TData?>? actual = null;
            BinaryTreeNode<TKey, TData?>? next = Root as BinaryTreeNode<TKey, TData?>;

            while (next != null)
            {
                actual = next;
                switch (next.Key.CompareTo(newNode.Key))
                {
                    case 0:
                        throw new ArgumentException($"An item with the key {key} has already been added.");
                    case < 0:
                        next = next.GetRightSon();
                        break;
                    default:
                        next = next.GetLeftSon();
                        break;
                }
            }

            newNode.Parent = actual;

            if (actual == null)
            {
                Root = newNode;
            }
            else if (actual.Key.CompareTo(newNode.Key) > 0)
            {
                actual.Children[0] = newNode;
            }
            else
            {
                actual.Children[1] = newNode;
            }

            Count++;
        }

        public override TData? Remove(TKey key)
        {
            BinaryTreeNode<TKey, TData?>? node = FindNode(key);

            if (node == null)
            {
                throw new InvalidOperationException($"Tree does not contain the key {key}");
            }

            BinaryTreeNode<TKey, TData?>? parent = node.Parent as BinaryTreeNode<TKey, TData?>;
            if (node.GetRightSon() == null && node.GetLeftSon() == null)
            {
                if (parent == null)
                {
                    Root = null;
                    Count--;
                    return node.Data;
                }

                if (node.IsLeftSon())
                {
                    parent.SetLeftSon(null);
                }
                else
                {
                    parent.SetRightSon(null);
                }

                node.Parent = null;
                Count--;
                return node.Data;
            }

            if (node.GetLeftSon() == null)
            {
                ReplaceNodes(node, node.GetRightSon());
            }
            else if (node.GetRightSon() == null)
            {
                ReplaceNodes(node, node.GetLeftSon());
            }
            else
            {
                BinaryTreeNode<TKey, TData?>? successor = InOrderSuccessor(node);
                if (successor.Parent != node)
                {
                    ReplaceNodes(successor, successor.GetRightSon());
                    successor.SetRightSon(node.GetRightSon());
                    successor.GetRightSon().Parent = successor;
                }

                ReplaceNodes(node, successor);
                successor.SetLeftSon(node.GetLeftSon());
                successor.GetLeftSon().Parent = successor;
            }

            Count--;
            return node.Data;
        }

        private BinaryTreeNode<TKey, TData?> InOrderSuccessor(BinaryTreeNode<TKey, TData?> node)
        {
            if (node.HasRightSon())
            {
                BinaryTreeNode<TKey, TData?> successor = node.GetRightSon()!;
                while (successor!.GetLeftSon() != null)
                {
                    successor = successor.GetLeftSon();
                }

                return successor;
            }

            BinaryTreeNode<TKey, TData?>? parent = node.Parent as BinaryTreeNode<TKey, TData?>;
            while (parent != null && node.IsRightSon())
            {
                node = parent;
                parent = parent.Parent as BinaryTreeNode<TKey, TData?>;
            }

            return node;
        }

        private void ReplaceNodes(BinaryTreeNode<TKey, TData?> node, BinaryTreeNode<TKey, TData?>? replacement)
        {
            if (node.Parent == null)
            {
                Root = replacement;
            }
            else if (node.IsLeftSon())
            {
                var parent = node.Parent as BinaryTreeNode<TKey, TData?>;

                parent!.SetLeftSon(replacement);
            }
            else
            {
                var parent = node.Parent as BinaryTreeNode<TKey, TData?>;

                parent!.SetRightSon(replacement);
            }

            if (replacement != null)
            {
                replacement.Parent = node.Parent;
            }
        }

        /// <summary>
        /// Find an element with the given key.
        /// </summary>
        /// <param name="key">Key of the element to find.</param>
        /// <returns>Data of the element with the given key or null.</returns>
        public override TData? Find(TKey key)
        {
            if (Root != null)
            {
                BinaryTreeNode<TKey, TData?>? node = FindNode(key);

                if (node != null)
                {
                    return node.Data;
                }
            }

            throw new ArgumentException($"Tree does not contain the key {key}");
        }

        public BinaryTreeNode<TKey, TData?>? FindNode(TKey key)
        {
            var actual = Root as BinaryTreeNode<TKey, TData?>;

            while (actual != null && !actual.Key.Equals(key))
            {
                if (key.CompareTo(actual.Key) < 0)
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

        public List<Tuple<TKey, TData?>> InOrder()
        {
            List<Tuple<TKey, TData?>> path = new(Count);
            BinaryTreeNode<TKey, TData?>? currentNode = Root as BinaryTreeNode<TKey, TData?>;

            if (Root != null)
            {
                Stack<BinaryTreeNode<TKey, TData?>> stack = new(Count);
                while (currentNode != null || stack.Count > 0)
                {
                    while (currentNode != null)
                    {
                        stack.Push(currentNode);
                        currentNode = currentNode.GetLeftSon();
                    }

                    currentNode = stack.Pop();

                    path.Add(Tuple.Create(currentNode.Key, currentNode.Data));

                    currentNode = currentNode.GetRightSon();
                }
            }

            return path;
        }

        public List<BinaryTreeNode<TKey, TData?>> LevelOrder()
        {
            List<BinaryTreeNode<TKey, TData?>> nodes = new List<BinaryTreeNode<TKey, TData?>>(Count);
            if (Root != null)
            {
                Queue<BinaryTreeNode<TKey, TData?>> level = new Queue<BinaryTreeNode<TKey, TData?>>();
                level.Enqueue((Root as BinaryTreeNode<TKey,TData?>)!);
                BinaryTreeNode<TKey, TData?> node;
                while (level.Count > 0)
                {
                    node = level.Dequeue();
                    nodes.Add(node);
                    foreach (AbstractTreeNode<TKey, TData?>? child in node.Children)
                    {
                        if (child != null)
                        {
                            level.Enqueue(child as BinaryTreeNode<TKey,TData?>);
                        }
                    }
                }
            }
            return nodes;
        }

        public void RotateNodeLeft(BinaryTreeNode<TKey, TData?> node)
        {
            if (node.IsRightSon())
            {
                var leftSon = node.GetLeftSon();
                var parent = node.Parent as BinaryTreeNode<TKey, TData>;
                var grandParent = parent.Parent as BinaryTreeNode<TKey, TData>;

                if (grandParent != null)
                {
                    if (parent.IsLeftSon())
                    {
                        grandParent.SetLeftSon(node);
                    }
                    else
                    {
                        grandParent.SetRightSon(node);
                    }
                }

                node.SetLeftSon(parent);
                parent.SetRightSon(leftSon);
                if (Root == parent)
                {
                    Root = node;
                }
            }
        }

        public void RotateNodeRight(BinaryTreeNode<TKey, TData?> node)
        {
            if (node.IsLeftSon())
            {
                var rightSon = node.GetRightSon();
                BinaryTreeNode<TKey, TData> parent = node.Parent as BinaryTreeNode<TKey, TData>;
                BinaryTreeNode<TKey, TData>? grandParent = parent.Parent as BinaryTreeNode<TKey, TData>;

                if (grandParent != null)
                {
                    if (parent.IsLeftSon())
                    {
                        grandParent.SetLeftSon(node);
                    }
                    else
                    {
                        grandParent.SetRightSon(node);
                    }
                }

                node.SetRightSon(parent);
                parent.SetLeftSon(rightSon);
                if (Root == parent)
                {
                    Root = node;
                }
            }
        }

        public void Balance(BinaryTreeNode<TKey, TData?> node)
        {
            //TODO: implement tree balancing algorithm (Try using reverse Level Order with unlimited balance factor?)
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the size of the tree.
        /// </summary>
        /// <returns>Number of items in the tree.</returns>
        public int GetSize()
        {
            return Count;
        }
    }
}
