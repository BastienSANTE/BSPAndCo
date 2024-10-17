using System.Collections.Generic;
using UnityEngine;

public class Display: MonoBehaviour
{
    public int originX, originY;
    public int width;
    public int height;
    
    [SerializeField] private int depth;
    [SerializeField] private int seed;
        
    public BSP2 _bsp;
    public Delaunay delaunay;
    public List<Room> roomList; 

    [ContextMenu("Start BSP")] private void Start()
    { 
        _bsp = new BSP2();
        DisplayRooms(); // Start the generation;
    }
    
    private void DisplayRooms()
    {
        roomList = new List<Room> { new Room(0, 0, width, height) };
        _bsp.Init(seed);
        _bsp.BSP(roomList, depth);
        _bsp.DrawRooms(roomList);
        delaunay.DelaunayExec(roomList);
    }
}