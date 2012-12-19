using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.Infrastructure.Systems
{
    public class Pool<T>
    {
        private Queue<T> _deadObjects;

        private Func<T> _factory = null;

        public Pool()
        {
            _deadObjects = new Queue<T>();
        }

        public Pool(Func<T> factory)
        {
            _deadObjects = new Queue<T>();
            _factory = factory;
        }
       


        public T AllocateObject()
        {
            if (_deadObjects.Count > 0)
            {
                return _deadObjects.Dequeue();
            }
            else
            {
                if (_factory != null)
                    return _factory.Invoke();
                else
                    return default(T);
            }
        }

        public void GiveBackObject(T deadObject)
        {
            _deadObjects.Enqueue(deadObject);
        }
    }
}
