using System.Text;
using Structures.File;
using Structures.Interface;
using TestStructures.Wrappers;

namespace TestStructures.HashFile
{
    public class BlockTests
    {
        [Fact]
        public void InsertSingleItem()
        {
            //Arrange
            Block<IntData> block = new Block<IntData>(1);
            IntData data = new IntData(3);
            string expectedString = $"{data.Integer}\n";

            //Act
            block.AddRecord(data);

            //Assert
            Assert.True(block.GetContentsString() == expectedString, $"Actual string {block.GetContentsString()} is different from expected string{expectedString}");
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(1, 10)]
        [InlineData(1, 100)]
        [InlineData(1, 1000)]
        [InlineData(1, 10000)]
        [InlineData(-1, 2)]
        [InlineData(-1, 10)]
        [InlineData(-1, 100)]
        [InlineData(-1, 1000)]
        [InlineData(-1, 10000)]
        [InlineData(46545, 10000)]
        [InlineData(-45645, 10000)]
        [InlineData(0, 10000)]
        public void InsertSingleItemHigherBlockFactor(int item, int capacity)
        {
            Block<IntData> block = new Block<IntData>(capacity);
            IntData data = new IntData(item);
            StringBuilder builder = new StringBuilder();
            builder.Append($"{item}\n");
            for (int index = 1; index < capacity; index++)
            {
                builder.Append($"{0}\n");
            }
            string expectedString = builder.ToString();

            //Act
            block.AddRecord(data);

            //Assert
            Assert.True(block.GetContentsString() == expectedString, $"Actual string {block.GetContentsString()} is different from expected string{expectedString}");
        }

        [Fact]
        public void RemoveSingleItem()
        {
            //Arrange
            Block<IntData> block = new Block<IntData>(1);
            IntData data = new IntData(3);
            string expectedString = "0\n";
            block.AddRecord(data);

            //Act
            block.RemoveRecord(data);

            //Assert
            Assert.True(block.GetContentsString() == expectedString, $"Actual string {block.GetContentsString()} is different from expected string {expectedString}");
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(1, 10)]
        [InlineData(1, 100)]
        [InlineData(1, 1000)]
        [InlineData(1, 10000)]
        [InlineData(-1, 2)]
        [InlineData(-1, 10)]
        [InlineData(-1, 100)]
        [InlineData(-1, 1000)]
        [InlineData(-1, 10000)]
        [InlineData(46545, 10000)]
        [InlineData(-45645, 10000)]
        [InlineData(0, 10000)]
        public void RemoveSingleItemHigherBlockFactor(int item, int capacity)
        {
            Block<IntData> block = new Block<IntData>(capacity);
            IntData data = new IntData(item);
            block.AddRecord(data);
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < capacity; index++)
            {
                builder.Append($"{0}\n");
            }
            string expectedString = builder.ToString();

            //Act
            block.RemoveRecord(data);

            //Assert
            Assert.True(block.GetContentsString() == expectedString, $"Actual string {block.GetContentsString()} is different from expected string{expectedString}");
        }

        [Fact]
        public void FindSingleItem()
        {
            //Arrange
            Block<IntData> block = new Block<IntData>(1);
            IntData data = new IntData(3);
            block.AddRecord(data);

            //Act
            IntData? actualData = block.FindRecord(data);

            //Assert
            Assert.True(actualData == data, $"Actual data {actualData} is different from expected data {data}");
        }

        [Fact]
        public void UpdateSingleItem()
        {
            //Arrange
            Block<IntData> block = new Block<IntData>(1);
            IntData data = new IntData(3);
            block.AddRecord(data);
            IntData update = new IntData(4);

            //Act
            var updateInfo = block.GetRecordForUpdate(data).Value;
            updateInfo.Item1 = update;
            block.UpdateRecord(updateInfo.Item1, updateInfo.Item2);
            IntData? actualData = block.FindRecord(data);
            IntData? actualUpdatedData = block.FindRecord(update);


            //Assert
            Assert.True(actualData == null && actualUpdatedData == update, $"Actual data {actualData} is different from expected data {data}");
        }

        [Theory]
        [InlineData(10,1,11)]
        [InlineData(10,4,5)]
        [InlineData(100,50,96)]
        [InlineData(100,4,83)]
        public void UpdateMultipleItems(int blockFactor, int min, int max)
        {
            //Arrange
            Block<IntData> block = new Block<IntData>(blockFactor);
            for (int item = min; item < max; item++)
            {
                IntData data = new IntData(item);
                block.AddRecord(data);
            }


            //Act
            for (int index = min; index < max; index++)
            {
                IntData update = new IntData(index * -1);
                var updateInfo = block.GetRecordForUpdate(new IntData(index)).Value;
                updateInfo.Item1 = update;
                block.UpdateRecord(updateInfo.Item1, updateInfo.Item2);
            }

            for (int index = min; index < max; index++)
            {
                if (block.FindRecord(new IntData(index)) != null)
                {
                    //This test does not work if 0 is added to the block.
                    Assert.Fail("Old items should not be found.");
                }

                if (block.FindRecord(new IntData(index * - 1)) == null)
                {
                    Assert.Fail("new items should be found.");
                }
            }

            //Assert
            Assert.True(block.ValidCount == max-min, $"Valid count should be {max - min}, actual was {block.ValidCount}.");
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(1, 10)]
        [InlineData(1, 100)]
        [InlineData(1, 1000)]
        [InlineData(1, 10000)]
        [InlineData(-1, 2)]
        [InlineData(-1, 10)]
        [InlineData(-1, 100)]
        [InlineData(-1, 1000)]
        [InlineData(-1, 10000)]
        [InlineData(46545, 10000)]
        [InlineData(-45645, 10000)]
        [InlineData(0, 10000)]
        public void FindSingleItemHigherBlockFactor(int item, int capacity)
        {
            Block<IntData> block = new Block<IntData>(capacity);
            IntData data = new IntData(item);
            block.AddRecord(data);

            //Act
            IntData? actualData = block.FindRecord(data);

            //Assert
            Assert.True(actualData == data, $"Actual data {actualData} is different from expected data {data}");
        }
    }
}
