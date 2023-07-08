using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    bool inEditMode = true;
    GameObject stanley;
    StanleyController stanleyController;
    public List<string> voicelines;
    public GameObject buttonPrefab;
    [SerializeField]
    Dictionary<string,List<GameObject>> buttonsDict;

    void Start()
    {
        buttonsDict = new Dictionary<string, List<GameObject>>();
        stanley = GameObject.FindGameObjectWithTag("Stanley");
        stanleyController = stanley.GetComponent<StanleyController>();
        stanley.SetActive(false);
        Transform canvas = GameObject.Find("Canvas").transform;
        float ypos = 0;
        foreach (string voiceline in voicelines)
        {
            // instantiate button for each voiceline under canvas
            Transform button = Instantiate(buttonPrefab, canvas.transform.position, canvas.rotation, canvas).transform;

            button.GetComponent<RectTransform>().anchoredPosition = new Vector3(-50, ypos, 0);

            Button uiButton = button.GetComponent<Button>();
            uiButton.onClick.AddListener(delegate{stanleyController.Step(voiceline);});
            
            button.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = voiceline.Split('|')[0];
            List<GameObject> buttons = null;
            if (!buttonsDict.TryGetValue(voiceline.Split('|')[1], out buttons)) {
                buttons = new List<GameObject>();
            }
            buttons.Add(button.gameObject);

            buttonsDict[voiceline.Split('|')[1]] = buttons;
            ypos -= 40;

        }
    }

    public void SpawnStanley() {
        stanley.SetActive(true);
        List<string> options = stanleyController.GetOptions();
        
    }


}
