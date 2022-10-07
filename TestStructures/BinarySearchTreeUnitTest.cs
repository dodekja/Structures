using System.Diagnostics;
using Structures.Tree;

namespace TestStructures
{
    public class BinarySearchTreeUnitTest : IDisposable
    {
        BinarySearchTree<int, int>? tree;

        public BinarySearchTreeUnitTest()
        {
            tree = new BinarySearchTree<int, int>();
        }

        [Fact]
        public void TestInsertSingleItem()
        {
            //Arrange
            var item = (1, 1);
            int treeSize = 0;

            //Act
            tree?.Add(item.Item1,item.Item2);
            treeSize = tree.GetSize();

            //Assert
            Xunit.Assert.Same(item.Item2,tree.Find(1));
        }

        [Fact]
        public void TestRemoveSingleItem()
        {
            //Arrange
            var item = (1, 1);
            tree?.Add(item.Item1, item.Item2);
            int treeSize = tree.GetSize();


            //Act
            tree.Remove(item.Item1);
            treeSize = tree.GetSize();

            //Assert
            Xunit.Assert.True(treeSize == 0);
        }

        public void Dispose()
        {
            tree = null;
        }
    }
}