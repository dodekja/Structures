using System.ComponentModel.Design;

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
        /// <param name="stack"></param>
        /// <exception cref="ArgumentException">Thrown when the tree already contains an element with the given key.</exception>
        private void Add(TKey key, TData? data, bool balance)
        {
            BinaryTreeNode<TKey, TData?> newNode = new(key, data);

            BinaryTreeNode<TKey, TData?>? actual = null;
            BinaryTreeNode<TKey, TData?>? next = Root as BinaryTreeNode<TKey, TData?>;

            Stack<(BinaryTreeNode<TKey, TData?>, bool)> updateSubtree = new();

            while (next != null)
            {
                actual = next;
                switch (next.Key.CompareTo(newNode.Key))
                {
                    case 0:
                        throw new ArgumentException($"An item with the key {key} has already been added.");
                    case < 0:
                        next = next.GetRightSon();
                        updateSubtree.Push(new(actual, false));

                        break;
                    default:
                        next = next.GetLeftSon();
                        updateSubtree.Push(new(actual, true));
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

            while (updateSubtree.Count > 0)
            {
                var item = updateSubtree.Pop();
                BinaryTreeNode<TKey, TData?> node = item.Item1;
                int subtreeDifference = node.GetSubtreeDifference();

                //Without this condition, the difference in subtree sizes is smaller but
                //the execution time is longer
                if (subtreeDifference == 0)
                {
                    balance = false;
                }

                if (item.Item2)
                {
                    node.IncrementLeftSubtree();
                    if (balance)
                    {
                        if (subtreeDifference == 1 || subtreeDifference == -1)
                        {
                            continue;
                        }

                        if (subtreeDifference < -1)
                        {
                            if (node.GetSubtreeDifference() < -1)
                            {
                                RotateNodeRight(node.GetLeftSon());
                            }
                        }
                        else
                        {
                            if (node.GetSubtreeDifference() > 1)
                            {
                                RotateNodeLeft(node.GetRightSon());
                            }
                        }
                    }
                }
                else
                {
                    node.IncrementRightSubtree();
                    if (balance)
                    {
                        if (subtreeDifference == 1 || subtreeDifference == -1)
                        {
                            continue;
                        }

                        if (subtreeDifference < -1)
                        {
                            if (node.GetSubtreeDifference() < -1)
                            {
                                RotateNodeRight(node.GetLeftSon());
                            }
                        }
                        else
                        {
                            if (node.GetSubtreeDifference() > 1)
                            {
                                RotateNodeLeft(node.GetRightSon());
                            }
                        }
                    }
                }
            }

            Count++;
        }

        public override void Add(TKey key, TData? data)
        {
            Add(key, data, false);
        }

        public void AddWithBalance(TKey key, TData? data)
        {
            Stack<BinaryTreeNode<TKey, TData?>>? path = new Stack<BinaryTreeNode<TKey, TData?>>();
            Add(key, data, true);
        }

        public override TData? Remove(TKey key)
        {
            return Remove(key, false);
        }

        public TData? RemoveWithBalance(TKey key)
        {
            return Remove(key, true);
        }

        private TData? Remove(TKey key, bool balance)
        {
            BinaryTreeNode<TKey, TData?>? node = FindNode(key);
            Stack<(BinaryTreeNode<TKey, TData?>, bool)> updateSubtree = new();

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

                var child = node;
                while (parent != null)
                {
                    if (child.IsLeftSon())
                    {
                        parent.LeftSubtreeHeight--;
                    }
                    else
                    {
                        parent.RightSubtreeHeight--;
                    }
                    if (balance)
                    {
                        int subtreeDifference = parent.GetSubtreeDifference();
                        if (subtreeDifference < -1)
                        {
                            RotateNodeRight(parent.GetLeftSon());
                        }
                        else
                        {
                            RotateNodeLeft(parent.GetRightSon());
                        }
                    }
                    child = parent;
                    parent = parent.Parent as BinaryTreeNode<TKey, TData?>;
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

            var childNode = node;
            while (parent != null)
            {
                if (childNode.IsLeftSon())
                {
                    parent.LeftSubtreeHeight--;
                }
                else
                {
                    parent.RightSubtreeHeight--;
                }
                if (balance)
                {
                    int subtreeDifference = parent.GetSubtreeDifference();
                    if (subtreeDifference < -1)
                    {
                        RotateNodeRight(parent.GetLeftSon());
                    }
                    else
                    {
                        RotateNodeLeft(parent.GetRightSon());
                    }
                }
                childNode = parent;
                parent = parent.Parent as BinaryTreeNode<TKey, TData?>;
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

        private BinaryTreeNode<TKey, TData?> InOrderPredecessor(BinaryTreeNode<TKey, TData?> node)
        {
            if (node.HasLeftSon())
            {
                BinaryTreeNode<TKey, TData?> predecessor = node.GetLeftSon()!;
                while (predecessor!.GetRightSon() != null)
                {
                    predecessor = predecessor.GetRightSon();
                }

                return predecessor;
            }

            BinaryTreeNode<TKey, TData?>? parent = node.Parent as BinaryTreeNode<TKey, TData?>;
            while (parent != null && node.IsLeftSon())
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

        public BinaryTreeNode<TKey, TData?>? FindNodeOrParent(TKey key)
        {
            var actual = Root as BinaryTreeNode<TKey, TData?>;

            while (actual != null && !actual.Key.Equals(key))
            {
                if (key.CompareTo(actual.Key) < 0 && actual.HasLeftSon())
                {
                    actual = actual.GetLeftSon();
                }
                else if(key.CompareTo(actual.Key) > 0 && actual.HasRightSon())
                {
                    actual = actual.GetRightSon();
                }
                else
                {
                    return actual;
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
                level.Enqueue((Root as BinaryTreeNode<TKey, TData?>)!);
                BinaryTreeNode<TKey, TData?> node;
                while (level.Count > 0)
                {
                    node = level.Dequeue();
                    nodes.Add(node);
                    foreach (AbstractTreeNode<TKey, TData?>? child in node.Children)
                    {
                        if (child != null)
                        {
                            level.Enqueue(child as BinaryTreeNode<TKey, TData?>);
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
                    node.Parent = null;
                }

                node.LeftSubtreeHeight = parent.LeftSubtreeHeight + parent.RightSubtreeHeight + 1;
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
                    node.Parent = null;
                }

                node.RightSubtreeHeight = parent.LeftSubtreeHeight + parent.RightSubtreeHeight + 1;
            }
        }

        public void Balance()
        {
            List<BinaryTreeNode<TKey, TData?>> nodes = LevelOrder();
            nodes.Reverse();
            foreach (BinaryTreeNode<TKey, TData?> node in nodes)
            {
                if (!node.HasLeftSon() && !node.HasRightSon())
                {
                    continue;
                }

                int subtreeDifference = node.GetSubtreeDifference();

                if (subtreeDifference == 1 || subtreeDifference == -1 || subtreeDifference == 0)
                {
                    continue;
                }

                if (subtreeDifference < -1)
                {
                    while (node.GetSubtreeDifference() < -1)
                    {
                        RotateNodeRight(node.GetLeftSon());
                    }
                }
                else
                {
                    while (node.GetSubtreeDifference() > 1)
                    {
                        RotateNodeLeft(node.GetRightSon());
                    }
                }
            }

            var curRoot = Root as BinaryTreeNode<TKey, TData?>;
            if (curRoot != null)
            {
                if (curRoot.GetSubtreeDifference() < -1)
                {
                    RotateNodeRight(curRoot.GetLeftSon());
                }
                else if (curRoot.GetSubtreeDifference() > 1)
                {
                    RotateNodeLeft(curRoot.GetRightSon());
                }
            }
        }

        /// <summary>
        /// Creates a tree from an unsorted array of (key, data) tuples.
        /// </summary>
        /// <param name="inputRange">unsorted array of (key, data) tuples</param>
        public void InsertRange(List<(TKey,TData?)> inputRange)
        {
            inputRange.Sort((x,y) => y.Item1.CompareTo(x.Item1));
            Root = InsertMedian(0, inputRange.Count-1, inputRange);
        }

        private BinaryTreeNode<TKey, TData?> InsertMedian(int leftBound, int rightBound, List<(TKey, TData?)> inputRange)
        {
            if (leftBound <= rightBound)
            {
                //find median
                int median = (int)Math.Floor((double)((leftBound + rightBound) / 2));
                var medianItem = inputRange[median];
                BinaryTreeNode<TKey, TData?> node = new BinaryTreeNode<TKey, TData?>(medianItem.Item1, medianItem.Item2);
                node.SetLeftSon(InsertMedian(leftBound,median - 1, inputRange));
                node.SetRightSon(InsertMedian(median + 1,rightBound, inputRange));
                return node;
            }

            return null;
        }

        public List<TData?> FindRange(TKey start, TKey end)
        {
            List<TData?> result = new();
            if (Root != null)
            {
                BinaryTreeNode<TKey, TData?>? current = InOrderPredecessor(FindNodeOrParent(start));
                if (current != null)
                {
                    while (current.Key.CompareTo(end) <= 0)
                    {
                        if (current.Key.CompareTo(start) >= 0)
                        {
                            result.Add(current.Data);
                        }

                        var old = current; 
                        current = InOrderSuccessor(current);
                        if (result.Contains(current.Data) || old == current)
                        {
                            break;
                        }
                    }
                }
            }
            return result;
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
