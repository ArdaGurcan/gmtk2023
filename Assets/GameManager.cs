using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool inEditMode = true;
    GameObject stanley;
    StanleyController stanleyController;
    

    void Start()
    {
        stanley = GameObject.FindGameObjectWithTag("Stanley");
        stanleyController = stanley.GetComponent<StanleyController>();
        stanley.SetActive(false);
    }

    public void SpawnStanley() {
        stanley.SetActive(true);
    }


}
