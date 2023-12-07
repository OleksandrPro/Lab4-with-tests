using Moq;

namespace Lab4.Tests
{
    public class ObjectPoolTests
    {
        [Fact]
        public void Get_ReturnsInactiveObject()
        {
            // Arrange
            var pool = new ObjectPool<FallingObject>(5);

            // Act
            var obj = pool.Get();

            // Assert
            Assert.True(obj.IsActive);
            Assert.NotNull(obj);
        }
        [Fact]
        public void Release_MakesObjectInactive()
        {
            // Arrange
            var pool = new ObjectPool<FallingObject>(5);
            var obj = pool.Get();

            // Act
            pool.Release(obj);

            // Assert
            Assert.False(obj.IsActive);
        }
        [Fact]
        public void Release_ResetsObject()
        {
            // Arrange
            var pool = new ObjectPool<FallingObject>(5);
            var objMock = new Mock<FallingObject>();
            objMock.As<IPoolable>();
            var obj = objMock.Object;
            // Act
            pool.Release(obj);

            // Assert
            objMock.As<IPoolable>().Verify(o => o.Reset(), Times.Once);
        }
        [Fact]
        public void Get_CallingGetWhenPoolIsEmpty_CreatesObjectWhenPoolEmpty()
        {
            // Arrange
            var pool = new ObjectPool<FallingObject>(0);

            // Act
            var obj = pool.Get();

            // Assert
            Assert.NotNull(obj);
            Assert.True(obj.IsActive);
            Assert.Equal(1, pool.Count);
        }
        [Fact]
        public void CreatePoolWithNegativeNumberOfElements_CreatePoolWithNegativeNumberOfElements_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ObjectPool<FallingObject>(-1));
        }
    }
}
