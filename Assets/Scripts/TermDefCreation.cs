using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Xml.Linq;
using System.Linq;

public class TermDefCreation : MonoBehaviour
{
    public GameObject titlePrefab;
    public GameObject descriptionPrefab;
    GameObject titleObject;
    GameObject descritionObject;
    [SerializeField] string title;
    [SerializeField] string description;

    public TMP_InputField titleTextMesh;
    public TMP_InputField descriptionTextMesh;
    TextMeshProUGUI termCounterText;
    public GameObject termDefInput;
    public Transform scrollViewContent;
    [SerializeField] private float termNumber = 0f;

    [SerializeField] List<GameObject> instPrefabs = new();
    [SerializeField] List<TMP_InputField> termsTextMesh = new();
    [SerializeField] List<TMP_InputField> definitionsTextMesh = new();

    Dictionary<string, string> termDefPairs = new();

    public GameObject sceneChanger;

    private void Start()
    {
        titleObject = Instantiate(titlePrefab, scrollViewContent);
        descritionObject = Instantiate(descriptionPrefab, scrollViewContent);

        termCounterText = termDefInput.GetComponent<TextMeshProUGUI>();
        for (int i = 0; i < 10; i++)
        {
            CreateTermDef();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Insert))
        {
            CreateTermDef();
        }
        else if (Input.GetKeyDown(KeyCode.Delete))
        {
            RemoveTermDef();
        }
        else if (Input.GetKeyDown(KeyCode.Home))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            sceneChanger.GetComponent<SceneChanger>().FadeToScene(0);
        }
    }

    public void CreateTermDef()
    {
        float num = termNumber;
        termCounterText.text = (num+1).ToString();
        GameObject prefab = Instantiate(termDefInput, scrollViewContent);
        instPrefabs.Add(prefab);
        termNumber++;
    }

    public void RemoveTermDef()
    {
        GameObject last = instPrefabs.Last();
        instPrefabs.Remove(last);
        Destroy(last);
        termNumber--;
    }

    public void Save()
    {
        termsTextMesh.Clear();
        definitionsTextMesh.Clear();
        termDefPairs.Clear();
        foreach (GameObject prefab in instPrefabs)
        {
            termsTextMesh.Add(prefab.transform.GetChild(0).gameObject.GetComponent<TMP_InputField>());
            definitionsTextMesh.Add(prefab.transform.GetChild(1).gameObject.GetComponent<TMP_InputField>());
        }
        for (int i = 0; i < termNumber; i++)
        {
            if (termsTextMesh[i].text != "")
            {
                termDefPairs.Add(termsTextMesh[i].text, definitionsTextMesh[i].text);
            }
            
        }

        titleTextMesh = titleObject.GetComponent<TMP_InputField>();
        descriptionTextMesh = descritionObject.GetComponent<TMP_InputField>();

        title = titleTextMesh.text;
        description = descriptionTextMesh.text;

        XElement titleElement = new("title", title);
        XElement descriptionElement = new("description", description);
        
        XElement xElement = new("root",
            titleElement,
            descriptionElement);

        foreach (string key in termDefPairs.Keys)
        {
            XElement element = new("term", new XAttribute("def", termDefPairs[key]), key);

            xElement.Add(element);
        }

        XDocument termDefFile = new(
            xElement
            );

        termDefFile.Save(Application.dataPath + @"\" + title + ".xml");
    }
}