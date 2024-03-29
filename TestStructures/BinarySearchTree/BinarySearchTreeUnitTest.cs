using Structures.Tree;
using TestStructures.Generator;

namespace TestStructures.BinarySearchTree
{
    public class BinarySearchTreeUnitTest : IDisposable
    {
        public BinarySearchTree<long, long>? Tree { get; private set; }

        public BinarySearchTreeUnitTest()
        {
            Tree = new BinarySearchTree<long, long>();
        }

        [Fact]
        public void RotateSingleItemLeft()
        {
            //Arrange
            var item = (1, 1);
            Tree.Add(item.Item1, item.Item2);

            //Act
            Tree.RotateNodeLeft(Tree.FindNode(item.Item1));

            //Assert
            Assert.True(Tree.Count == 1 && Tree.InOrder().Count == 1, $"Expected tree size is 1, actual was {Tree.InOrder().Count}");
        }

        [Fact]
        public void RotateSingleItemRight()
        {
            //Arrange
            var item = (1, 1);
            Tree.Add(item.Item1, item.Item2);

            //Act
            Tree.RotateNodeRight(Tree.FindNode(item.Item1));

            //Assert
            Assert.True(Tree.Count == 1 && Tree.InOrder().Count == 1, $"Expected tree size is 1, actual was {Tree.InOrder().Count}");
        }

        [Theory]
        [MemberData(nameof(MultipleItemRotations))]
        public void RotateMultipleItemsLeft(int[] keys, int keyToRotate)
        {
            //Arrange
            foreach (var key in keys)
            {
                Tree.Add(key, key);
            }

            //Act
            Tree.RotateNodeLeft(Tree.FindNode(keyToRotate));

            //Assert
            Assert.True(Tree.Count == keys.Length && Tree.InOrder().Count == keys.Length, $"Expected tree size is {keys.Length}, actual was {Tree.InOrder().Count}");
        }

