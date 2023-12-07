using System;
using SFML.Graphics;
using System.Collections.Generic;

namespace Lab4
{
    public interface IView
    {
        RenderWindow GameWindow { get; }
        UI UI { get; }
        Sprite CurrentPlayerModel { get; }
        List<Sprite> Platforms { get; }
        List<Sprite> FallingDamageObjects { get; }
        List<Sprite> FallingScoreObjects { get; }

        LinkedList<Sprite> CurrentAnimation { get; }
        LinkedList<Sprite> IdleRightAnimation { get; }
        LinkedList<Sprite> IdleLeftAnimation { get; }
        LinkedList<Sprite> MovingRightAnimation { get; }
        LinkedList<Sprite> MovingLeftAnimation { get; }
        Dictionary<Type, LinkedList<Sprite>> StateAnimationPairs { get; }
        void Init(RenderWindow window);
        void AddController(Controller controller);
        void DrawScene();
        void LoadLevel(Level level);
        void UpdateAnimation(Player p);
        void UpdateFallingObjectPosition(int y);
        void AddFallingObject(FallingObject fObj);
        void AddFallingScoreObject(FallingObject fObj);
        void RemoveFallingObjectSprite(int x, int y);
        void RemoveFallingScoreObjectSprite(int x, int y);
        void SetEndGameScreen();
    }
}