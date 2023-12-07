﻿namespace Lab4
{
    public interface IPoolable
    {
        bool IsActive { get; }
        void Reset();
        void SetActive(bool b);
    }
}
