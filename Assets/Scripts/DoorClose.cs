using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClose : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
       animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void closedoor() {
      animator.SetTrigger("Close");
    }
}
