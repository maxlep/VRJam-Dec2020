using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform roomContainer;
    [SerializeField] private float gridBoxSize = 2.5f;

    private Dictionary<(int, int), RoomReferences> roomDict= new Dictionary<(int, int), RoomReferences>();


    [Button(ButtonSizes.Medium)]
    public void InitializeRooms()
    {
        roomDict.Clear();
        InitRoomList();
        DisableInteriorWalls();
    }
    
    [Button(ButtonSizes.Medium)]
    private void ResetRooms()
    {
        roomDict.Clear();
        InitRoomList();
        EnableAllWalls();
    }
    
    
    private void InitRoomList()
    {
        //Populate Room list
        foreach (Transform roomTransform in roomContainer)
        {
            //TODO: Account for slight error in the positioning of room on grid (Ex. 1.05 instead of 1);
            int x = (int) Mathf.Floor(roomTransform.position.x / gridBoxSize);
            int y = (int) Mathf.Floor(roomTransform.position.z / gridBoxSize);
            RoomReferences roomReference = roomTransform.GetComponent<RoomReferences>();
            roomReference.GlobalAlignWalls();
            
            if (roomDict.ContainsKey((x, y))) 
                Debug.LogError($"Room already exists! This gameobject is" +
                               $" duplicate: {roomDict[(x, y)].gameObject}");
            
            roomDict.Add((x, y), roomReference);
        }
    }

    private void DisableInteriorWalls()
    {
        //Iterate through dict and disable interior walls
        foreach ((int x, int y) coordinate in roomDict.Keys)
        {
            (int, int) northNeightborCoord = (coordinate.x, coordinate.y + 1);
            (int, int) eastNeighborCoord = (coordinate.x + 1, coordinate.y);
            (int, int) southNeighborCoord = (coordinate.x, coordinate.y - 1);
            (int, int) westNeighborCoord = (coordinate.x - 1, coordinate.y);

            bool hasNorthNeighbor = roomDict.ContainsKey(northNeightborCoord);
            bool hasEastNeighbor = roomDict.ContainsKey(eastNeighborCoord);
            bool hasSouthNeighbor = roomDict.ContainsKey(southNeighborCoord);
            bool hasWestNeighbor = roomDict.ContainsKey(westNeighborCoord);

            if (hasNorthNeighbor) roomDict[coordinate].FrontWallGlobal.SetActive(false);
            if (hasEastNeighbor) roomDict[coordinate].RightWallGlobal.SetActive(false);
            if (hasSouthNeighbor) roomDict[coordinate].BackWallGlobal.SetActive(false);
            if (hasWestNeighbor) roomDict[coordinate].LeftWallGlobal.SetActive(false);
        }
    }

    private void EnableAllWalls()
    {
        //Iterate through dict and enable all walls
        foreach ((int x, int y) coordinate in roomDict.Keys)
        {
            roomDict[coordinate].FrontWallGlobal.SetActive(true);
            roomDict[coordinate].RightWallGlobal.SetActive(true);
            roomDict[coordinate].BackWallGlobal.SetActive(true);
            roomDict[coordinate].LeftWallGlobal.SetActive(true);
        }
    }
    
    
    
    
    
}
