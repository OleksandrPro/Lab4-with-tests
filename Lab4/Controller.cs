using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Timers;

namespace Lab4
{
    public class Controller : IControllerLaunch, IControllerView, IStateObserver
    {
        public IView View { get; private set; }
        public IModel Model { get; private set; }

        private IRandom _random;
        private Timer _fallingObjectsTimer;
        private Timer _fallingScoreObjectsTimer;
        private Player _player;
        private Level _currentLevel;

        public bool isAKeyPressed = false;
        public bool isDKeyPressed = false;
        public bool _isNotGameOver = true;
        public Controller() { }
        public void Init(IView view, IModel model)
        {
            View = view;
            Model = model;

            _random = new CustomRandom(new Random());
            _fallingObjectsTimer = new Timer(Lab4.Model.FALLING_OBJECT_SPAWN_TIME);
            _fallingScoreObjectsTimer = new Timer(Lab4.Model.FALLING_SCORE_OBJECT_SPAWN_TIME);

            _player = model.CurrentLevel.player;
            _currentLevel = model.CurrentLevel;

            _fallingObjectsTimer.Enabled = true;
            _fallingScoreObjectsTimer.Enabled = true;
            InitializeEventSubscriptions(View, Model);
        }
        public void InitRandom(IRandom rand)
        {
            _random = rand;
        }
        public void InitTimer(Timer timer)
        {
            _fallingObjectsTimer = timer;
        }
        private void InitializeEventSubscriptions(IView view, IModel model)
        {
            View.GameWindow.KeyPressed += OnKeyPressedHorizontal;
            View.GameWindow.KeyReleased += OnKeyReleasedHorizontal;
            View.GameWindow.KeyPressed += OnKeyPressedVertical;
            _fallingObjectsTimer.Elapsed += SpawnFallingDamageObject;
            _fallingScoreObjectsTimer.Elapsed += SpawnFallingScoreObject;
            _player.Attach((IPositionChangeObserver)View);
            _player.Attach((IHealthEventObserver)View.UI);
            _player.Attach((IStateObserver)this);
            ((IScoreUpdate)Model).Attach((IScoreUpdateObserver)View.UI);
            _player.Died += EndGame;
        }
        private void UnsubscribeFromEvents()
        {
            View.GameWindow.KeyPressed -= OnKeyPressedHorizontal;
            View.GameWindow.KeyReleased -= OnKeyReleasedHorizontal;
            View.GameWindow.KeyPressed -= OnKeyPressedVertical;
            _fallingObjectsTimer.Elapsed -= SpawnFallingDamageObject;
            _fallingScoreObjectsTimer.Elapsed -= SpawnFallingScoreObject;
            _player.Detach((IPositionChangeObserver)View);
            _player.Detach((IHealthEventObserver)View.UI);
            _player.Detach((IStateObserver)this);
            ((IScoreUpdate)Model).Detach((IScoreUpdateObserver)View.UI);
            _player.Died -= EndGame;
        }
        public void OnKeyPressedHorizontal(object sender, EventArgs e)
        {
            if (((KeyEventArgs)e).Code == Keyboard.Key.A)
            {
                isAKeyPressed = true;
            }
            if (((KeyEventArgs)e).Code == Keyboard.Key.D)
            {
                isDKeyPressed = true;
            }
        }
        public void OnKeyReleasedHorizontal(object sender, EventArgs e)
        {
            if (((KeyEventArgs)e).Code == Keyboard.Key.A)
            {
                isAKeyPressed = false;
            }
            if (((KeyEventArgs)e).Code == Keyboard.Key.D)
            {
                isDKeyPressed = false;
            }
        }
        public void OnKeyPressedVertical(object sender, EventArgs e)
        {
            int movementCoeff = 0;
            if (((KeyEventArgs)e).Code == Keyboard.Key.W)
            {
                movementCoeff = -1;
            }
            if (((KeyEventArgs)e).Code == Keyboard.Key.S)
            {
                movementCoeff = 1;
            }
            MovePlayerVertical(Lab4.Model.VERTICAL_UNIT_SIZE * movementCoeff);
        }
        public void Update(IStateUpdate subject)
        {
            View.UpdateAnimation(_player);
        }        
        public void MovementHandler()
        {
            if (!isAKeyPressed && !isDKeyPressed)
            {
                if (_player.CurrentState is MovingLeft)
                {
                    _player.ChangeState<IdleLeft>();
                }
                else if (_player.CurrentState is MovingRight)
                {
                    _player.ChangeState<IdleRight>();
                }
            }
            else if (isAKeyPressed && isDKeyPressed)
            {
                _player.BackToIdle();
            }
            else if (isAKeyPressed)
            {
                _player.ChangeState<MovingLeft>();
            }
            else if (isDKeyPressed)
            {
                _player.ChangeState<MovingRight>();
            }
            MovePlayerHorizontal();
        }
        public void SpawnFallingDamageObject(Object source, ElapsedEventArgs e)
        {
            int randomObjPos = _random.Next(0, 1250);
            FallingObject fObj = Model.SpawnFallingDamageObject(randomObjPos, 0);
            View.AddFallingObject(fObj);
        }
        public void SpawnFallingScoreObject(Object source, ElapsedEventArgs e)
        {
            int randomObjPos = _random.Next(0, 1250);
            FallingObject fObj = Model.SpawnFallingScoreObject(randomObjPos, 0);
            View.AddFallingScoreObject(fObj);
        }
        public FloatRect GetColiderOfModel()
        {
            return View.CurrentPlayerModel.GetGlobalBounds();
        }
        private void MovePlayerHorizontal()
        {
            bool possibleCollision = PreUpdateX(_player);
            if (possibleCollision)
            {
                return;
            }
            _player.MoveHorizontal();
        }
        private void MovePlayerVertical(int y)
        {
            bool possibleCollision = PreUpdateY(_player, y);
            if (possibleCollision)
            {
                return;
            }
            _player.MoveVertical(y);
        }
        private bool PreUpdateX(Player p)
        {
            FloatRect newCollider = p.Collider;
            newCollider.Left += p.CurrentState.MovementCoeffcientX * Lab4.Model.HORIZONTAL_UNIT_SIZE;
            return CheckPlayerPossibleCollision(newCollider);
        }
        private bool PreUpdateY(Player p, int posChange)
        {
            FloatRect newCollider = p.Collider;
            newCollider.Top += posChange;
            return CheckPlayerPossibleCollision(newCollider);
        }
        private bool CheckPlayerPossibleCollision(FloatRect playerCollider)
        {
            foreach (var item in _currentLevel.platforms)
            {
                bool willCollide = Engine.isIntersect(playerCollider, item.Collider);
                if (willCollide)
                {
                    return true;
                }
            }
            foreach (var item in _currentLevel.barrier)
            {
                bool willCollide = Engine.isIntersect(item, playerCollider);
                if (willCollide)
                {
                    return true;
                }
            }
            return false;
        }
        private FallingObject CheckGameObjectsCollision(List<FallingObject> list)
        {
            FallingObject toDestroy = null;
            foreach (var item in list)
            {
                bool isCollide = Engine.isIntersect(_player.Collider, item.Collider);
                if (isCollide)
                {
                    toDestroy = item;
                    return toDestroy;                                       
                }
            }
            return toDestroy;
        }
        private void CheckDamageObjectsCollision()
        {
            FallingObject fObj = CheckGameObjectsCollision(Model.SpawnedDamageObjects);
            if(fObj!=null)
            {
                int x = fObj.X;
                int y = fObj.Y;
                Model.DespawnFallingDamageObject(fObj);
                View.RemoveFallingObjectSprite(x, y);
                _player.ApplyDamage();
            }
        }
        private void CheckScoreObjectsCollision()
        {
            FallingObject fObj = CheckGameObjectsCollision(Model.SpawnedScoreObjects);
            if (fObj != null)
            {
                int x = fObj.X;
                int y = fObj.Y;
                Model.DespawnFallingScoreObject(fObj);
                View.RemoveFallingScoreObjectSprite(x, y);
                Model.AddScore();
            }
        }
        public void RenderLevel()
        {
            View.LoadLevel(_currentLevel);
        }
        public void AddPlayerCollider()
        {
            _player.Collider = GetColiderOfModel();
        }
        public void AddPlatformCollider(Platform p, FloatRect collider)
        {
            p.Collider = collider;
        }
        public void AddBarrier(int x, int y, int height, int width)
        {
            _currentLevel.barrier.Add(new FloatRect(x, y, height, width));
        }       
        public void AddFallingObjectCollider(FallingObject fObj, FloatRect collider)
        {
            fObj.Collider = collider;
            Engine.InitCollider(fObj.Collider, fObj.X, fObj.Y, fObj.Collider.Height, fObj.Collider.Width);
        }
        private FallingObject UpdateFallingObjectsPossition(List<FallingObject> list)
        {
            FallingObject toDestroy = null;
            foreach (var item in list)
            {
                item.IncreaseVerticalSpeed(Lab4.Model.OBJECT_FALLING_SPEED);
                item.UpdateColliderPosition();
                if (item.Y >= View.GameWindow.Size.Y)
                {
                    toDestroy = item;
                }
            }
            return toDestroy;
        }
        public void Update()
        {
            CheckObjectsCollision(Model.SpawnedDamageObjects, _player.ApplyDamage, View.RemoveFallingObjectSprite, Model.DespawnFallingDamageObject);
            CheckObjectsCollision(Model.SpawnedScoreObjects, Model.AddScore, View.RemoveFallingScoreObjectSprite, Model.DespawnFallingScoreObject);
            FallingObject toDestroy = UpdateFallingObjectsPossition(Model.SpawnedDamageObjects);
            if (toDestroy != null)
            {
                Model.DespawnFallingDamageObject(toDestroy);
            }
            toDestroy = UpdateFallingObjectsPossition(Model.SpawnedScoreObjects);
            if (toDestroy != null)
            {
                Model.DespawnFallingScoreObject(toDestroy);
            }
            View.UpdateFallingObjectPosition(Lab4.Model.OBJECT_FALLING_SPEED);
        }

        private void CheckObjectsCollision(List<FallingObject> list, Action onCollision, Action<int, int> removeSprite, Action<FallingObject> despawnObject)
        {
            var resItem = list.FirstOrDefault();
            bool isCollide = false;
            foreach (var item in list.ToList())
            {
                resItem = item;
                isCollide = Engine.isIntersect(_player.Collider, item.Collider);
                if (isCollide)
                {
                    int x = resItem.X;
                    int y = resItem.Y;

                    despawnObject(resItem);
                    removeSprite(x, y);
                    onCollision();
                    return;
                }
            }           
        }
        public void EndGame(object sender, EventArgs e)
        {
            _isNotGameOver = false;
            _player.BackToIdle();
            UnsubscribeFromEvents();
            View.SetEndGameScreen();
        }
    }
}