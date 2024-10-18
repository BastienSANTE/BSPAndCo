using System.Collections.Generic;
using UnityEngine;

public class Display: MonoBehaviour
{
    public int originX, originY;
    public int width;
    public int height;
    
    [SerializeField] private int depth;
    [SerializeField] private int seed;
        
    public BSP2 bsp;
    public Delaunay delaunay;
    public List<Room> roomList; 

    [ContextMenu("Start BSP")] private void Start()
    { 
        bsp = new BSP2();
        DisplayRooms(); // Start the generation;
    }
    
    private void DisplayRooms()
    {
        roomList = new List<Room> { new Room(0, 0, width, height) };
        bsp.Init(seed);
        bsp.BSP(roomList, depth);
        bsp.DrawRooms(roomList);
        delaunay.DelaunayExec(roomList);
    }
}