using System;

namespace Lab4
{
    public interface IControllerLaunch
    {
        void Init(IView view, IModel model);
        void InitTimer(System.Timers.Timer timer);
        void InitRandom(IRandom rand);
        void MovementHandler();
        void RenderLevel();
        void Update();
    }
}
