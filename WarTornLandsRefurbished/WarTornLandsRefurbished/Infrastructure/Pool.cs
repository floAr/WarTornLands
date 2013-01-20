using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands.Infrastructure.Systems
{
    public class Pool<T>
    {
        private readonly List<T> _allItems = new List<T>();
        private readonly Queue<T> _freeItems = new Queue<T>();

        private readonly Func<T> _itemFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pool{T}" /> class, for objects with default constructor.
        /// </summary>
        public Pool()
        {
            this._itemFactory = null;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Pool{T}" /> class.
        /// </summary>
        /// <param name="createItemAction">Function returning T to initialize a new object.</param>
        public Pool(Func<T> createItemAction)
        {
            this._itemFactory = createItemAction;
        }

        /// <summary>
        /// Preallocates the specified item count.
        /// </summary>
        /// <param name="itemCount">How many items should be allocated.</param>
        public void Preallocate(int itemCount)
        {
            T[] items = new T[itemCount];
            for (int i = 0; i < itemCount; ++i)
            {
                items[i] = GetFreeItem();
            }

            for (int i = 0; i < itemCount; ++i)
            {
                FlagFreeItem(items[i]);
            }
        }

        /// <summary>
        /// Flags the item as free.
        /// </summary>
        /// <param name="item">The item.</param>
        public void FlagFreeItem(T item)
        {
            _freeItems.Enqueue(item);
        }

        /// <summary>
        /// Gets a free item.
        /// </summary>
        /// <returns>A new or reused item of type T</returns>
        public T GetFreeItem()
        {
            if (_freeItems.Count == 0)
            {
                T item;
                if (_itemFactory != null)
                    item = _itemFactory();
                else
                    item = default(T);
                _allItems.Add(item);
                return item;
            }
            return _freeItems.Dequeue();
        }

        /// <summary>
        /// Gets all managed items.
        /// </summary>
        /// <value>
        /// The items currently in the pool.
        /// </value>
        public List<T> Items
        {
            get { return _allItems; }
        }

        /// <summary>
        /// REset the pool.
        /// </summary>
        public void Clear()
        {
            _allItems.Clear();
            _freeItems.Clear();
        }
    }
}
