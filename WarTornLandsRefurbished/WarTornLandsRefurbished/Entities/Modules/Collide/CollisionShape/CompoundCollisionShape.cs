using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.Entities.Modules.Collide.CollisionShape
{
   public class CompoundCollisionShape:ICollisionShape
    {
       private List<ICollisionShape> _shapes;
       public List<ICollisionShape> Shapes { get { return _shapes; } set { _shapes = value; } }
       public CompoundCollisionShape()
       {
           _shapes = new List<ICollisionShape>();
       }
       public CompoundCollisionShape( IEnumerable<ICollisionShape> shapes)
       {
          _shapes=new List<ICollisionShape>();
          _shapes.AddRange(shapes);
       }
       public void AddCollisionShape(ICollisionShape shape)
       {
           _shapes.Add(shape);
       }

    }
}
