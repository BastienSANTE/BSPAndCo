using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public partial class BSP2
{
    public Random Generator { get; private set; }
    
    public void Init()
    {
        Generator.InitState()
    }

    public List<Room> BSP(int width, int height, int depth)
    {
        List<Room> rooms = new List<Room> { new Room(Vector2Int.zero, width, height) };

        if (depth <= 0)
            return rooms;

        Room roomToProcess = SelectLargestRoom(rooms);
        Slice(rooms, roomToProcess);

        // BSP();
        return rooms;
    }
    
    public List<Room> BSP(List<Room> rooms, int depth)
    {
        if (depth <= 0)
            return rooms;

        Room roomToProcess = SelectLargestRoom(rooms);
        Slice(rooms, roomToProcess);

        // BSP();
        return rooms;
    }

    private Room SelectLargestRoom(List<Room> roomList)
    {
        Room largestRoom = roomList.OrderByDescending(r => r.area).FirstOrDefault();

        if (largestRoom == null)
            throw new Exception("Error: unable to find a room");
        
        Debug.Log($"Largest room is {largestRoom.ID}, Area: {largestRoom.area}");
        return largestRoom;
    }
    
    private void Slice(List<Room> canvas, Room roomToSlice)
    {
        Room roomA, roomB;
        Vector2Int sliceCoords;
        SliceDirection sliceDir;
        
        
    }
}