
namespace WarTornLands.Entities.Modules
{
    public abstract class BaseModule
    {
        protected Entity _owner = null;
        public Entity Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
    }
}
