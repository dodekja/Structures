using Structures.Tree;

namespace TestStructures
{
    public class BinarySearchTreeUnitTest : IDisposable
    {
        public BinarySearchTree<int, int>? Tree { get; private set; }

        public BinarySearchTreeUnitTest()
        {
            Tree = new BinarySearchTree<int, int>();
        }

        [Fact]
        public void InsertSingleItem()
        {
            //Arrange
            var item = (1, 1);
            int treeSize = 0;

            //Act
            Tree?.Add(item.Item1,item.Item2);
            treeSize = Tree.GetSize();

            //Assert
            Assert.True(treeSize == 1, $"Tree size is {treeSize}, should be 1.");
        }

        [Fact]
        public void RemoveSingleItem()
        {
            //Arrange
            var item = (1, 1);
            Tree?.Add(item.Item1, item.Item2);
            int treeSize = Tree.GetSize();


            //Act
            Tree.Remove(item.Item1);
            treeSize = Tree.GetSize();

            //Assert
            Assert.True(treeSize == 0);
        }

        [Fact]
        public void InOrderTraversalEmpty()
        {
            //Arrange
            List<Tuple<int, int>>? list = null;

            //Act
            list = Tree?.InOrder();

            //Assert
            Assert.True(list is { Count: 0 });
        }

        [Fact]
        public void InOrderTraversalSingleItem()
        {
            //Arrange
            List<Tuple<int, int>>? list = null;
            Tuple<int, int> expectedItem = new(1,1);
            Tree?.Add(expectedItem.Item1, expectedItem.Item2);

            //Act
            list = Tree?.InOrder();

            //Assert
            Assert.Contains(expectedItem, list);
        }

        [Theory]
        [MemberData(nameof(InOrderTraversalMultipleItemsData))]
        public void InOrderTraversalMultipleItems(List<Tuple<int, int>> itemsToInsert, List<Tuple<int, int>> expectedList)
        {
            //Arrange
            foreach (Tuple<int, int> tuple in itemsToInsert)
            {
                Tree?.Add(tuple.Item1,tuple.Item2);
            }

            List<Tuple<int, int>>? actualList = null;

            //Act
            actualList = Tree?.InOrder();

            //Assert
            Assert.Equal(expectedList, actualList);
        }

        public static IEnumerable<object[]> InOrderTraversalMultipleItemsData =>
            new List<object[]>
            {
                new object[]
                {
                    new List<Tuple<int,int>> {new(5,5), new(10,10), new(11,11), new(20,20), new(30,30), new(40,40), new(45,45)},
                    new List<Tuple<int,int>> {new(5,5), new(10,10), new(11,11), new(20,20), new(30,30), new(40,40), new(45,45)}
                },
                new object[] {new List<Tuple<int,int>> {new(5,5), new(15,15), new(18,18), new(20,20), new(25,25), new(30,30), new(35,35),
                    new(40, 40), new(45, 45), new(50, 50), new(60, 60) },
                    new List<Tuple<int,int>> {new(5,5), new(15,15), new(18,18), new(20,20), new(25,25), new(30,30), new(35,35),
                        new(40, 40), new(45, 45), new(50, 50), new(60, 60) }
                },
                new object[] {new List<Tuple<int,int>> {new(30,30), new(20,20), new(40,40), new(15,15), new(25,25), new(35,35), new(50,50),
                        new(5, 5), new(18, 18), new(45, 45), new(60, 60) },
                    new List<Tuple<int,int>> {new(5,5), new(15,15), new(18,18), new(20,20), new(25,25), new(30,30), new(35,35),
                        new(40, 40), new(45, 45), new(50, 50), new(60, 60) }
                },

            };

        public void Dispose()
        {
            Tree = null;
        }
    }
}