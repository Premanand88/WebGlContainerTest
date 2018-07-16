using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.ReferenceClass
{
    public class WallAdded
    {
        public GameObject wall { set; get; }

        public float wallPos { set; get; }

        public bool Vertical { set; get; }

        public List<WallAdded> RemoveItem(GameObject wall, List<WallAdded> wallsList)
        {
            List<WallAdded> removeItem = new List<WallAdded>();
            foreach (WallAdded wallItem in wallsList)
            {

                try
                {
                    if (wall == wallItem.wall)
                    {
                        removeItem.Add(wallItem);
                    }
                }
                catch
                {
                    Debug.Log("Error");
                }
            }
            foreach (WallAdded wallItem in removeItem)
            {
                wallsList.Remove(wallItem);
            }
            return wallsList;
        }
    }
}
