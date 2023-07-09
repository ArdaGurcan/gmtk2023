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

  }
}