        [Theory]
        [MemberData(nameof(MultipleItemRotations))]
        public void RotateMultipleItemsRight(int[] keys, int keyToRotate)
        {
            //Arrange
            foreach (var key in keys)
            {
                Tree.Add(key, key);
            }

            //Act
            Tree.RotateNodeRight(Tree.FindNode(keyToRotate));

            //Assert
            Assert.True(Tree.Count == keys.Length && Tree.InOrder().Count == keys.Length, $"Expected tree size is {keys.Length}, actual was {Tree.InOrder().Count}");
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
        public void FindRangeEmtpyTree()
        {
            //Arrange && Act
            var result = Tree.FindRange(0, 10);

            //Assert
            Assert.True(result is { Count: 0 });
        }


        [Theory]
        [MemberData(nameof(FindRangeTestData))]
        public void FindRangeManualInput(List<(long, long)> data, int start, int end)
        {
            //Arrange
            Tree.InsertRange(data);

            //Act
            var result = Tree.FindRange(start, end);

            //Assert
            bool equals = true;
            int controlIndex;
            int subtreeDifference = 0;
            for (controlIndex = 0; controlIndex < result.Count; controlIndex++)
            {
                if (result[controlIndex] >= start && result[controlIndex] <= end)
                {
                    equals = true;
                }
                else
                {
                    equals = false;
                    break;
                }
            }

            Assert.True(equals, $"The list contains an item that is outside the range <{start};{end}> at index: {controlIndex}");
        }

        [Theory]
        [InlineData(0, 10, 2, 4)]
        [InlineData(0, 1000, 20, 400)]
        [InlineData(0, 1000, 20, 900)]
        [InlineData(-1000, 1000, -200, 900)]
        [InlineData(0, 1000, -100, 2000)]
        [InlineData(0, 100, -100, 50)]
        [InlineData(0, 100, 50, 200)]
        public void FindRangeGeneratedInput(int itemsStart, int itemsEnd, int rangeStart, int rangeEnd)
        {
            //Arrange
            for (int itemIndex = itemsStart; itemIndex < itemsEnd; itemIndex++)
            {
                Tree.Add(itemIndex, itemIndex);
            }

            //Act
            var result = Tree.FindRange(rangeStart, rangeEnd);

            //Assert
            bool equals = true;
            int controlIndex;
            for (controlIndex = 0; controlIndex < result.Count; controlIndex++)
            {
                if (result[controlIndex] >= rangeStart && result[controlIndex] <= rangeEnd)
                {
                    equals = true;
                }
                else
                {
                    equals = false;
                    break;
                }
            }

            Assert.True(equals, $"The list contains an item that is outside the range <{rangeStart};{rangeEnd}> at index: {controlIndex}");
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
        [InlineData(10, 0)]
        [InlineData(100, 0)]
        [InlineData(1000, 0)]
        [InlineData(10000, 0)]
        [InlineData(100000, 0)]
        [InlineData(1000000, 0)]
        [InlineData(10000000, 0)]
        public void InsertWithBalance(int countOfNodes, int? seed)
        {
            //Arrange && Act
            List<BinaryTreeNode<long, long>> actualList = new List<BinaryTreeNode<long, long>>(countOfNodes);
            Random random = new Random(seed ?? 0);
            for (int index = 0; index < countOfNodes; index++)
            {
                int number = random.Next();
                try
                {
                    Tree.AddWithBalance(number, number);
                }
                catch (Exception)
                {
                    index--;
                }
            }

            //Assert
            actualList = Tree.LevelOrder();
            bool equals = true;
            int controlIndex;
            int subtreeDifference = 0;
            for (controlIndex = 0; controlIndex < actualList.Count; controlIndex++)
            {
                subtreeDifference = actualList[controlIndex].GetSubtreeDifference();
                if (subtreeDifference == 1 || subtreeDifference == 0 || subtreeDifference == -1)
                {
                    equals = true;
                }
                else
                {
                    equals = false;
                    break;
                }
            }
            Assert.True(equals, $"Actual and expected lists differ at index {controlIndex} with subtree difference {subtreeDifference}");
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
            int actualCount;
            long key;

            //Act
            for (long i = 0; i < count; i++)
            {
                key = generator.NextInt64();
                Tree.Add(key, key);
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
            int expectedCount = keys.Count - 1;
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
        public void RemoveMultipleRandomItems(int count)
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
        public void LevelOrderTraversalEmpty()
        {
            //Arrange
            List<BinaryTreeNode<long, long>> list;

            //Act
            list = Tree?.LevelOrder();

            //Assert
            Assert.True(list is { Count: 0 });
        }

        [Fact]
        public void LevelOrderTraversalSingleItem()
        {
            //Arrange
            List<BinaryTreeNode<long, long>> list;
            Tree.Add(1, 1);

            //Act
            list = Tree?.LevelOrder();

            //Assert
            Assert.True(list is { Count: 1 });
        }

        [Theory]
        [MemberData(nameof(LevelOrderTraversalMultipleItemsData))]
        public void LevelOrderTraversalMultipleItems(List<BinaryTreeNode<long, long>> itemsToInsert, List<BinaryTreeNode<long, long>> expectedList)
        {
            //Arrange
            List<BinaryTreeNode<long, long>> actualList;
            foreach (BinaryTreeNode<long, long> item in itemsToInsert)
            {
                Tree.Add(item.Key, item.Data);
            }

            //Act
            actualList = Tree?.LevelOrder();


            //Assert
            bool equals = true;
            int index;
            for (index = 0; index < actualList.Count; index++)
            {
                if (actualList[index].Key == expectedList[index].Key)
                {
                    equals = true;
                }
                else
                {
                    equals = false;
                    break;
                }
            }
            Assert.True(equals, $"The actual list differs from the expected at index {index}.");
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
            var generator = new OperationsGenerator(addCount, removeCount, getCount, seed);
            long key = 0;
            int operationsPerformed = addCount - removeCount;

            Operations operation;

            //Act

            for (long index = 0; index < addCount + removeCount + getCount; index++)
            {
                operation = generator.GenerateOperation();
                switch (operation)
                {
                    case Operations.Add:
                        key = generator.GenerateKey(operation);
                        try
                        {
                            Tree.Add(key, key);
                        }
                        catch (ArgumentException)
                        {
                            operationsPerformed--;
                        }
                        break;
                    case Operations.Remove:
                        try
                        {
                            Tree.Remove(generator.GenerateKey(operation));
                        }
                        catch (InvalidOperationException)
                        {
                            operationsPerformed++;
                        }
                        break;
                    case Operations.Get:
                        try
                        {
                            Tree.Find(generator.GenerateKey(operation));
                        }
                        catch (ArgumentException)
                        {
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            Assert.True(Tree.Count == operationsPerformed, $"Actual Count was {Tree.Count}, expected {operationsPerformed}");
        }

        [Theory]
        [MemberData(nameof(InsertRangeTestData))]
        public void InsertRangeManual(List<(long, long)> data)
        {
            //Arrange
            List<BinaryTreeNode<long, long>>? actualList;

            //Act
            Tree.InsertRange(data);
            actualList = Tree?.LevelOrder();

            //Assert
            bool equals = true;
            int controlIndex;
            int subtreeDifference = 0;
            for (controlIndex = 0; controlIndex < actualList.Count; controlIndex++)
            {
                subtreeDifference = actualList[controlIndex].GetSubtreeDifference();
                if (subtreeDifference == 1 || subtreeDifference == 0 || subtreeDifference == -1)
                {
                    equals = true;
                }
                else
                {
                    equals = false;
                    break;
                }
            }
            Assert.True(equals, $"Actual and expected lists differ at index {controlIndex} with subtree difference {subtreeDifference}");
        }

        [Theory]
        [InlineData(-1000000, 0)]
        [InlineData(-100000, 0)]
        [InlineData(-10000, 0)]
        [InlineData(-1000, 0)]
        [InlineData(-100, 0)]
        [InlineData(-10, 0)]
        [InlineData(-1, 0)]
        [InlineData(0, 0)]
        [InlineData(0, 1)]
        [InlineData(0, 10)]
        [InlineData(0, 100)]
        [InlineData(0, 1000)]
        [InlineData(0, 10000)]
        [InlineData(0, 100000)]
        [InlineData(0, 1000000)]
        [InlineData(0, 10000000)]
        [InlineData(-3, 10000000)]
        public void InsertRangeGenerated(int start, int end)
        {
            //Arrange
            List<BinaryTreeNode<long, long>>? actualList;
            List<(long, long)> data = new List<(long, long)>(end - start);
            for (int index = start; index < end; index++)
            {
                data.Add(new(index, index));
            }

            //Act
            Tree.InsertRange(data);
            actualList = Tree?.LevelOrder();

            //Assert
            bool equals = true;
            int controlIndex;
            int subtreeDifference = 0;
            for (controlIndex = 0; controlIndex < actualList.Count; controlIndex++)
            {
                subtreeDifference = actualList[controlIndex].GetSubtreeDifference();
                if (subtreeDifference == 1 || subtreeDifference == 0 || subtreeDifference == -1)
                {
                    equals = true;
                }
                else
                {
                    equals = false;
                    break;
                }
            }
            Assert.True(equals, $"Actual and expected lists differ at index {controlIndex} with subtree difference {subtreeDifference}");
        }

        [Fact]
        public void BalanceEmptyTree()
        {
            //Arrange
            Exception? ex;

            //Act
            ex = Record.Exception(() => Tree.Balance());

            //Assert
            Assert.Null(ex);
        }

        [Theory]
        [MemberData(nameof(BalanceTreeData))]
        public void BalanceTreeManualInput(List<BinaryTreeNode<long, long>> insertNodes, List<long> expectedKeysOrder)
        {
            //Arrange
            List<BinaryTreeNode<long, long>> actualList;
            foreach (BinaryTreeNode<long, long> item in insertNodes)
            {
                Tree.Add(item.Key, item.Data);
            }

            //Act
            Tree.Balance();
            actualList = Tree?.LevelOrder();

            //Assert
            bool equals = true;
            int controlIndex;
            int subtreeDifference = 0;
            for (controlIndex = 0; controlIndex < actualList.Count; controlIndex++)
            {
                subtreeDifference = actualList[controlIndex].GetSubtreeDifference();
                if (subtreeDifference == 1 || subtreeDifference == 0 || subtreeDifference == -1)
                {
                    equals = true;
                }
                else
                {
                    equals = false;
                    break;
                }
            }
            Assert.True(equals, $"Actual and expected lists differ at index {controlIndex} with subtree difference {subtreeDifference}");
        }

        [Theory]
        [InlineData(10, 0)]
        [InlineData(100, 0)]
        [InlineData(1000, 0)]
        [InlineData(10000, 0)]
        [InlineData(100000, 0)]
        [InlineData(1000000, 0)]
        public void BalanceTreeGeneratedInput(int countOfNodes, int? seed)
        {
            //Arrange
            List<BinaryTreeNode<long, long>> actualList = new List<BinaryTreeNode<long, long>>(countOfNodes);
            Random random = new Random(seed ?? 0);
            for (int index = 0; index < countOfNodes; index++)
            {
                int number = random.Next();
                try
                {
                    Tree.Add(number, number);
                }
                catch (Exception)
                {
                    index--;
                }
            }

            //Act
            Tree.Balance();
            actualList = Tree?.LevelOrder();

            //Assert
            bool equals = true;
            int controlIndex;
            int subtreeDifference = 0;
            for (controlIndex = 0; controlIndex < actualList.Count; controlIndex++)
            {
                subtreeDifference = actualList[controlIndex].GetSubtreeDifference();
                if (subtreeDifference == 1 || subtreeDifference == 0 || subtreeDifference == -1)
                {
                    equals = true;
                }
                else
                {
                    equals = false;
                    break;
                }
            }
            Assert.True(equals, $"Actual and expected lists differ at index {controlIndex} with subtree difference {subtreeDifference}");

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
                new object[]
                {
                    10000000, 5000000, 10000000, 1
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
                    new List<Tuple<long, long>> {new(5,5), new(15,15), new(18,18), new(20,20), new(25,25), new(30,30), new(35,35),
                        new(40, 40), new(45, 45), new(50, 50), new(60, 60) }
                },

            };

        public static IEnumerable<object[]> BalanceTreeData =>
            new List<object[]>
            {
                new object[]
                {
                    new List<BinaryTreeNode<long,long>> {new(5,5), new(10,10), new(11,11), new(20,20), new(30,30), new(40,40), new(45,45)},
                    new List<long> {20,10,40,5,11,30,45}
                },
                new object[]
                {
                    new List<BinaryTreeNode<long,long>> {new(45,45), new(40, 40), new(30, 30), new(20, 20), new(11, 11), new(10, 10), new(5,5) },
                    new List<long> {20,10,40,5,11,30,45}
                },
                new object[]
                {
                    new List<BinaryTreeNode<long,long>> {new(45,45), new(5, 5), new(40, 40), new(10, 10), new(30, 30), new(11, 11), new(20, 20)   },
                    new List<long> {11,5,30,10,20,45,40}
                },
            };

        public static IEnumerable<object[]> LevelOrderTraversalMultipleItemsData =>
            new List<object[]>
            {
                new object[]
                {
                    new List<BinaryTreeNode<long,long>> {new(5,5), new(10,10), new(11,11), new(20,20), new(30,30), new(40,40), new(45,45)},
                    new List<BinaryTreeNode<long,long>> {new(5,5), new(10,10), new(11,11), new(20,20), new(30,30), new(40,40), new(45,45)}
                },
                new object[] {new List<BinaryTreeNode<long,long>> {new(5,5), new(15,15), new(18,18), new(20,20), new(25,25), new(30,30), new(35,35),
                        new(40, 40), new(45, 45), new(50, 50), new(60, 60) },
                    new List<BinaryTreeNode<long,long>> {new(5,5), new(15,15), new(18,18), new(20,20), new(25,25), new(30,30), new(35,35),
                        new(40, 40), new(45, 45), new(50, 50), new(60, 60) }
                },
                new object[] {new List<BinaryTreeNode<long,long>> {new(30,30), new(20,20), new(40,40), new(15,15), new(25,25), new(35,35), new(50,50),
                        new(5, 5), new(18, 18), new(45, 45), new(60, 60) },
                    new List<BinaryTreeNode<long,long>> {new(30,30), new(20,20), new(40,40), new(15,15), new(25,25), new(35,35), new(50,50),
                        new(5, 5), new(18, 18), new(45, 45), new(60, 60) }
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

        public static IEnumerable<object[]> MultipleItemRotations =>
            new List<object[]>
            {
                new object[]
                {
                    new[] {1,2},2
                },
                new object[]
                {
                    new[] {1,2},1
                },
                new object[]
                {
                    new[] {2,1},1
                },
                new object[]
                {
                    new[] {2,1},2
                },
                new object[]
                {
                    new[] {2,1,3},2
                },
                new object[]
                {
                    new[] {2,1,3},1
                },
                new object[]
                {
                    new[] {2,1,3},3
                },
            };

        public static IEnumerable<object[]> InsertRangeTestData =>
            new List<object[]>
            {
                new object[]
                {
                    new List<(long,long)> {(1,1)},
                },
                new object[]
                {
                    new List<(long,long)> {(1,1), (2, 2), (3, 3), (4, 4)}
                },
                new object[]
                {
                    new List<(long,long)> {  (4, 4), (3, 3), (2, 2), (1,1)}
                }
            };

        public static IEnumerable<object[]> FindRangeTestData =>
            new List<object[]>
            {
                new object[]
                {
                    new List<(long,long)> {(1,1)},
                    0,
                    1
                },
                new object[]
                {
                    new List<(long,long)> {(1,1), (2, 2), (3, 3), (4, 4)},
                    0,
                    2
                },
                new object[]
                {
                    new List<(long,long)> {  (4, 4), (3, 3), (2, 2), (1,1)},
                    0,
                    2
                }
            };

        public void Dispose()
        {
            Tree = null;
        }
    }
}