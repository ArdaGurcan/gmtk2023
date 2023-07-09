using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
  public string text;
  public bool active = true;

  public void Deactivate()
  {
    active = false;
    transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = new Color(0.5f, 0.5f, 0.5f);

    GetComponent<Button>().interactable = false;
    
  }

  public void Hide()
  {
    transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = new Color(0.8f, 0.8f, 0.8f);
    GetComponent<Button>().interactable = false;

  }

  public void Show()
  { 
    transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;
    if (active) {
        GetComponent<Button>().interactable = true;

    }

  }
  public void Activate()
  {
    active = true;
    GetComponent<Button>().interactable = true;
  }
}
