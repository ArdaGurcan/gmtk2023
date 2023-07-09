using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
  public enum Direction
  {
    up,
    down,
    left,
    right
  }

  public enum RoomType
  {
    normal,
    start,
    end,
    distraction
  }

  public GameObject[] doors = new GameObject[4];
  public GameObject[] walls = new GameObject[4];
  public GameObject[] footsteps = new GameObject[4];
  public Material[] colors = new Material[4];
  public LayerMask rooms;

  public bool up;
  public bool down;
  public bool left;
  public bool right;

  bool[] doorStates;

  public RoomType room_type = RoomType.normal;

  public bool visited = false;

  void Start()
  {

    doorStates = new bool[] { up, down, left, right };

    for (int i = 0; i < 4; i++)
    {
      walls[i].SetActive(!doorStates[i]);
      doors[i].SetActive(doorStates[i]);
      footsteps[i].GetComponent<Renderer>().material = colors[i];
    }

  }

  public (List<Vector3>, List<Material>) GetDoors()
  {
    Vector3[] cardinalDirs = new Vector3[] { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

    List<Vector3> doorDirections = new List<Vector3>();
    List<Material> doorColors = new List<Material>();


    for (int i = 0; i < 4; i++)
    {
      if (doorStates[i])
      {
        Collider[] futureCollisions = Physics.OverlapSphere(transform.position + cardinalDirs[i] * 2f, 0.1f, 1 << 7);
        if (futureCollisions.Length > 0 && futureCollisions[0].GetComponent<Room>().visited)
        {
          continue;
        }
        doorDirections.Add(cardinalDirs[i]);
        doorColors.Add(colors[i]);
      }
    }

    return (doorDirections, doorColors);
  }



  void Update()
  {
    // Collider[] colliding = Physics.OverlapBox(transform.position, transform.forward*2, Quaternion.identity, rooms);
    Collider[] colliding = Physics.OverlapSphere(transform.position, 1.3f, rooms);
    foreach(Collider col in colliding) {
      Vector3 pos = col.gameObject.transform.position;
        if(Vector3.SqrMagnitude(pos - (transform.position - new Vector3(0, 0, 2))) < 0.1f)// room below
          doors[1].SetActive(false);
        else
          doors[1].SetActive(doorStates[1]);

        // if(pos == transform.position - new Vector3(2, 0 ,0))// room to the left
        if(Vector3.SqrMagnitude(pos - (transform.position - new Vector3(2, 0, 0))) < 0.1f)// room below
          doors[2].SetActive(false);
        else
          doors[2].SetActive(doorStates[2]);

        // if(pos == transform.position + new Vector3(2, 0 ,0))// room to the left
        if(Vector3.SqrMagnitude(pos - (transform.position + new Vector3(2, 0, 0))) < 0.1f)// room below
          doors[3].SetActive(false);
        else
          doors[3].SetActive(doorStates[3]);
    }
  }
}
