using Structures.Tree;

namespace TestStructures
{
    public class BinaryTreeNodeTest
    {
        [Theory]
        [MemberData(nameof(GetSonsCountData))]
        public void GetSonsCountTest(BinaryTreeNode<int, int>? leftSon, BinaryTreeNode<int, int>? rightSon, int expectedCount)
        {
            //Arrange
            BinaryTreeNode<int, int> node = new BinaryTreeNode<int, int>(0, 0);
            node.SetLeftSon(leftSon);
            node.SetRightSon(rightSon);
            int actualCount = 0;

            //Act
            actualCount = node.GetSonsCount();

            //Assert
            Assert.True(actualCount == expectedCount, $"Count of the children is {actualCount}, should be {expectedCount}");
        }

        public static IEnumerable<object?[]> GetSonsCountData =>
            new List<object?[]>
            {
                new object?[]
                {
                    new BinaryTreeNode<int,int>(1,1),new BinaryTreeNode<int,int>(2,2), 2
                },
                new object?[]
                {
                    null ,new BinaryTreeNode<int,int>(2,2), 1
                },
                new object?[]
                {
                    new BinaryTreeNode<int,int>(1,1), null, 1
                },
                new object?[]
                {
                    null, null, 0
                }
            };
    }
}
