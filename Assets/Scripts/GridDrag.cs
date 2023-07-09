using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDrag : MonoBehaviour
{
  public Vector3 mousePos;
  public bool stationary = false;
public static bool paused = false;
  void OnMouseDown()
  {

    mousePos = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
  }

  void OnMouseDrag()
  {
    if (!stationary && !paused)
        transform.position = ToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePos), 2f);
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
