using System;
using UnityEngine;

public class Display: MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
        
    private BSP2 _bsp;

    private void Start()
    { 
        _bsp = new BSP2();
        DisplayRooms(); // Start the generation;
    }

    [ContextMenu("Display Rooms")]
    public void DisplayRooms()
    {
        _bsp.BSP();
    }
}