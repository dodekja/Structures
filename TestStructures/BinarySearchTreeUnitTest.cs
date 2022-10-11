using System.Security.Principal;
using Structures.Tree;
using TestStructures.Generator;

namespace TestStructures
{
    public class BinarySearchTreeUnitTest : IDisposable
    {
        public BinarySearchTree<long, long>? Tree { get; private set; }

        public BinarySearchTreeUnitTest()
        {
            Tree = new BinarySearchTree<long, long>();
        }

        [Fact]
        public void FindSingleItem()
        {
            //Arrange
            var item = (1, 1);
            Tree?.Add(item.Item1, item.Item2);

            //Act
            var data = Tree.Find(item.Item1);

            //Assert
            Assert.True(data == 1, $"Item found was {data}, should be {item.Item1}.");
        }
        
        [Fact]
        public void FindEmptyTree()
        {
            //Arrange Act and Assert
            Assert.Throws<ArgumentException>(() => Tree!.Find(0));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(100000)]
        public void FindMultipleRandomItems(int count)
        {
            //Arrange
            var generator = new Random();
            int index;
            long actualCount = 0;
            long key;
            bool found = false;
            List<long> keysList = new List<long>(count);

            for (long i = 0; i < count; i++)
            {
                key = generator.NextInt64();
                Tree.Add(key, key);
                keysList.Add(key);
            }

            //Act
            for (int i = 0; i < count; i++)
            {
                key = keysList[generator.Next(0, keysList.Count)];
                var data = Tree.Find(key);
                if (data == key)
                {
                    found = true;
                }
                else
                {
                    found = false;
                }
            }
            
            //Assert
            Assert.True(found);

        }

        [Fact]
        public void InsertSingleItem()
        {
            //Arrange
            var item = (1, 1);
            long treeSize = 0;

            //Act
            Tree?.Add(item.Item1, item.Item2);
            treeSize = Tree!.GetSize();

            //Assert
            Assert.True(treeSize == 1, $"Tree size is {treeSize}, should be 1.");
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(50000)]
        public void InsertMultipleRandomItems(int count)
        {
            //Arrange
            var generator = new Random();
            int actualCount = 0;
            long key;

            //Act
            for (long i = 0; i < count; i++)
            {
                key = generator.NextInt64();
                Tree.Add(key,key);
            }
            actualCount = Tree.InOrder().Count();

            //Assert
            Assert.True(Tree.Count == count && actualCount == count);
        }

        [Fact]
        public void RemoveSingleItem()
        {
            //Arrange
            var item = (1, 1);
            Tree!.Add(item.Item1, item.Item2);
            int treeSize;


            //Act
            long data = Tree.Remove(item.Item1);
            treeSize = Tree.GetSize();

            //Assert
            Assert.True(treeSize == 0 && data == 1);
        }

        [Fact]
        public void RemoveEmptyTree()
        {
            //Arrange Act and Assert
            Assert.Throws<InvalidOperationException>(() => Tree!.Remove(0));
        }

