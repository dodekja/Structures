using Structures.File;
using TestStructures.Generator;
using TestStructures.Wrappers;

namespace TestStructures.HashFile
{
    public class HashFileTests
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(10, 1)]
        [InlineData(100, 1)]
        [InlineData(1000, 1)]
        [InlineData(2, 2)]
        [InlineData(10, 2)]
        [InlineData(100, 2)]
        [InlineData(1000, 2)]
        public void InsertSingleItem(int blockFactor, int numberOfBlocks)
        {
            //Arrange
            string filename = "test.dat";
            using StaticHashFile<IntData> staticHashFile = new StaticHashFile<IntData>(filename, blockFactor, numberOfBlocks);
            IntData data = new IntData(Random.Shared.Next());

            //Act
            staticHashFile.Insert(data);
            var actual = staticHashFile.Find(data);

            //Assert
            Assert.True(actual?.IsEqual(data), "Data not found");
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(10, 1)]
        [InlineData(100, 1)]
        [InlineData(1000, 1)]
        [InlineData(2, 2)]
        [InlineData(10, 2)]
        [InlineData(100, 2)]
        [InlineData(1000, 2)]
        public void RemoveSingleItem(int blockFactor, int numberOfBlocks)
        {
            //Arrange
            string filename = "test.dat";
            using StaticHashFile<IntData> staticHashFile = new StaticHashFile<IntData>(filename, blockFactor, numberOfBlocks);
            IntData data = new IntData(Random.Shared.Next());
            staticHashFile.Insert(data);

            //Act
            staticHashFile.Delete(data);
            var actual = staticHashFile.Find(data);

            //Assert
            Assert.True(actual == null, "Data not removed found");
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(10, 1)]
        [InlineData(100, 1)]
        [InlineData(1000, 1)]
        [InlineData(2, 2)]
        [InlineData(10, 2)]
        [InlineData(100, 2)]
        [InlineData(1000, 2)]
        public void FindSingleItem(int blockFactor, int numberOfBlocks)
        {
            //Arrange
            string filename = "test.dat";
            using StaticHashFile<IntData> staticHashFile = new StaticHashFile<IntData>(filename, blockFactor, numberOfBlocks);
            IntData data = new IntData(Random.Shared.Next());
            staticHashFile.Insert(data);

            //Act
            var actual = staticHashFile.Find(data);

            //Assert
            Assert.True(actual?.IsEqual(data), "Expected and actual items are not equal");
        }

        [Theory]
        //should pass
        [InlineData(100, 20, 1000, 500, 500)]
        [InlineData(15, 200, 1000, 500, 500)]
        [InlineData(10, 20,  100, 50, 50)]
        [InlineData(10, 20, 100, 90, 50)]
        [InlineData(1000, 50, 10000, 900, 50)]
        [InlineData(100, 30000, 1000000, 90000, 50000)]
        [InlineData(50, 300000 ,1000000, 90000, 50000)]
        [InlineData(100, 50 ,1000, 50, 50000)]
        [InlineData(100,40 , 1000, 900, 500)]
        [InlineData(100, 50 ,1000, 486, 500)]
        [InlineData(10000, 50 ,100000, 40860, 50000)]

        public void IntegrationTests(int blockFactor, int numberOfBlocks, int addCount, int removeCount, int getCount, int seed = 0)
        {
            //Arrange
            using StaticHashFile<IntData> staticHashFile = new StaticHashFile<IntData>("test.dat", blockFactor, numberOfBlocks);
            var generator = new OperationsGenerator(addCount, removeCount, getCount, seed);

            //Act
            for (long index = 0; index < addCount + removeCount + getCount; index++)
            {
                var operation = generator.GenerateOperation();
                switch (operation)
                {
                    case Operations.Add:
                        IntData data = new IntData(generator.GenerateKey(operation));
                        try
                        {
                            staticHashFile.Insert(data);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Assert.Fail("These test should not have collisions");
                        }
                        break;
                    case Operations.Remove:
                        try
                        {
                            staticHashFile.Delete(new IntData(generator.GenerateKey(operation)));
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Assert.Fail("Key not found");
                        }
                        break;
                    case Operations.Get:
                        staticHashFile.Find(new IntData(generator.GenerateKey(operation)));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            foreach (int operationKey in generator.KeysForOperations)
            {
                if (staticHashFile.Find(new IntData(operationKey)) == null)
                {
                    Assert.Fail("Key not found");
                }
            }

            //Assert
            Assert.True(true);
        }

        [Theory]
        //should pass
        [InlineData(10, 10, 1000, 500, 500)]
        [InlineData(1, 1, 1000, 500, 500)]
        public void IntegrationTestsErrors(int blockFactor, int numberOfBlocks, int addCount, int removeCount, int getCount, int seed = 0)
        {
            //Arrange
            using StaticHashFile<IntData> staticHashFile = new StaticHashFile<IntData>("test.dat", blockFactor, numberOfBlocks);
            var generator = new OperationsGenerator(addCount, removeCount, getCount, seed);
            bool insertError = false;
            bool removeError = false;
            bool findError = false;

            //Act
            for (long index = 0; index < addCount + removeCount + getCount; index++)
            {
                var operation = generator.GenerateOperation();
                switch (operation)
                {
                    case Operations.Add:
                        IntData data = new IntData(generator.GenerateKey(operation));
                        try
                        {
                            staticHashFile.Insert(data);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Assert.True(true,"These test should have collisions");
                            insertError = true;
                        }
                        break;
                    case Operations.Remove:
                        try
                        {
                            staticHashFile.Delete(new IntData(generator.GenerateKey(operation)));
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Assert.True(true, "If a collision occurs, not finding data to remove is possible");
                            removeError = true;
                        }
                        break;
                    case Operations.Get:
                        if (staticHashFile.Find(new IntData(generator.GenerateKey(operation))) == null)
                        {
                            Assert.True(true, "If a collision occurs, not finding data is possible");
                            findError = true;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            Assert.True(insertError || removeError || findError, "No error occurred, check test data");
        }
    }
}
