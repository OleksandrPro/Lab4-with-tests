using Moq;
using Lab4;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System;
using System.Reflection;
using System.Timers;

namespace Lab4.Tests
{
    public class BasicMechanicTests
    {
        private Mock<IView> DefaultSetupMockView()
        {
            var mockView = new Mock<IView>();
            var mockWindow = new Mock<RenderWindow>(It.IsAny<VideoMode>(), It.IsAny<string>());

            mockView.Setup(w => w.GameWindow).Returns(mockWindow.Object);
            mockView.As<IPositionChangeObserver>().Setup(v => v.Update(It.IsAny<IPositionChanged>()));

            return mockView;
        }
        [Fact]
        public void MovementHandler_LeftKeyPressed_PlayerMovesLeftAndItsPositionCorrect()
        {
            //Arrange
            var mockView = DefaultSetupMockView();

            Model model = new Model();
            model.Init();

            Controller controller = new Controller();
            controller.Init(mockView.Object, model);
            int expectedPositionX = new Level1().player.X - Model.HORIZONTAL_UNIT_SIZE;

            //Act
            controller.isAKeyPressed = true;
            controller.MovementHandler();

            //Assert
            int actualPositionX = model.CurrentLevel.player.X;          
            IPlayerState actualPlayerState = model.CurrentLevel.player.CurrentState;

            Assert.Equal(expectedPositionX, actualPositionX);
            Assert.True(actualPlayerState is MovingLeft);
        }
        [Fact]
        public void MovementHandler_RightKeyPressed_PlayerMovesRighItsPositionCorrect()
        {
            //Arrange
            var mockView = DefaultSetupMockView();

            Model model = new Model();
            model.Init();

            Controller controller = new Controller();
            controller.Init(mockView.Object, model);

            int expectedPositionX = new Level1().player.X + Model.HORIZONTAL_UNIT_SIZE;            

            //Act
            controller.isDKeyPressed = true;
            controller.MovementHandler();

            //Assert
            int actualPositionX = model.CurrentLevel.player.X;
            IPlayerState actualPlayerState = model.CurrentLevel.player.CurrentState;

            Assert.Equal(expectedPositionX, actualPositionX);
            Assert.True(actualPlayerState is MovingRight);
        }
        [Fact]
        public void MovementHandler_NoKeyPressed_PlayerStaysIdleItsPositionCorrect()
        {
            //Arrange
            var mockView = DefaultSetupMockView();

            Model model = new Model();
            model.Init();

            Controller controller = new Controller();
            controller.Init(mockView.Object, model);

            int expectedPositionX = new Level1().player.X;

            //Act
            controller.MovementHandler();

            //Assert
            int actualPositionX = model.CurrentLevel.player.X;
            IPlayerState actualPlayerState = model.CurrentLevel.player.CurrentState;

            Assert.Equal(expectedPositionX, actualPositionX);
            Assert.True(actualPlayerState is IdleRight);
        }
        [Fact]
        public void MovementHandler_InputFrom2DifferrentFramesLeftAndRightKeyPressed_PlayerMovesLeftAndStaysIdleItsPositionCorrect()
        {
            //Arrange
            var mockView = DefaultSetupMockView();

            Model model = new Model();
            model.Init();

            Controller controller = new Controller();
            controller.Init(mockView.Object, model);

            int expectedPositionX = new Level1().player.X - Model.HORIZONTAL_UNIT_SIZE;

            //Act
            controller.isAKeyPressed = true;
            controller.MovementHandler();
            controller.isAKeyPressed = true;
            controller.isDKeyPressed = true;
            controller.MovementHandler();

            //Assert
            int actualPositionX = model.CurrentLevel.player.X;
            IPlayerState actualPlayerState = model.CurrentLevel.player.CurrentState;

            Assert.Equal(expectedPositionX, actualPositionX);
            Assert.True(actualPlayerState is IdleLeft);
        }
        [Fact]
        public void MovementHandler_LeftAndRightKeyPressed_PlayerStaysIdleItsPositionCorrect()
        {
            //Arrange
            var mockView = DefaultSetupMockView();

            Model model = new Model();
            model.Init();

            Controller controller = new Controller();
            controller.Init(mockView.Object, model);

            int expectedPositionX = new Level1().player.X;

            //Act
            controller.isAKeyPressed = true;
            controller.isDKeyPressed = true;
            controller.MovementHandler();

            //Assert
            int actualPositionX = model.CurrentLevel.player.X;
            IPlayerState actualPlayerState = model.CurrentLevel.player.CurrentState;

            Assert.Equal(expectedPositionX, actualPositionX);
            Assert.True(actualPlayerState is IdleRight);
        }
        [Fact]
        public void SpawnFallingObject_TimerElapsed_ObjectSpawnedItsPositionCorrect()
        {
            //Arrange
            var mockView = DefaultSetupMockView();
            var mockTimer = new Mock<System.Timers.Timer>();
            var mockRandom = new Mock<IRandom>();
            int setupReturnValue = 200;
            mockRandom.Setup(i=>i.Next(It.IsAny<int>(), It.IsAny<int>())).Returns(setupReturnValue);

            Model model = new Model();
            model.Init();

            Controller controller = new Controller();
            
            controller.Init(mockView.Object, model);
            controller.InitRandom(mockRandom.Object);
            controller.InitTimer(mockTimer.Object);

            int expectedPositionX = setupReturnValue;
            int expectedPositionY = 0;

            //Act
            controller.SpawnFallingDamageObject(this, new EventArgs() as ElapsedEventArgs);

            //Assert
            int actualPositionX = model.SpawnedDamageObjects.First().X;
            int actualPositionY = model.SpawnedDamageObjects.First().Y;

            Assert.Equal(expectedPositionX, actualPositionX);
            Assert.Equal(expectedPositionY, actualPositionY);
        }
    }
}