        [Theory]
        [MemberData(nameof(RemoveSpecialCasesData))]
        public void RemoveSpecialCases(List<long> keys, long keyToRemove)
        {
            //Arrange
            int expectedCount = (keys.Count - 1);
            int actualCount;
            foreach (var key in keys)
            {
                Tree.Add(key, key);
            }

            //Act
            Tree.Remove(keyToRemove);
            actualCount = Tree.InOrder().Count;

            //Assert
            Assert.True(Tree.Count == expectedCount && actualCount == expectedCount, $"Expected tree size is {expectedCount}, actual was {actualCount}");
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        [InlineData(50000)]
        public void RemoveMutlipleRandomItems(int count)
        {
            //Arrange
            var generator = new Random();
            int index;
            long actualCount = 0;
            long key;
            List<long> keysList = new List<long>(count);

            for (long i = 0; i < count; i++)
            {
                key = generator.NextInt64();
                Tree.Add(key, key);
                keysList.Add(key);
            }

            //Act
            for (long j = 0; j < count; j++)
            {
                index = generator.Next(0, keysList.Count);
                key = keysList[index];
                Tree.Remove(key);
                keysList.Remove(key);
            }
            actualCount = Tree.InOrder().Count();

            //Assert
            Assert.True(Tree.Count == 0 && actualCount == 0, $"Expected count 0, actual was {actualCount}");
        }

        [Fact]
        public void InOrderTraversalEmpty()
        {
            //Arrange
            List<Tuple<long, long>>? list;

            //Act
            list = Tree?.InOrder();

            //Assert
            Assert.True(list is { Count: 0 });
        }

        [Fact]
        public void InOrderTraversalSingleItem()
        {
            //Arrange
            List<Tuple<long, long>>? list = null;
            Tuple<long, long> expectedItem = new(1, 1);
            Tree?.Add(expectedItem.Item1, expectedItem.Item2);

            //Act
            list = Tree?.InOrder();

            //Assert
            Assert.Contains(expectedItem, list);
        }

        [Theory]
        [MemberData(nameof(InOrderTraversalMultipleItemsData))]
        public void InOrderTraversalMultipleItems(List<Tuple<long, long>> itemsToInsert, List<Tuple<long, long>> expectedList)
        {
            //Arrange
            foreach (Tuple<long, long> tuple in itemsToInsert)
            {
                Tree?.Add(tuple.Item1, tuple.Item2);
            }

            List<Tuple<long, long>> actualList;

            //Act
            actualList = Tree!.InOrder();

            //Assert
            Assert.Equal(expectedList, actualList);
        }

        [Theory]
        [MemberData(nameof(IntegrationTestsData))]
        public void IntegrationTests(int addCount, int removeCount, int getCount, int? seed = null)
        {
            //Arrange
            TreeOperationsGenerator generator;
            generator = new TreeOperationsGenerator(addCount, removeCount, getCount, seed);
            long countOfOperations = addCount + removeCount + getCount;
            long key = 0;
            TreeOperations operation;
            
            //Act

            for (long index = 0; index < countOfOperations; index++)
            {
                operation = generator.GenerateOperation();
                switch (operation)
                {
                    case TreeOperations.Add:
                        key = generator.GenerateKey(operation);
                        Tree.Add(key, key);
                        break;
                    case TreeOperations.Remove:
                        Tree.Remove(generator.GenerateKey(operation));
                        break;
                    case TreeOperations.Get:
                        Tree.Find(generator.GenerateKey(operation));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            Assert.True(Tree.Count == addCount - removeCount, $"Actual Count was {Tree.Count}, expected {addCount - removeCount}");
        }

        public static IEnumerable<object[]> IntegrationTestsData =>
            new List<object[]>
            {
                new object[]
                {
                    10, 5, 10, 1
                },
                new object[]
                {
                    100, 50, 100, 1
                },
                new object[]
                {
                    10000, 5000, 10000, 1
                },
                new object[]
                {
                    100000, 50000, 100000, 1
                },
                
            };

        public static IEnumerable<object[]> InOrderTraversalMultipleItemsData =>
            new List<object[]>
            {
                new object[]
                {
                    new List<Tuple<long,long>> {new(5,5), new(10,10), new(11,11), new(20,20), new(30,30), new(40,40), new(45,45)},
                    new List<Tuple<long,long>> {new(5,5), new(10,10), new(11,11), new(20,20), new(30,30), new(40,40), new(45,45)}
                },
                new object[] {new List<Tuple<long,long>> {new(5,5), new(15,15), new(18,18), new(20,20), new(25,25), new(30,30), new(35,35),
                    new(40, 40), new(45, 45), new(50, 50), new(60, 60) },
                    new List<Tuple<long,long>> {new(5,5), new(15,15), new(18,18), new(20,20), new(25,25), new(30,30), new(35,35),
                        new(40, 40), new(45, 45), new(50, 50), new(60, 60) }
                },
                new object[] {new List<Tuple<long,long>> {new(30,30), new(20,20), new(40,40), new(15,15), new(25,25), new(35,35), new(50,50),
                        new(5, 5), new(18, 18), new(45, 45), new(60, 60) },
                    new List<Tuple<long,long>> {new(5,5), new(15,15), new(18,18), new(20,20), new(25,25), new(30,30), new(35,35),
                        new(40, 40), new(45, 45), new(50, 50), new(60, 60) }
                },

            };

        public static IEnumerable<object[]> RemoveSpecialCasesData =>
            new List<object[]>
            {
                //1
                new object[]
                {
                    new List<long> {1},1
                },
                //2
                new object[]
                {
                    new List<long> {2,1},2
                },
                //3
                new object[]
                {
                    new List<long> {2,3},2
                },
                //4
                new object[]
                {
                    new List<long> {2,1,3},2
                },
                //5
                new object[]
                {
                    new List<long> {5,1,10,7},5
                },
                //6
                new object[]
                {
                    new List<long> {5,15,10,20},15
                },
                //7
                new object[]
                {
                    new List<long> {20,10,5,15},10
                },
                //8
                new object[]
                {
                    new List<long> {100,10,5,50,30,25},10
                },
                //9
                new object[]
                {
                    new List<long> {100,10,5},10
                },
                //10
                new object[]
                {
                    new List<long> {10,100,50},100
                },
                //11
                new object[]
                {
                    new List<long> {10,50,100},50
                },
                //12
                new object[]
                {
                    new List<long> {50,10,25},10
                },
                //13
                new object[]
                {
                    new List<long> {50,10},10
                },
                //13
                new object[]
                {
                    new List<long> {10,50},50
                },
            };

        public void Dispose()
        {
            Tree = null;
        }
    }
}