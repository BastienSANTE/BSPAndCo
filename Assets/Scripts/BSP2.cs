using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BSP2
{
    public void Init(int seed)
    {
        Random.InitState(seed);
    }

    // public List<Room> BSP(int width, int height, int depth)
    // {
    //     List<Room> rooms = new List<Room> { new Room(0, 0, width, height) };
    //
    //     if (depth <= 0)
    //         return rooms;
    //
    //     Room roomToProcess = SelectLargestRoom(rooms);
    //     Slice(rooms, roomToProcess);
    //
    //     BSP();
    //     return rooms;
    // }
    
    public List<Room> BSP(List<Room> rooms, int depth)
    {
        if (depth <= 0)
            return rooms;

        Room roomToProcess = SelectLargestRoom(rooms);
        Slice(rooms, roomToProcess);
        
        Debug.Log($"There are {rooms.Count} rooms");
        depth--;
        BSP(rooms, depth);
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
    
    private List<Room> Slice(List<Room> canvas, Room slicedRoom)
    {
        Room roomA, roomB;
        SliceDirection sliceDir = (Random.Range(0f, 1f) > .5f) ? SliceDirection.Horizontal : SliceDirection.Vertical;

        Vector2Int sliceCoords = new(
            Random.Range(slicedRoom.orx, slicedRoom.orx + slicedRoom.width),
            Random.Range(slicedRoom.ory, slicedRoom.ory + slicedRoom.height));

        if (sliceDir == SliceDirection.Horizontal)
        {
            roomA = new Room(slicedRoom.orx, slicedRoom.ory, slicedRoom.width, (sliceCoords.y - slicedRoom.ory));       //Bottom Room
            roomB = new Room(slicedRoom.orx, sliceCoords.y, slicedRoom.width, (slicedRoom.height - roomA.height)); 
            canvas.Add(roomA);
            canvas.Add(roomB);
            Debug.Log("Removing sliced room");
            canvas.Remove(slicedRoom);
        } else
        {
            roomA = new Room(slicedRoom.orx, slicedRoom.ory, (sliceCoords.x - slicedRoom.orx), slicedRoom.height);
            roomB = new Room(sliceCoords.x, slicedRoom.ory, (slicedRoom.width - roomA.width), slicedRoom.height);
            canvas.Add(roomA);
            canvas.Add(roomB);
            Debug.Log("Removing sliced room");
            canvas.Remove(slicedRoom);
        }
        
        Debug.Log($"Sliced at {sliceCoords}, {sliceDir}\n" +
                  $"Room A : {roomA.orx}, {roomA.ory}, {roomA.width}, {roomA.height}\n" +
                  $"Room B : {roomB.orx}, {roomB.ory}, {roomB.width}, {roomB.height}");
        return canvas;
    }

    public void DrawRooms(List<Room> rooms)
    {
        foreach (Room r in rooms)
        {
            DrawBox(r.orx, r.ory, r.endx, r.endy, Color.green, 5);
        }
    }

    private void DrawBox(int x, int y, int ex, int ey, Color c, float duration)
    {
        Debug.DrawLine(
            new Vector3(x, y, 0),
            new Vector3(ex, y, 0), c, duration);
        Debug.DrawLine(
            new Vector3(x, y, 0),
            new Vector3(x, ey, 0), c, duration);
        Debug.DrawLine(
            new Vector3(x, ey, 0),
            new Vector3(ex, ey, 0), c, duration);
        Debug.DrawLine(
            new Vector3(ex, y, 0),
            new Vector3(ex, ey, 0), c, duration);
    }
}