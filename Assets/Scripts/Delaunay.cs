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
    
    private void DelaunayExec(List<Room> rooms)
    {
        _display = this.GetComponent<Display>();
        _pointList = GetPoints(_display.roomList);
        _superTriangle = CreateSuperTriangle(_display);

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
            new Point(new Vector2(disp.originX + 2 * disp.width, disp.originY + disp.height / 2)));

        return triangle;
    }
    
}
