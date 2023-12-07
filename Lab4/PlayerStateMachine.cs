using System;
using System.Collections.Generic;

namespace Lab4
{
    public class PlayerStateMachine
    {
        private Dictionary<Type, IPlayerState> _states;
        public IPlayerState _currentState;
        public Player Player { get; private set; }
        public Type State { get; private set; }

        public delegate void StateChanged(object sender, EventArgs e);
        public event StateChanged NewState;

        public PlayerStateMachine()
        {
            _states = new Dictionary<Type, IPlayerState>()
            {
                [typeof(IdleRight)] = new IdleRight(this),
                [typeof(IdleLeft)] = new IdleLeft(this),
                [typeof(MovingRight)] = new MovingRight(this),
                [typeof(MovingLeft)] = new MovingLeft(this)
            };
        }
        public void InitPlayer(Player p)
        {
            Player = p;
        }
        public void EnterIn<TState>() where TState : IPlayerState
        {
            if(_states.TryGetValue(typeof(TState), out IPlayerState state))
            {
                _currentState = state;
                State = state.GetType();
                NewState?.Invoke(this, new EventArgs());
            }
        } 
    }
}
