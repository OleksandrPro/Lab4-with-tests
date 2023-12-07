namespace Lab4
{
    public class IdleRight : IPlayerState
    {
        private int _movementCoeffcientX = 0;
        public int MovementCoeffcientX { get { return _movementCoeffcientX; } }
        private readonly PlayerStateMachine _playerStateMachine;
        public IdleRight(PlayerStateMachine psm) 
        {
            _playerStateMachine = psm;
        }
        public void Move()
        {
            _playerStateMachine.Player.X += Model.HORIZONTAL_UNIT_SIZE * _movementCoeffcientX;
        }
        public void BackToIdle()
        {

        }
    }
}
