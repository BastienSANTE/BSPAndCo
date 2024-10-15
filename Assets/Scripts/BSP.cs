/*using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BSP : MonoBehaviour
{
    //Original room settings
    public Vector2Int baseRoomOrigin;       //Coordinates of the base room
    public int baseRoomW;                   //User-provided size
    public int baseRoomH;                   //User-provided size
    public int seed;                        //User-provided random seed
    public int depth;                       //User-provided recursion depth for BSPSlice()
    public int minWidth, minHeight;         //
    public int sliceMargin;
    
    private List<Room> rooms;                    //List of all rooms in BSP tree
    private int _iterations;                      //Iteration counter to avoid inf. recursion
    
    // Start is called before the first frame update
    [ContextMenu("Start")]void Start()
    {
        rooms = new List<Room>();
        
        UnityEngine.Random.InitState(seed);
        //Create a new Random based on the user-provided seed
        
        //Generate base room and add it to the list 
        Room baseRoom = new Room(baseRoomOrigin, baseRoomW, baseRoomH);
        // DebugDrawRoom(baseRoom);
        rooms.Add(baseRoom);
        // Debug.Log($"Base Room : {baseRoom.origin.x}, {baseRoom.origin.y}\n {baseRoom.width}, {baseRoom.height}");
        
        //Start of the recursive function
        // Debug.Log("Starting BSP Slice");
        BSPSlice(rooms, depth, minWidth, minHeight, sliceMargin);
        
    }
    private void OnDrawGizmos()
    {
        DebugDrawRooms(rooms);
    }

    // private void OnGUI()
    // {
    //     List<Room> rl = rooms;
    //     foreach (Room r in rl)
    //        GUI.Label(new Rect(r.center, new Vector2(10, 10)), $"{rl.IndexOf(r)}", new GUIStyle{fontSize = 20, fontStyle = FontStyle.Bold, normal});
    // }

    private void DebugDrawRooms(List<Room> rl)
    {
        if (rl == null || rl.Count == 0) return;
        
        foreach (Room r in rl)
        {
           Debug.DrawLine(new Vector3(r.origin.x, r.origin.y, 0), new Vector3(r.origin.x + r.width, r.origin.y, 0), Color.red); 
           Debug.DrawLine(new Vector3(r.origin.x, r.origin.y, 0), new Vector3(r.origin.x, r.origin.y + r.height, 0), Color.red);
           Debug.DrawLine(new Vector3(r.origin.x, r.origin.y + r.height, 0), new Vector3(r.origin.x + r.width, r.origin.y + r.height, 0), Color.red);
           Debug.DrawLine(new Vector3(r.origin.x + r.width, r.origin.y, 0), new Vector3(r.origin.x + r.width, r.origin.y + r.height, 0), Color.red);
           Debug.DrawLine(new Vector3(r.origin.x, r.origin.y, 0), new Vector3(r.origin.x + r.width, r.origin.y + r.height, 0), Color.cyan);
        }
    }
    
    private void BSPSlice(List<Room> roomList, int it, int minW, int minH, int sm)
    {
        
        int roomID;                             //ID of the selected room in the list
        Vector2Int sliceCoord;                  //Point at which the slice occurs
        bool vertical;                          //Selects either horizontal or vertical
        Room selRoom;
        
        if (it == 0)
        {
            Debug.Log("Recursion limit reached, returning");
            return;
        }
        
        // Debug.Log($"Depth : {it}");
        
        //Select the largest room from the room list
        roomID = SelectLargestRoom(roomList);
        selRoom = roomList[roomID];
        if (selRoom.width < minW || selRoom.height < minH)
            return;
        
        //Select a random coordinate within the room's bounds to slice
        sliceCoord = new Vector2Int(
            Mathf.FloorToInt(Random.Range(selRoom.origin.x + sm, selRoom.origin.x + selRoom.width - sm)),
            Mathf.FloorToInt(Random.Range(selRoom.origin.y + sm, selRoom.origin.y + selRoom.height - sm)));
        
        //Select if slice will be vertical or horizontal
        //Changed to alternate equally between the two for debugging
        vertical = Random.value > 0.5f;

        //Create both rooms resulting from the slice
        if (vertical || selRoom.height < selRoom.width)
        {
            Room lRoom = new Room(new Vector2Int(selRoom.origin.x, selRoom.origin.y),
                                 (selRoom.origin.x + sliceCoord.x - selRoom.origin.x), selRoom.height);
                                    roomList.Add(lRoom);
            
            Room rRoom = new Room(new Vector2Int(sliceCoord.x, selRoom.origin.y),
                                 (selRoom.origin. x + selRoom.width - sliceCoord.x), selRoom.height);
                                    roomList.Add(rRoom);
            
            Debug.Log($"Sliced at X {sliceCoord.x} in Room {roomID}, Vertical\n" +
                      $"Resulting Rooms are L : {lRoom.origin.x}, {lRoom.origin.y}, w={lRoom.width}, h={lRoom.height}   " + 
                      $"R : {rRoom.origin.x}, {rRoom.origin.y}, w={rRoom.width}, h={rRoom.height}\n" + 
                      $"Room {roomID} will now be removed");
        }
        else if (!vertical)
        {
            Room bRoom = new Room(new Vector2Int(selRoom.origin.x, selRoom.origin.y), 
                                   (selRoom.origin.y - sliceCoord.y), selRoom.width);
                                    roomList.Add(bRoom);
            
            Room tRoom = new Room(new Vector2Int(selRoom.origin.x, sliceCoord.y),
                                  (selRoom.origin.x), selRoom. origin. y + selRoom.height - sliceCoord.y);
                                    roomList.Add(tRoom);
            
            Debug.Log($"Sliced at Y {sliceCoord.y} in Room {roomID}, Horizontal\n" +
                      $"Resulting Rooms are B : {bRoom.origin.x}, {bRoom.origin.y}, w={bRoom.width}, h={bRoom.height}   " + 
                      $"T : {tRoom.origin.x}, {tRoom.origin.y}, w={tRoom.width}, h={tRoom.height}\n" + 
                      $"Room {roomID} will now be removed");
        }
        
        roomList.Remove(selRoom);
        
        DebugDrawRooms(roomList);
        
        BSPSlice(roomList, it - 1, minW, minH, sm);
        
    }

    private int SelectLargestRoom(List<Room> roomList)
    {
        int largestArea = 0;
        int largestRoomID = 0;
        
        foreach (Room r in roomList)
        {
            if (r.area > largestArea)
            {
                largestRoomID = roomList.IndexOf(r);
                largestArea = r.area;
            }
        }
        
        Debug.Log($"Largest room is {largestRoomID}, Area: {largestArea}");
        return largestRoomID;
    }
}*/