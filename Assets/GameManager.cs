using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool inEditMode = true;
    GameObject stanley;
    StanleyController stanleyController;
    public List<string> voicelines;
    public GameObject buttonPrefab;


    void Start()
    {
        stanley = GameObject.FindGameObjectWithTag("Stanley");
        stanleyController = stanley.GetComponent<StanleyController>();
        stanley.SetActive(false);

        foreach (string voiceline in voicelines)
        {
            // instantiate button for each voiceline under canvas
        }
    }

    public void SpawnStanley() {
        stanley.SetActive(true);
        List<string> options = stanleyController.GetOptions();

    }


}
