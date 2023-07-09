using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanleyController : MonoBehaviour
{
    public LayerMask rooms;
    public Animator animator;
    public bool turning = false;
    public float time = 5f;
    public Quaternion rot;

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
        else
        {
            Debug.Log("Stanley just needed 5 more minutes with the distraction and then he would surely be on his way.");
        }
    }

    void Move(string move)
    {
        Vector3 end_pos = transform.position + transform.forward * 2f;
        if (move.Equals("left"))
        {
            end_pos = transform.position - transform.right * 2f;
            animator.SetTrigger("TurnLeft");
            StartCoroutine(TurnAndMove(true, 1, transform.position, end_pos));
        }
        else if (move.Equals("right"))
        {
            end_pos = transform.position + transform.right * 2f;
            animator.SetTrigger("TurnRight");
            StartCoroutine(TurnAndMove(true, 1, transform.position, end_pos));
        }
        else
        {
            // turnLeft is dummy value never used
            StartCoroutine(TurnAndMove(false, 1, transform.position, end_pos));
        }

    }

    private IEnumerator TurnAndMove(bool toturn, int step, Vector3 begin_pos, Vector3 end_pos)
    {

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
        for (float t = 0; t < 1; t += Time.deltaTime / time)
        {
            transform.position = Vector3.Lerp(begin_pos, end_pos, t);
            yield return new WaitForEndOfFrame();
        }
        animator.SetBool("Moving", false);

        Collider[] collisions = Physics.OverlapSphere(transform.position, 0.1f, rooms);

        if (collisions.Length == 0)
        {
            Debug.Log("In an attempt to spite the narrator, Stanley fell into the deep, endless void");
            animator.SetBool("Falling", true);
            // animation for falling
        }
        else
        {
            Room currentRoom = collisions[0].GetComponent<Room>();
            currentRoom.visited = true;
            Debug.Log(currentRoom.room_type);
            // close door animation
            if (currentRoom.room_type == Room.RoomType.distraction)
            {
                stuck = true;
                Debug.Log("Stanley would finish the story right after he enjoyed this nice little distraction.");
                animator.SetBool("Stuck", true);
            }
        }
    }

    public List<string> GetOptions()
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, 0.1f, rooms);
        (List<Vector3> dirs, List<Material> colors) = collisions[0].GetComponent<Room>().GetDoors();

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

    public void StartTurning()
    {
        turning = true;
    }

    public void StopTurning()
    {
        turning = false;
    }
}
