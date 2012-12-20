using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WarTornLands.Infrastructure.Systems.Camera2D
{
    class GridInfo<T> where T:ISpatial
    {
        public Rectangle GridRect;
        public List<T> GridItems;
 
        public GridInfo(int x, int y, int width, int height)
        {
            GridRect = new Rectangle(x, y, width, height);
            GridItems = new List<T>();
        }
    }
    public class SpatialGrid<T> where T:ISpatial
    { 
        List<GridInfo<T>> gridInfos;
        public SpatialGrid()
        {
            gridInfos = new List<GridInfo<T>>();
        }

        public void RegisterItem(T item)
        {
            GridInfo<T> targetGridInfo = null;
 
            int gridInfoCount = gridInfos.Count;
            for (int i = 0; i < gridInfoCount; ++i)
            {
                if (gridInfos[i].GridRect.Contains((int)item.Position.X, (int)item.Position.Y))
                {
                    targetGridInfo = gridInfos[i];
                    break;
                }
            }
 
            if (targetGridInfo == null)
            {
                int gridStartX = ((int)item.Position.X / Constants.GRID_CELL_WIDTH) * Constants.GRID_CELL_WIDTH;
                int gridStartY = ((int)item.Position.Y / Constants.GRID_CELL_HEIGHT) * Constants.GRID_CELL_HEIGHT;

                targetGridInfo = new GridInfo<T>(gridStartX, gridStartY, Constants.GRID_CELL_WIDTH, Constants.GRID_CELL_HEIGHT);
                gridInfos.Add(targetGridInfo);
            }
            System.Diagnostics.Debug.Assert(targetGridInfo != null);
            targetGridInfo.GridItems.Add(item);
        }

        public void GetItemsInRect(ref List<T> items, Rectangle rect,bool appendToList=false)
        {
            if (!appendToList)
                items.Clear();
            int gridInfoCount = gridInfos.Count;
            for (int i = 0; i < gridInfoCount; ++i)
            {
                if (rect.Intersects(gridInfos[i].GridRect))
                {
                    int itemCount = gridInfos[i].GridItems.Count;
                    for (int j = 0; j < itemCount; ++j)
                    {
                        if (rect.Intersects(gridInfos[i].GridItems[j].BoundingRect))
                        {
                            if (!gridInfos[i].GridItems[j].ToBeRemoved)
                                items.Add(gridInfos[i].GridItems[j]);
                            else
                                gridInfos[i].GridItems.Remove(gridInfos[i].GridItems[j]);
                        }
                    }
                }
            }

        }

        public void GetAllItems(ref List<T> items)
        {
             int gridInfoCount = gridInfos.Count;
            for (int i = 0; i < gridInfoCount; ++i)
            {
                items.AddRange(gridInfos[i].GridItems);
            }
        }
    }
}
