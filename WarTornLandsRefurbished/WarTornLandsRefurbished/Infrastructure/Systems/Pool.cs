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

        /// <summary>
        /// Initializes a new instance of the <see cref="Pool{T}" /> class.
        /// </summary>
        public Pool()
        {
            _deadObjects = new Queue<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pool{T}" /> class.
        /// </summary>
        /// <param name="factory">Parameterless factory method, which returns a new instance of type T</param>
        public Pool(Func<T> factory)
        {
            _deadObjects = new Queue<T>();
            _factory = factory;
        }
       

        /// <summary>
        /// Pull a new object of type T from the pool
        /// </summary>
        /// <returns>Reused or new T</returns>
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

        /// <summary>
        /// Returns object of type T into the pool
        /// </summary>
        /// <param name="deadObject">The object which will be thown back into the pool</param>
        public void GiveBackObject(T deadObject)
        {
            _deadObjects.Enqueue(deadObject);
        }
    }
}
