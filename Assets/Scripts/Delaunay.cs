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
        _triList = new List<Triangle> { _superTriangle };
        _depth = _pointList.Count;                          //Set recursion depth to number of points in list
        
        //Create 2 lists of points, respectively for used and unused points
        List<Point> usedPoints   =   new List<Point>();
        List<Point> unusedPoints = _pointList.ToList();
        
       //_triList = Triangulate(usedPoints, unusedPoints, _triList, _superTriangle);

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
            new Point(new Vector2(disp.originX, disp.originY + 2 * disp.height)),
            new Point(new Vector2(disp.originX + 2 * disp.width, disp.originY + disp.height / 2)), true);
        
        DrawTriangle(triangle, Color.cyan, 5f);
        
        return triangle;
    }

    private void DrawTriangle(Triangle t, Color c, float duration)
    {
        Debug.DrawLine(new Vector3(t.p1.x, t.p1.y, 0), new Vector3(t.p2.x, t.p2.y, 0), c, duration);
        Debug.DrawLine(new Vector3(t.p2.x, t.p2.y, 0), new Vector3(t.p3.x, t.p3.y, 0), c, duration);
        Debug.DrawLine(new Vector3(t.p3.x, t.p3.y, 0), new Vector3(t.p1.x, t.p1.y, 0), c, duration);
    }

    /*private List<Triangle> Triangulate(List<Point> used, List<Point> unused, List<Triangle> tl, Triangle st)
    {
        used.Add(unused[0]); //Add first point of list to used points
        unused.RemoveAt(0);  //Remove from unused points
        
        
        
        
        Triangulate(used, unused, tl, st);
    }*/

}
