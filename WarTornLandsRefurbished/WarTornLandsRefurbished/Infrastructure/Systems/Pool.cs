using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.Infrastructure.Systems
{
   public class Pool<T> where T:new()
    {
       private Queue<T> _deadObjects;


       public Pool()
       {
           _deadObjects = new Queue<T>();
       }

       public T AllocateObject()
       {
           if (_deadObjects.Count > 0)
           {
               return _deadObjects.Dequeue();
           }
           else
           {
               return new T();
           }
       }

       public void GiveBackObject(T deadObject)
       {
           _deadObjects.Enqueue(deadObject);
       }


    }
}
