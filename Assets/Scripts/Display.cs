using System;
using System.Collections.Generic;
using UnityEngine;

public class Display: MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int depth;
    [SerializeField] private int seed;
        
    private BSP2 _bsp;
    private List<Room> roomList; 

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
    }
}