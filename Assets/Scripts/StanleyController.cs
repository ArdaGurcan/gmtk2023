using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanleyController : MonoBehaviour
{
  public LayerMask rooms;
  public Animator animator;
  public bool turning;
  public float time = 5f;

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
  }

  void Move(string move) { 
    Vector3 end_pos = transform.position + transform.forward * 2f ;
    if (move.Equals("left"))
    {
      end_pos = transform.position - transform.right * 2f ;
      animator.SetTrigger("TurnLeft");
      StartCoroutine(TurnAndMove(true, 1,transform.position, end_pos));
    }
    else if (move.Equals("right"))
    {
      end_pos = transform.position + transform.right * 2f ;
      animator.SetTrigger("TurnRight");
      StartCoroutine(TurnAndMove(true, 1, transform.position, end_pos));
    } else {
      // turnLeft is dummy value never used
      StartCoroutine(TurnAndMove(false, 1, transform.position, end_pos));
    }

    Collider[] collisions = Physics.OverlapSphere(transform.position, 0.1f, rooms);
    if (collisions.Length == 0) {
      Debug.Log("In an attempt to spite the narrator, Stanley simply fell into the deep, endless void");
      // animation for falling
    }
  }

  private IEnumerator TurnAndMove(bool toturn, int step, Vector3 begin_pos, Vector3 end_pos) {
      if(toturn) {
          do {
              yield return new WaitForEndOfFrame();
              Debug.Log("Turning");
          } while(turning);
      }
      Debug.Log("Turning Ended Starting Move");
      animator.SetBool("Moving", true);
      for(float t = 0; t < 1; t += Time.deltaTime / time) {
      // for(float t = 0; t < 1; t += Time.deltaTime / 10){
          Debug.Log("Moving Stanley");
          transform.position = Vector3.Lerp(begin_pos, end_pos, t);
          yield return new WaitForEndOfFrame();
      }
      animator.SetBool("Moving", false);
  }

  List<string> GetOptions()
  {
    Collider[] collisions = Physics.OverlapSphere(transform.position, 0.1f, rooms);
    (List<Vector3> dirs, List<Material> colors) = collisions[0].GetComponent<Room>().GetDoors();
    collisions[0].GetComponent<Room>().visited = true;

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

  public void Turning() {
    turning = true;
  }

  public void StopTurning() {
    turning = false;
  }
}
