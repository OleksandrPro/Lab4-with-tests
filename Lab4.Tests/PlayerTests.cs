using Xunit;
using Moq;

namespace Lab4.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void MoveHorizontal_HorizontalMovement_CallsMoveOnCurrentState()
        {
            // Arrange
            var player = new Player(0, 0);
            var mockState = new Mock<IPlayerState>();
            player.CurrentState = mockState.Object;

            // Act
            player.MoveHorizontal();

            // Assert
            mockState.Verify(s => s.Move(), Times.Once);
        }
        [Fact]
        public void ApplyDamage_PlayerGetsDamage_UpdatesHealthAndNotifiesHealthObservers()
        {
            // Arrange
            var player = new Player(0, 0);
            var mockObserver = new Mock<IHealthEventObserver>();
            player.Attach(mockObserver.Object);
            var initialHealth = player.Health;

            // Act
            player.ApplyDamage();

            // Assert
            Assert.Equal(initialHealth - Model.OBJECT_DAMAGE, player.Health);
            mockObserver.Verify(o => o.Update(player), Times.Once);
        }
        [Fact]
        public void ChangeState_SetPlayerState_ObserversNotificationOccur()
        {
            // Arrange
            var player = new Player(0, 0);            
            PlayerStateMachine psm = new PlayerStateMachine();
            psm.InitPlayer(player);
            player.InitPSM(psm);
            var mockObserver = new Mock<IStateObserver>();
            player.Attach(mockObserver.Object);

            // Act
            player.ChangeState<MovingRight>();

            // Assert
            mockObserver.Verify(o => o.Update(player), Times.Once);
        }
        [Fact]
        public void ChangePostion_ChangePlayerPositionInHorizontalDirection_ObserversNotificationOccur()
        {
            // Arrange
            var player = new Player(0, 0);            
            PlayerStateMachine psm = new PlayerStateMachine();
            psm.InitPlayer(player);
            player.InitPSM(psm);
            var mockObserver = new Mock<IPositionChangeObserver>();
            player.Attach(mockObserver.Object);

            // Act
            player.ChangeState<MovingRight>();
            player.MoveHorizontal();

            // Assert
            mockObserver.Verify(o => o.Update(player), Times.Once);
        }
        [Fact]
        public void ChangePostion_ChangePlayerPositionInVerticalDirection_ObserversNotificationOccur()
        {
            // Arrange
            var player = new Player(0, 0);
            var mockObserver = new Mock<IPositionChangeObserver>();
            player.Attach(mockObserver.Object);

            // Act
            player.MoveVertical(Model.VERTICAL_UNIT_SIZE);

            // Assert
            mockObserver.Verify(o => o.Update(player), Times.Once);
        }
        [Fact]
        public void BackToIdle_PlayerEntersInIdleStateFromCurrentState_CallsBackToIdleOnCurrentState()
        {
            // Arrange
            var player = new Player(0, 0);
            var mockState = new Mock<IPlayerState>();
            player.CurrentState = mockState.Object;

            // Act
            player.BackToIdle();

            // Assert
            mockState.Verify(s => s.BackToIdle(), Times.Once);
        }
    }
}
