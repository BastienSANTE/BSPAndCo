using UnityEngine;

public class Point
{
    public float x, y;
    public Vector2 position;

    public Point(Vector2 coords)
    {
        this.x = coords.x;
        this.y = coords.y;
        this.position = coords;
    }
}