using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public string text;
    public bool active = true;

    public void Deactivate() {
        active = false;
        GetComponent<Button>().interactable = false;
    }

    public void Activate() {
        active = true;
        GetComponent<Button>().interactable = true;
    }
}
