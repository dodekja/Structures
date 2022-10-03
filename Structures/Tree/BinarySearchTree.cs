namespace Structures.Tree
{
    public class BinarySearchTree<TKey, TData> : AbstractTree<TKey, TData> where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        protected override int Size { get; set; }

        protected override AbstractTreeNode<TKey, TData>? Root { get; set; }

        public BinarySearchTree()
        {
            Root = null;
            Size = 0;
        }

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

            Size++;
        }

        public override void Remove(TKey key)
        {
            throw new NotFiniteNumberException();
        }

        /// <summary>
        /// Find a node with the
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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

        public int GetSize()
        {
            return Size;
        }
    }
}
