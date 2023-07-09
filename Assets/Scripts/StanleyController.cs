using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanleyController : MonoBehaviour
{
  public LayerMask rooms;
  public Animator animator;
  public LayerMask doors;
  public GameManager mgr;
  public AudioClip[] distraction;
  private AudioSource voice_track01, voice_track02;
  public AudioClip[] voice_lines;
  public bool turning = false;
  public float time = 5f;
  public bool moving = false;
  private double startTime;
  private double duration = 0;

  [SerializeField]
  bool stuck = false;

  void Start()
  {
    stuck = false;
    mgr = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    voice_track01 = gameObject.AddComponent<AudioSource>();
    voice_track02 = gameObject.AddComponent<AudioSource>();
    startTime = AudioSettings.dspTime + 0.5f;
    // InvokeRepeating("Step", 0, 1);
  }

  public void Step(string instruction)
  {
    IDictionary<string,int> getIndex = new Dictionary<string, int>();
    getIndex.Add("stay", 0);// Assign Correctly
    getIndex.Add("forward", 1);
    getIndex.Add("left", 2);
    getIndex.Add("right", 3);
    if (!moving)
    {
      // yield return new WaitForEndOfFrame();
      if (!stuck)
      {
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
            voice_track01.clip = voice_lines[getIndex[instruction.Split("|")[1]]];
            voice_track01.PlayScheduled(startTime + duration);
            startTime = AudioSettings.dspTime + 0.5f;
            duration = voice_lines[getIndex[instruction.Split("|")[1]]].samples / voice_lines[getIndex[instruction.Split("|")[1]]].frequency + 1f;

            Debug.Log("Stanley had only one choice, surely he wouldn't stay in the same room forever just to disobey the narrator, right?");
          }
          else
          {
            string move = worstOptions[Random.Range(0, worstOptions.Count)];
            int choice = getIndex[instruction.Split("|")[1]];            

            duration = voice_lines[choice].samples / voice_lines[choice].frequency + 1f; // TODO: Change to move in direction
            voice_track01.clip = voice_lines[choice];
            voice_track01.PlayScheduled(startTime + duration);
            startTime = AudioSettings.dspTime + 0.5f;
            duration = voice_lines[choice].samples / voice_lines[choice].frequency + 1f; // TODO: Change to move in direction
            Debug.Log(instruction.Split("|")[0]);
            Move(move);
          }
        }
      }
      else
      {
        Debug.Log("Stanley just needed 5 more minutes with the distraction and then he would surely be on his way.");
      }
    }
  }

  void Move(string move)
  {
    int choice = -1;
    Vector3 end_pos = transform.position + transform.forward * 2f;
    if (move.Equals("left"))
    {
      end_pos = transform.position - transform.right * 2f;
      animator.SetTrigger("TurnLeft");
      StartCoroutine(TurnAndMove(true, 1, transform.position, end_pos));
      choice = 6;
      voice_track02.clip = voice_lines[choice];
      voice_track02.PlayScheduled(startTime + duration);
      startTime = AudioSettings.dspTime + 0.5f;
      duration = voice_lines[choice].samples / voice_lines[choice].frequency + 2f; // TODO: Change to disappointment left voice line
    }
    else if (move.Equals("right"))
    {
      end_pos = transform.position + transform.right * 2f;
      animator.SetTrigger("TurnRight");
      StartCoroutine(TurnAndMove(true, 1, transform.position, end_pos));
      choice = 7;
      voice_track02.clip = voice_lines[choice];
      voice_track02.PlayScheduled(startTime + duration);
      startTime = AudioSettings.dspTime + 0.5f;
      duration = voice_lines[choice].samples / voice_lines[choice].frequency + 1f; // TODO: Change to disappointment right voice line
    }
    else
    {
      // turnLeft is dummy value never used
      StartCoroutine(TurnAndMove(false, 1, transform.position, end_pos));
      choice = 5;
      voice_track02.clip = voice_lines[choice];
      voice_track02.PlayScheduled(startTime + duration);
      startTime = AudioSettings.dspTime + 0.5f;
      duration = voice_lines[choice].samples / voice_lines[choice].frequency + 1f; // TODO: Change to disappointment forward voice line
    }

  }


  public void Reset()
  {
    StopAllCoroutines();
    moving = false;
    turning = false;

    animator.SetBool("Moving", false);
    animator.SetBool("Falling", false);
    animator.SetBool("Stuck", false);
    Animator[] anims = GameObject.FindObjectsOfType<Animator>();
    foreach (Animator anim in anims)
    {
      anim.Rebind();
      anim.Update(0f);
    }

  }



  private IEnumerator TurnAndMove(bool toturn, int step, Vector3 begin_pos, Vector3 end_pos)
  {
    moving = true;
    if (toturn)
    {
      do
      {
        yield return new WaitForSeconds(1);
        yield return null;
      } while (turning);
    }

    // transform.Rotate(0, 90, 0);

    animator.SetBool("Moving", true);
    StartCoroutine(Falling());
    StartCoroutine(ThroughDoor());
    for (float t = 0; t < 1; t += Time.deltaTime / time)
    {
      transform.position = Vector3.Lerp(begin_pos, end_pos, t);
      yield return null;
    }
    animator.SetBool("Moving", false);

    Collider[] collisions = Physics.OverlapSphere(transform.position, 0.1f, rooms);
    // Debug.Log(collisions.Length);
    if (collisions.Length == 0)
    {
      Debug.Log("In an attempt to spite the narrator, Stanley fell into the deep, endless void");
    }
    else
    {
      Room currentRoom = collisions[0].GetComponent<Room>();
      currentRoom.visited = true;
      // Debug.Log(currentRoom.room_type);
      // close door animation
      if (currentRoom.room_type == Room.RoomType.distraction)
      {
        
        stuck = true;
        Debug.Log("Stanley would finish the story right after he enjoyed this nice little distraction.");
        animator.SetBool("Stuck", true);
      }
    }
    yield return new WaitForSeconds(0.75f);
    GameManager.Loop();
    moving = false;
  }

  private IEnumerator Falling()
  {
    bool change_music = false;
    moving = true;
    Collider[] collisions;
    do
    {
      collisions = Physics.OverlapSphere(transform.position, 0.01f, rooms);
      if (collisions.Length == 0)
      {
        animator.SetBool("Falling", true);
        animator.SetBool("Moving", false);
        int choice = 10; //TODO: INITIALIZE TO THE CORRECT INDEX for falling
        voice_track01.clip = voice_lines[choice];
        voice_track01.PlayScheduled(startTime + duration);
        startTime = AudioSettings.dspTime + 0.5f;
        duration = voice_lines[choice].samples / voice_lines[choice].frequency + 1f;
      } else {
        if(!change_music) {
          if(collisions.Length != 0 && !change_music) {
            if(collisions[0].GetComponent<Room>().room_type == Room.RoomType.distraction) {
              int index = Random.Range(0,2);
              SoundManager.instance.SwapTrack(distraction[index], distraction[index]);
              change_music = true;
              int choice = Random.Range(8,9); //TODO: INITIALIZE TO THE CORRECT INDEX for distraction
              voice_track01.clip = voice_lines[choice];
              voice_track01.PlayScheduled(startTime + duration);
              startTime = AudioSettings.dspTime + 0.5f;
              duration = voice_lines[choice].samples / voice_lines[choice].frequency + 1f;
            }
          }
        }
      }
      yield return null;
    } while (animator.GetBool("Moving"));
  }

  private IEnumerator ThroughDoor()
  {
    do
    {
      Collider[] collisions = Physics.OverlapSphere(transform.position + transform.up * 0.7f, 0.1f, doors);
      // Debug.Log("Checking for doors: " + collisions.Length);
      if (collisions.Length != 0)
      {
        DoorClose dc = collisions[0].GetComponentInChildren<DoorClose>();
        // Debug.Log("In Door Trigger");
        dc.CloseDoor();
      }
      yield return null;
    } while (animator.GetBool("Moving"));
  }

  public List<string> GetOptions()
  {
    Collider[] p_collisions = Physics.OverlapSphere(transform.position, 0.1f, rooms);
    List<string> outputs = new List<string>();
    if (p_collisions.Length > 0)
    {

      Room curr_room = p_collisions[0].GetComponent<Room>();

      if(curr_room.room_type == Room.RoomType.end) {
        mgr.levelComplete = true;
      }

      (List<Vector3> dirs, List<Material> colors) = curr_room.GetDoors();


      for (int i = 0; i < dirs.Count; i++)
      {

        string doorString = colors[i].name + " ";

        if (Vector3.Magnitude(dirs[i] - transform.forward) < 0.1f)
        {
          if (curr_room.GetRoomForward() == null ||
              curr_room.GetRoomForward().GetComponent<Room>().doorStates[1])
            doorString += "forward";
        }
        else if (Vector3.Magnitude(dirs[i] - transform.right) < 0.1f)
        {
          // Debug.Log(curr_room.GetRoomRight().transform.name);
          if (curr_room.GetRoomRight() == null ||
              curr_room.GetRoomRight().GetComponent<Room>().doorStates[2])
            doorString += "right";
        }
        else if (Vector3.Magnitude(dirs[i] + transform.right) < 0.1f)
        {
          // Debug.Log("Left of Current Room is: " + curr_room.GetRoomLeft().transform.name);
          if (curr_room.GetRoomLeft() == null ||
              curr_room.GetRoomLeft().GetComponent<Room>().doorStates[3])
          {
            doorString += "left";
          }
        }
        else if (Vector3.Magnitude(dirs[i] + transform.forward) > 0.1f)
        {
          Debug.LogError("door direction doesn't match character rotation");
        }
        outputs.Add(doorString);
      }
    }
    return outputs;
  }

  public void StartTurning()
  {
    turning = true;
  }

  public void StopTurning()
  {
    turning = false;
  }

  // void OnDrawGizmos() {
  //   Gizmos.DrawSphere(transform.position+transform.up*0.7f, 0.1f);
  // }
}
