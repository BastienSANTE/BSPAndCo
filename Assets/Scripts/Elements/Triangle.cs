using UnityEditor;
 using UnityEngine;
 using Vector2 = UnityEngine.Vector2;
 using Vector3 = UnityEngine.Vector3;

 public class Triangle
 {
     public Point p1, p2, p3;
     public bool IsSuperTri;
     public bool isDelaunay;
     public Vector2 Circumcenter;
     public float CircumRad;
     //public float Circ
     
     public Triangle(Point p1, Point p2, Point p3)
     {
         this.p1 = p1;
         this.p2 = p2;
         this.p3 = p3;
         Circumcenter = GetCircumCenter(this);
         CircumRad = GetCircumRad(this);
     }
     
     
     public Vector2 GetCircumCenter(Triangle t)
     {
         //Calculate the circumcenter coordinates using the midpoint formula
         float ax, ay, bx, by, cx, cy, d, circumX, circumY;
         ax = t.p1.x; ay = t.p1.y;
         bx = t.p2.x; by = t.p2.y;
         cx = t.p3.x; cy = t.p3.y;

         d = 2 * (ax * (by - cy) + bx * (cy - ay) + cx * (ay - by));

         circumX = ((ax * ax + ay * ay) * (by - cy) + (bx * bx + by * by) * (cy - ay) + (cx * cx + cy * cy) * (ay - by)) / d;
         circumY = ((ax * ax + ay * ay) * (cx - bx) + (bx * bx + by * by) * (ax - cx) + (cx * cx + cy * cy) * (bx - ax)) / d;

         return new Vector2(circumX, circumY);
     }

     public float GetCircumRad(Triangle t)
     {
         return Vector2.Distance(t.Circumcenter, new Vector2(p1.x, p1.y));
     }
 }