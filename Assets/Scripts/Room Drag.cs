using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDrag : MonoBehaviour
{
    public GameObject prefab;

    public Vector3 mousePos;
    public bool stationary = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnMouseDown()
    {
        mousePos = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
    }
     void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePos);
        if(gameObject.transform.position.x <= 4) {
            Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MousePos.y = 0;
            Instantiate(prefab, MousePos, gameObject.transform.rotation);
            Destroy(gameObject);
            Debug.Log("outside");
        }
        Debug.Log(gameObject.transform.position);
      
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}
