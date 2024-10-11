using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
   public int ID;
   public Vector2Int origin;
   public Vector2 center;
   public int width, height;
   public int area;

   public Room(Vector2Int origin, int w, int h)
   {
      this.origin = origin;
      this.width = w;
      this.height = h;
      this.area = w * h;
   }
   
}
