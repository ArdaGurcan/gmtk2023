using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDrag : MonoBehaviour
{
    public GameObject prefab;

    public Vector3 mousePos;
    public bool stationary = false;
    public static bool paused = false;
    Vector3 initial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnMouseDown()
    {
        initial = transform.position;
        mousePos = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
    }
     void OnMouseDrag()
    {   
        if(!paused && GameManager.Check()) {

        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePos);
        if(gameObject.transform.position.x <= 6) {
            Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition-mousePos);
            MousePos.y = 0;
            Instantiate(prefab, ToGrid(MousePos, 2f), gameObject.transform.rotation);
            Destroy(gameObject);
        
        }
        }
      
      
    }

    void OnMouseUp()
    {
        if(gameObject.transform.position.x > 6) {
            transform.position = initial;
        
        }
        
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
