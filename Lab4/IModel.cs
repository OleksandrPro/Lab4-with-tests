using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab4
{
    public interface IModel
    {
        ObjectPool<FallingObject> FallingDamageObjects { get; }
        ObjectPool<FallingObject> FallingScoreObjects { get; }
        List<FallingObject> SpawnedDamageObjects { get; }
        List<FallingObject> SpawnedScoreObjects { get; }
        int Score { get; }
        Level CurrentLevel { get;}
        void Init();
        void AddController(Controller controller);
        FallingObject SpawnFallingDamageObject(int x, int y);

        void DespawnFallingDamageObject(FallingObject obj);
        FallingObject SpawnFallingScoreObject(int x, int y);

        void DespawnFallingScoreObject(FallingObject obj);
        void AddScore();
        FallingObject GetFirst();
    }
}
