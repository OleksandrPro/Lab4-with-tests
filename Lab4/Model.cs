using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab4
{
    public class Model : IModel, IScoreUpdate
    {
        private Controller _controller;
        public Level CurrentLevel { get; private set; }
        public ObjectPool<FallingObject> FallingDamageObjects { get; private set; }
        public ObjectPool<FallingObject> FallingScoreObjects { get; private set; }
        public List<FallingObject> SpawnedDamageObjects { get; set; }
        public List<FallingObject> SpawnedScoreObjects { get; set; }
        private List<IScoreUpdateObserver> _scoreUpdateObservers = new List<IScoreUpdateObserver>();

        public const int HORIZONTAL_UNIT_SIZE = 10;
        public const int VERTICAL_UNIT_SIZE = 250;
        public const int OBJECT_DAMAGE = 1;
        public const int PLAYER_START_HEALTH = 3;
        public const int OBJECT_FALLING_SPEED = 2;
        public const int FALLING_OBJECT_SPAWN_TIME = 2784;
        public const int INITIAL_NUMBER_OF_FALLING_OBJECTS = 8;
        public const int INITIAL_NUMBER_OF_FALLING_SCORE_OBJECTS = 15;
        public const int POINTS_PER_ITEM = 10;
        public const int FALLING_SCORE_OBJECT_SPAWN_TIME = 1200;

        private int _score;
        public int Score 
        { 
            get
            {
                return _score;
            }
            private set
            {
                if (value != _score) 
                {
                    _score = value;
                    ScoreUpdateNotify();
                }
            }
        }
        public Model()
        {
            
        }
        public void Init()
        {
            CurrentLevel = new Level1();
            FallingDamageObjects = new ObjectPool<FallingObject>(INITIAL_NUMBER_OF_FALLING_OBJECTS);
            FallingScoreObjects = new ObjectPool<FallingObject>(INITIAL_NUMBER_OF_FALLING_SCORE_OBJECTS);
            SpawnedDamageObjects = new List<FallingObject>();
            SpawnedScoreObjects = new List<FallingObject>();
            Score = 0;
        }
        public void Attach(IScoreUpdateObserver observer)
        {
            _scoreUpdateObservers.Add(observer);
        }
        public void Detach(IScoreUpdateObserver observer)
        {
            _scoreUpdateObservers.Remove(observer);
        }
        public void ScoreUpdateNotify()
        {
            foreach (var observer in _scoreUpdateObservers)
            {
                observer.Update(this);
            }
        }
        public void AddController(Controller controller)
        {
            _controller = controller;
        }
        private FallingObject TemplateSpawn(ObjectPool<FallingObject> pool, int x, int y, List<FallingObject> result)
        {
            FallingObject newFObj = pool.Get();
            newFObj.SetPosition(x, y);
            result.Add(newFObj);
            return newFObj;
        }
        private void TemplateDespawn(FallingObject obj, ObjectPool<FallingObject> pool, List<FallingObject> list)
        {
            FallingObject toRemove = list.FirstOrDefault(remove => remove.X == obj.X && remove.Y == obj.Y);
            list.Remove(toRemove);
            obj.SetPosition(0, 0);
            pool.Release(toRemove);
        }
        public FallingObject SpawnFallingDamageObject(int x, int y)
        {
            var res = TemplateSpawn(FallingDamageObjects, x, y, SpawnedDamageObjects);
            return res;
        }
        public void DespawnFallingDamageObject(FallingObject obj)
        {
            TemplateDespawn(obj, FallingDamageObjects, SpawnedDamageObjects);
        }        
        public FallingObject SpawnFallingScoreObject(int x, int y)
        {
            var res = TemplateSpawn(FallingScoreObjects, x, y, SpawnedScoreObjects);
            return res;
        }
        public void DespawnFallingScoreObject(FallingObject obj)
        {
            TemplateDespawn(obj, FallingScoreObjects, SpawnedScoreObjects);
        }
        public FallingObject GetFirst()
        {
            return SpawnedDamageObjects.FirstOrDefault();
        }
        public void AddScore()
        {
            Score += POINTS_PER_ITEM;
        }
    }
}
