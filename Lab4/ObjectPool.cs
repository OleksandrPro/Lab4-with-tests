using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Lab4
{
    public class ObjectPool<T> where T : IPoolable, new()
    {
        public int Count { get; private set; }
        private List<T> _objects;

        public ObjectPool(int initialNumberOfObjects)
        {
            if (initialNumberOfObjects < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            _objects = new List<T>();
            for (int i = 0; i < initialNumberOfObjects; i++)
            {
                T obj = Create();
                obj.SetActive(false);
            }
            Count = _objects.Count;
        }
        public T Get()
        {
            var obj = _objects.FirstOrDefault(x => !x.IsActive);
            if (obj == null)
            {
                obj = Create();
            }
            obj.SetActive(true);
            return obj;
        }

        private T Create()
        {
            T obj = new T();
            _objects.Add(obj);
            Count = _objects.Count;
            return obj;
        }

        public void Release(T obj)
        {
            obj.Reset();
        }
    }
}
