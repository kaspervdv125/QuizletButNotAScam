using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Xml.Linq;
using TMPro;

public class LearnWrite : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI outputField;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button nextButton;

    int number;
    [SerializeField] bool outputIsTerm;

    public GameObject sceneChanger;
    string title = LoadSets.title;
    public Dictionary<string, string> learnSet;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        Load(Application.dataPath + @"\" + title + ".xml");

        outputField.color = new Color32(204, 204, 204, 100);

        NextQuestion();

        nextButton.onClick.AddListener(CheckAnswer);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckAnswer();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            sceneChanger.GetComponent<SceneChanger>().FadeToScene(0);
        }
        else if (Input.GetKeyDown(KeyCode.Home))
        {
            Switch();
        }
    }

    public void CheckAnswer()
    {
        EventSystem.current.SetSelectedGameObject(inputField.gameObject);

        if (outputIsTerm)
        {
            Debug.Log(inputField.text);
            if (learnSet.ElementAt(number).Value == inputField.text)
            {
                CorrectAnswer();
            }
            else
            {
                IncorrectAnswer();
            }
        }
        else
        {
            if (learnSet.ElementAt(number).Key == inputField.text)
            {
                CorrectAnswer();
            }
            else
            {
                IncorrectAnswer();
            }
        }

        inputField.text = "";
    }

    public void CorrectAnswer()
    {
        Debug.Log("Correct");
        outputField.color = new Color32(131, 201, 149, 255);
        inputField.text = "Correct";
        learnSet.Remove(learnSet.ElementAt(number).Key);

        Debug.Log(learnSet.Count());

        Invoke("NextQuestion", 2.0f);
    }

    public void IncorrectAnswer()
    {
        Debug.Log("InCorrect");
        outputField.color = new Color32(252, 60, 53, 255);

        if (outputIsTerm)
        {

            outputField.text = "Correct answer: " + learnSet.ElementAt(number).Value;

        }
        else
        {
            outputField.text = "Correct answer: " + learnSet.ElementAt(number).Key;
        }
        Invoke("NextQuestion", 2.0f);
    }

    public void NextQuestion()
    {
        if (learnSet.Count() > 0)
        {
            number = Random.Range(0, (learnSet.Count) - 1);

            outputField.color = new Color32(204, 204, 204, 255);

            if (outputIsTerm)
            {
                outputField.text = learnSet.ElementAt(number).Key;
                inputField.text = "";
            }
            else
            {
                outputField.text = learnSet.ElementAt(number).Value;
                inputField.text = "";
            }
        }
        else
        {
            outputField.color = new Color32(255, 215, 0, 255);
            outputField.text = "Set complete!";
        }
    }

    private void Load(string file)
    {
        learnSet = new Dictionary<string, string>();
        
        XDocument xDocument = XDocument.Load(file);
        IEnumerable<XElement> elements = xDocument.Root.Elements();

        Debug.Log(elements.Count());

        for (int i = 2; i < elements.Count(); i++)
        {

            string term = elements.ElementAt(i).Value;
            string def = elements.ElementAt(i).Attribute("def").Value;

            Debug.Log($"{term}, {def}");

            learnSet.Add(term, def);
        }
    }

    public void Switch()
    {
        outputIsTerm = !outputIsTerm;
        NextQuestion();
    }
}
