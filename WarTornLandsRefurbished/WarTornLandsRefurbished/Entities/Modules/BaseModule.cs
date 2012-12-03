
using Microsoft.Xna.Framework;
namespace WarTornLands.Entities.Modules
{
    public abstract class BaseModule
    {
        public Entity Owner { get { return _owner; } }

        protected Entity _owner = null;

        public virtual void SetOwner(Entity owner)
        {
            _owner = owner; 
        }
    }
}
