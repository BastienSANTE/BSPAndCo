using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/*Delaunay triangulation algorithm
 1. Get the center of each room created by the BSP, and put them in a list of points.
 2.For each point, create a circle passing by 2 points, and containing NO other points in it, aka an "empty" triangle
     Verify that the triangle is "empty", by calculating the center of the triangle, and if any points are within
     the radius, using a vector norm.
 3. Draw
 */

public class Delaunay : MonoBehaviour
{
    private Display _display;
    private List<Point> _pointList;
    private Triangle _superTriangle;
    private List<Triangle> _triList;
    private int _depth;
    
    public void DelaunayExec(List<Room> rooms)
    {
        _display = this.GetComponent<Display>();            //Get the main component for the BSP
        _pointList = GetPoints(_display.roomList);          //Create a list of points from the BSP rooms
        _superTriangle = CreateSuperTriangle(_display);     //Create supertriangle from the display bounds
        _triList = new List<Triangle>{_superTriangle};
        _depth = 5;                          //Set recursion depth to number of points in list
        
        //Create 2 lists of points, respectively for used and unused points
        List<Point> usedPoints   =   new List<Point>();
        List<Point> unusedPoints = _pointList.ToList();
        
       _triList = Triangulate(_triList , _pointList, _depth);
       
       DrawTriangles(_triList, Color.blue, 5);
    }

    
    private List<Point> GetPoints(List<Room> rooms)
    {
        List<Point> pointList = new List<Point>();
        
        foreach (Room r in rooms)
        {
            pointList.Add(new Point(r.center));
        }

        return pointList;
    }

    private Triangle CreateSuperTriangle(Display disp)
    {
        Triangle triangle = new Triangle(
            new Point(new Vector2(disp.originX, disp.originY - disp.height)),
            new Point(new Vector2(disp.originX, disp.originY + 2f * disp.height)),
            new Point(new Vector2(disp.originX + 2f * disp.width, disp.originY + disp.height / 2f)));
        
        return triangle;
    }

    private void DrawTriangles(List<Triangle> tl, Color c, float duration)
    {
        foreach (Triangle t in tl)
        {
            Debug.DrawLine(new Vector3(t.p1.x, t.p1.y, 0), new Vector3(t.p2.x, t.p2.y, 0), c, duration);
            Debug.DrawLine(new Vector3(t.p2.x, t.p2.y, 0), new Vector3(t.p3.x, t.p3.y, 0), c, duration);
            Debug.DrawLine(new Vector3(t.p3.x, t.p3.y, 0), new Vector3(t.p1.x, t.p1.y, 0), c, duration);
        }
    }
    

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        if(_pointList.Count < 1)
            return;
        
        foreach (Point p in _pointList)
        {
            Gizmos.DrawWireSphere(new Vector3(p.x, p.y, 0), .4f);
        }

        /*foreach (Triangle t in _triList)
        {
            Gizmos.DrawWireSphere(new Vector3(t.Circumcenter.x, t.Circumcenter.y), t.CircumRad);
        }*/
    }
    
    private List<Triangle> Triangulate(List<Triangle> triangles, List<Point> points, int depth)
    {
        
        /*Check the circumcircle of each triangle for Delaunay Conformity (no other points in CC)
        If a triangle is found with a non-empty circumcircle : 
            Create 3 new triangles from this one
            Delete former triangle*/
        
        //This code ugly AF, but I don't have any time left
        
        if (depth == 0)
        {
            return triangles;
        }

        Debug.Log($"There are {triangles.Count} triangles");

        foreach (Triangle t in triangles.ToList())
        {
            if (t.isDelaunay)
                break;
            
            foreach (Point p in points.ToList())
            {
                if (Vector2.Distance(t.Circumcenter, p.position) < t.CircumRad - .01f)
                {
                    Debug.Log($"Triangle {triangles.IndexOf(t)} is not Delaunay");
                    Triangle subTri1 = new Triangle(
                        new Point(new Vector2(t.p1.x, t.p1.y)), new Point(new Vector2(t.p2.x, t.p2.y)), new Point(new Vector2(p.x, p.y)));
                        triangles.Add(subTri1);
                    Triangle subTri2 = new Triangle(
                        new Point(new Vector2(t.p2.x, t.p2.y)), new Point(new Vector2(t.p3.x, t.p3.y)), new Point(new Vector2(p.x, p.y)));
                        triangles.Add(subTri2);
                    Triangle subTri3 = new Triangle(
                        new Point(new Vector2(t.p3.x, t.sp3.y)), new Point(new Vector2(t.p1.x, t.p1.y)), new Point(new Vector2(p.x, p.y)));
                        triangles.Add(subTri3);
                    
                    
                    triangles.Remove(t);
                }
                else
                { 
                    t.isDelaunay = true;
                    Debug.Log($"Triangle {triangles.IndexOf(t)} is Delaunay");

                }
            }
        }

        depth--;
        
        Triangulate(triangles, points, depth);
        
        return triangles;
    }

}
