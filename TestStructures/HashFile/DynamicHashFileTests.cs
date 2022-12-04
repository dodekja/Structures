using Structures.File;
using Structures.Interface;
using TestStructures.Generator;
using TestStructures.Generator.Helpers;
using TestStructures.Wrappers;

namespace TestStructures.HashFile
{
    public class DynamicHashFileTests
    {
        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void InsertSingleItem(int blockFactor)
        {
            //Arrange
            string filename = "dynamicTest.dat";
            using DynamicHashFile<IntData> dynamicHashFile = new DynamicHashFile<IntData>(filename, blockFactor);
            IntData data = new IntData(Random.Shared.Next());

            //Act
            dynamicHashFile.Insert(data);
            var actual = dynamicHashFile.Find(data);

            //Assert
            Assert.True(actual?.IsEqual(data), "Data not found");
        }

        [Theory]
        [InlineData(10,5)]
        [InlineData(10,10)]
        [InlineData(10,15)]
        [InlineData(10,20)]
        [InlineData(100,20)]
        [InlineData(100,100)]
        [InlineData(100,110)]
        [InlineData(100,2000)]
        [InlineData(1000,600)]
        [InlineData(1000,1000)]
        [InlineData(1000,1100)]
        [InlineData(1000,2000)]
        [InlineData(1000,10000)]
       // [InlineData(1000,1000000)]
        public void InsertMultipleItems(int blockFactor, int numberOfItems)
        {
            //Arrange
            string filename = "dynamicTest.dat";
            using DynamicHashFile<IntData> dynamicHashFile = new DynamicHashFile<IntData>(filename, blockFactor);

            int counter = 0;

            //Act
            for (int i = 0; i < numberOfItems; i++)
            {
                IntData data = new IntData(i);
                dynamicHashFile.Insert(data);
                var actual = dynamicHashFile.Find(data);
                if (actual.Integer == i)
                {
                    counter++;
                }
            }

            var something = dynamicHashFile.GetAllBlockContents();
            File.Delete(filename);
            //Assert
            Assert.True(counter == numberOfItems, "Data not found");
        }

        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void RemoveSingleItem(int blockFactor)
        {
            //Arrange
            string filename = "dynamicTest.dat";
            using DynamicHashFile<IntData> dynamicHashFile = new DynamicHashFile<IntData>(filename, blockFactor);
            IntData data = new IntData(Random.Shared.Next());
            dynamicHashFile.Insert(data);

            //Act
            dynamicHashFile.Delete(data);
            var actual = dynamicHashFile.Find(data);

            //Assert
            Assert.True(actual == null, "Data not removed");
        }

        [Theory]
        [InlineData(10, 5)]
        [InlineData(10, 10)]
        [InlineData(10, 15)]
        [InlineData(10, 20)]
        [InlineData(100, 20)]
        [InlineData(100, 100)]
        [InlineData(100, 110)]
        [InlineData(100, 2000)]
        [InlineData(1000, 600)]
        [InlineData(1000, 1000)]
        [InlineData(1000, 1100)]
        [InlineData(1000, 2000)]
        [InlineData(1000, 10000)]
        public void RemoveMultipleItem(int blockFactor, int numberOfItems)
        {
            //Arrange
            string filename = "dynamicTest.dat";
            using DynamicHashFile<IntData> dynamicHashFile = new DynamicHashFile<IntData>(filename, blockFactor);
            int counter = 0;
            for (int i = 0; i < numberOfItems; i++)
            {
                IntData data = new IntData(i);
                dynamicHashFile.Insert(data);
                counter++;
            }

            //Act
            for (int i = 0; i < numberOfItems; i++)
            {
                IntData data = new IntData(i);
                dynamicHashFile.Delete(data);
                var actual = dynamicHashFile.Find(data);
                if (actual == null)
                {
                    counter--;
                }
            }

            //Assert
            Assert.True(counter == 0, "Some data were not removed successfully");
        }

        [Theory]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public void FindSingleItem(int blockFactor)
        {
            //Arrange
            string filename = "dynamicTest.dat";
            using DynamicHashFile<IntData> dynamicHashFile = new DynamicHashFile<IntData>(filename, blockFactor);
            IntData data = new IntData(Random.Shared.Next());
            dynamicHashFile.Insert(data);

            //Act
            var actual = dynamicHashFile.Find(data);

            //Assert
            Assert.True(actual?.IsEqual(data), "Expected and actual items are not equal");
        }

        [Theory]
        //should pass
        [InlineData(100, 1000, 500, 500)]
        [InlineData(10, 1000, 500, 500)]
        [InlineData(10, 100, 50, 50)]
        [InlineData(10, 100, 90, 50)]
        [InlineData(1000, 10000, 900, 50)]
        [InlineData(100, 1000000, 90000, 50000)]
        [InlineData(50, 1000000, 90000, 50000)]
        [InlineData(100, 1000, 50, 50000)]
        [InlineData(100, 1000, 900, 500)]
        //[InlineData(1000, 1000000, 50000, 50000)]
        //[InlineData(1000, 1000000, 500000, 50)]
        //[InlineData(1000, 1000000, 500, 500000)]
        public void IntegrationTests(int blockFactor, int addCount, int removeCount, int getCount, int seed = 0)
        {
            //Arrange
            using DynamicHashFile<IntData> dynamicHashFile = new DynamicHashFile<IntData>("dynamicTest", blockFactor);
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
                            dynamicHashFile.Insert(data);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Assert.Fail("These test should not have collisions");
                        }
                        break;
                    case Operations.Remove:
                        try
                        {
                            dynamicHashFile.Delete(new IntData(generator.GenerateKey(operation)));
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Assert.Fail("Key not found");
                        }
                        break;
                    case Operations.Get:
                        dynamicHashFile.Find(new IntData(generator.GenerateKey(operation)));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            //var something = dynamicHashFile.GetAllBlockContents();

            foreach (int operationKey in generator.KeysForOperations)
            {
                if (dynamicHashFile.Find(new IntData(operationKey)) == null)
                {
                    Assert.Fail($"Key {operationKey} not found");
                }
            }

            File.Delete("dynamicTest");

            //Assert
            Assert.True(true);
        }
    }
}
