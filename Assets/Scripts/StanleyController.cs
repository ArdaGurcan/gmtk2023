using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanleyController : MonoBehaviour
{
  public LayerMask rooms;

  [SerializeField]
  bool stuck = false;

  void Start()
  {
    stuck = false;
    // InvokeRepeating("Step", 0, 1);
  }

  public void Step(string instruction)
  {
    // yield return new WaitForEndOfFrame();
<<<<<<< HEAD
    if (!stuck)
    {
=======
    if (!stuck) {
>>>>>>> e443d45 (added distraction room and prefabs for special rooms)
      List<string> options = GetOptions();
      if (options.Count > 0)
      {

        List<string> worstOptions = new List<string>();
        foreach (var option in options)
        {
          if (!instruction.Contains(option.Split(' ')[1]))
          {
            worstOptions.Add(option.Split(' ')[1]);
          }
        }
        if (worstOptions.Count == 0)
        {
          Debug.Log("Stanley had only one choice, surely he wouldn't stay in the same room forever just to disobey the narrator, right?");
        }
        else
        {
          string move = worstOptions[Random.Range(0, worstOptions.Count)];
          Debug.Log(instruction.Split("|")[0]);
          Move(move);
        }
      }
<<<<<<< HEAD
    }
    else
    {
=======
    } else {
>>>>>>> e443d45 (added distraction room and prefabs for special rooms)
      Debug.Log("Stanley just needed 5 more minutes with the distraction and then he would surely be on his way.");
    }
  }

  void Move(string move)
  {
    if (move.Equals("left"))
    {
      // animation for turning left 
      transform.Rotate(new Vector3(0, -90));
    }
    else if (move.Equals("right"))
    {
      // animation for turning right
      transform.Rotate(new Vector3(0, 90));
    }

    // animation for walking straight
    transform.position += transform.forward * 2f;

    Collider[] collisions = Physics.OverlapSphere(transform.position, 0.1f, rooms);
    if (collisions.Length == 0)
    {
      Debug.Log("In an attempt to spite the narrator, Stanley fell into the deep, endless void");
      // animation for falling
    } else {
      Room currentRoom = collisions[0].GetComponent<Room>();
      if (currentRoom.room_type == Room.RoomType.distraction){
          stuck = true;
          Debug.Log("Stanley would finish the story right after he enjoyed this nice little distraction.");
      }
    }
    else
    {
      Room currentRoom = collisions[0].GetComponent<Room>();
      currentRoom.visited = true;
      // close door animation
      if (currentRoom.room_type == Room.RoomType.distraction)
      {
        stuck = true;
        Debug.Log("Stanley would finish the story right after he enjoyed this nice little distraction.");
      }
    }
  }

  public List<string> GetOptions()
  {
    Collider[] collisions = Physics.OverlapSphere(transform.position, 0.1f, rooms);
    (List<Vector3> dirs, List<Material> colors) = collisions[0].GetComponent<Room>().GetDoors();
    Room currentRoom = collisions[0].GetComponent<Room>();
    currentRoom.visited = true;

    List<string> outputs = new List<string>();

    for (int i = 0; i < dirs.Count; i++)
    {

      string doorString = colors[i].name + " ";

      if (Vector3.Magnitude(dirs[i] - transform.forward) < 0.1f)
      {
        doorString += "forward";
      }
      else if (Vector3.Magnitude(dirs[i] - transform.right) < 0.1f)
      {
        doorString += "right";
      }
      else if (Vector3.Magnitude(dirs[i] + transform.right) < 0.1f)
      {
        doorString += "left";
      }
      else if (Vector3.Magnitude(dirs[i] + transform.forward) > 0.1f)
      {
        Debug.LogError("door direction doesn't match character rotation");
      }
      outputs.Add(doorString);
    }

    return outputs;
  }
}
