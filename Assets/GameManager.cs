using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  bool inEditMode = true;
  public bool levelComplete = false;
  GameObject stanley;
  StanleyController stanleyController;
  public static Vector3 initialPosition;
  // public List<string> voicelines;
  // public GameObject buttonPrefab;
  // [SerializeField]
  // Dictionary<string,List<GameObject>> buttonsDict;

  void Start()
  {
    // buttonsDict = new Dictionary<string, List<GameObject>>();
    stanley = GameObject.FindGameObjectWithTag("Stanley");
    stanleyController = stanley.GetComponent<StanleyController>();
    // stanley.SetActive(false);
    initialPosition = stanley.transform.position;
    GridDrag.paused = false;
    RoomDrag.paused = false;
    // Transform parent = GameObject.Find("Voice Panel").transform;

    // float ypos = 0;
    // foreach (string voiceline in voicelines)
    // {
    //     // instantiate button for each voiceline under canvas
    //     Transform button = Instantiate(buttonPrefab, parent.transform.position, parent.rotation, parent).transform;

    //     button.GetComponent<RectTransform>().anchoredPosition = new Vector3(-50, ypos, 0);

    //     Button uiButton = button.GetComponent<Button>();
    //     uiButton.onClick.AddListener(delegate{stanleyController.Step(voiceline);});

    //     button.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = voiceline.Split('|')[0];
    //     List<GameObject> buttons = null;
    //     if (!buttonsDict.TryGetValue(voiceline.Split('|')[1], out buttons)) {
    //         buttons = new List<GameObject>();
    //     }
    //     buttons.Add(button.gameObject);

    //     buttonsDict[voiceline.Split('|')[1]] = buttons;
    //     ypos -= 40;

    // }
    Loop();
  }

  public static bool Check()
  {
    GameObject character = GameObject.FindGameObjectWithTag("Stanley");
    bool c = Vector3.SqrMagnitude(character.transform.position - initialPosition) < 0.5f;
    Debug.Log(c);
    return c;
  }

  public void RepawnStanley()
  {
    // StopAllCoroutines();
    stanley.transform.position = initialPosition;
    stanleyController.Reset();
    stanley.SetActive(true);
    inEditMode = false;
    ButtonScript[] buttons = GameObject.FindObjectsOfType<ButtonScript>();
    foreach (ButtonScript button in buttons)
    {
      button.Activate();
    }
    GridDrag.paused = false;
    RoomDrag.paused = false;
    ButtonScript[] btns = GameObject.FindObjectsOfType<ButtonScript>();
    foreach (ButtonScript btn in btns)
    {
      btn.Activate();
    }
    Loop();
    StartCoroutine(WaitAndLoop());
  }
  IEnumerator WaitAndLoop()
  {

    yield return new WaitForSeconds(0.25f);
    Loop();
  }
  void Update()
  {
    if (levelComplete)
    {
      StartCoroutine(ChangeLevel());
    }

    if (Input.GetKeyDown(KeyCode.Space))
    {
      Loop();
    }
    
    if (Input.GetKeyDown(KeyCode.R)) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
  }

  IEnumerator ChangeLevel()
  {
    yield return new WaitForSeconds(5);
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

  public static void Loop()
  {
    ButtonScript[] buttons = GameObject.FindObjectsOfType<ButtonScript>();
    GameObject character = GameObject.FindGameObjectWithTag("Stanley");
    StanleyController controller = character.GetComponent<StanleyController>();
    List<string> options = controller.GetOptions();
    string allOptions = "";

    foreach (string option in options)
    {
      allOptions += option + "\n";
    }
    Debug.Log(allOptions);
    foreach (ButtonScript btn in buttons)
    {
      if (btn.active)
      {
        if (allOptions.Contains(btn.text.Split('|')[1]) || btn.text.Split('|')[1] == "stay")
        {
          btn.Show();

        }
        else
        {
          btn.Hide();
        }
      }
    }


  }


}
