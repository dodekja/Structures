using System.Transactions;

namespace Structures.Tree
{
    public class BinarySearchTree<TKey, TData> : AbstractTree<TKey, TData> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        /// <summary>
        /// Number of items in the tree.
        /// </summary>
        protected override int Count { get; set; }

        /// <summary>
        /// Root of the tree.
        /// </summary>
        protected override AbstractTreeNode<TKey, TData>? Root { get; set; }

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
        public override void Add(TKey key, TData data)
        {
            BinaryTreeNode<TKey, TData> newNode = new BinaryTreeNode<TKey, TData>(key, data);

            BinaryTreeNode<TKey, TData>? actual = null;
            BinaryTreeNode<TKey, TData>? next = (BinaryTreeNode<TKey, TData>?)Root;

            while (next != null)
            {
                actual = next;
                switch (next.Key.CompareTo(newNode.Key))
                {
                    case 0:
                        throw new ArgumentException("An item with the same key has already been added.");
                    case < 0:
                        next = next.GetLeftSon();
                        break;
                    default:
                        next = next.GetRightSon();
                        break;
                }
            }

            newNode.Parent = actual;

            if (actual == null)
            {
                Root = newNode;
            }
            else if (actual.Key.CompareTo(newNode.Key) < 0)
            {
                actual.Children[0] = newNode;
            }
            else
            {
                actual.Children[1] = newNode;
            }

            Count++;
        }

        public override void Remove(TKey key)
        {
            throw new NotImplementedException();
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
                BinaryTreeNode<TKey, TData>? actual = Root as BinaryTreeNode<TKey, TData>;

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
                if (actual != null) return actual.Data;
            }
            return default;
        }

        public List<Tuple<TKey, TData?>> InOrder()
        {
            List<Tuple<TKey, TData?>> path = new(Count);
            BinaryTreeNode<TKey,TData>? currentNode = Root as BinaryTreeNode<TKey, TData>;

            if (Root != null)
            {
                Stack<BinaryTreeNode<TKey, TData>> stack = new Stack<BinaryTreeNode<TKey, TData>>(Count);
                while (currentNode != null || stack.Count > 0)
                {
                    while (currentNode != null)
                    {
                        stack.Push(currentNode);
                        currentNode = currentNode.GetRightSon();
                    }

                    currentNode = stack.Pop();

                    path.Add(Tuple.Create(currentNode.Key,currentNode.Data));

                    currentNode = currentNode.GetLeftSon();
                }
            }

            return path;
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
