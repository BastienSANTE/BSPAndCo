using UnityEngine;

public class Room
{
   public int ID;
   public int orx, ory;
   public int width, height;
   public Vector2Int origin;
   public Vector2 center;
   public int area;

   public int endx, endy;

   public Room(int orx, int ory, int w, int h)
   {
      this.orx = orx;
      this.ory = ory;
      this.origin = new Vector2Int(orx, ory);
      this.width = w;
      this.height = h;
      this.area = w * h;
      this.endx = orx + w;
      this.endy = ory + h;
      this.center = new Vector2(orx + w / 2, ory + h / 2);
   }
   
}
