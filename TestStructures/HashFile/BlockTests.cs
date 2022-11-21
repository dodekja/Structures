using System.Text;
using Structures.File;
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
