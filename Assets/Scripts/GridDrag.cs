using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDrag : MonoBehaviour
{
  public Vector3 mousePos;
  public bool stationary = false;
public static bool paused = false;
  public GameObject[] rooms;
  public bool overlap = false;
  // index 0 is transform.x index 1 is transform.z
  private Vector3[] roomList = new Vector3[20];

  void OnMouseDown()
  {

    mousePos = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
  }

  void OnMouseDrag()
  {
    rooms = GameObject.FindGameObjectsWithTag("Room");
    for(int i = 0; i < rooms.Length; i++) {
      
      Vector3 roomPos = rooms[i].transform.position;
      if(gameObject.transform.position != roomPos) {
        roomList[i] = roomPos;
      }
      else {
        roomList[i] = new Vector3(1000f, 1000f, 1000f);
      }
    }
   
    Vector3 toGridPosition = ToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePos), 2f);
    for(int i = 0; i < rooms.Length; i++) {
      if(roomList[i].x == toGridPosition.x && roomList[i].z == toGridPosition.z) {
       
        overlap = true;    
      }
    }
     if (!stationary && !overlap && !paused)
        transform.position = toGridPosition;
    overlap = false;
  }

  Vector3 ToGrid(Vector3 pos, float size)
  {
    
    return new Vector3(
            Mathf.Round(pos.x / size) * size,
            pos.y,
            Mathf.Round(pos.z / size) * size
        );
  }
}
