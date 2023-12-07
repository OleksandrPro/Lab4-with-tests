﻿using SFML.Graphics;

namespace Lab4
{
    public interface IControllerView
    {
        FloatRect GetColiderOfModel();
        void AddPlayerCollider();
        void AddPlatformCollider(Platform p, FloatRect collider);
        void AddBarrier(int x, int y, int height, int width);
        void AddFallingObjectCollider(FallingObject fObj, FloatRect collider);
    }
}
