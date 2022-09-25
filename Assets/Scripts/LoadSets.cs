using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class LoadSets : MonoBehaviour
{
    [SerializeField] (string, string) titleDesc;
    public static string title;
    string desc;
    XDocument[] sets;

    public Transform scrollViewContent;
    public GameObject loadSetButton;

    public GameObject sceneChanger;

    public static Dictionary<string, string> set = new Dictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
        string[] files = System.IO.Directory.GetFiles(Application.dataPath, "*.xml");

        foreach (string file in files)
        {
            titleDesc = LoadPart(file);

            title = titleDesc.Item1;
            desc = titleDesc.Item2;

            GameObject loadButton = Instantiate(loadSetButton, scrollViewContent);
            Button loadButtonButton = loadButton.GetComponent<Button>();
            loadButtonButton.onClick.AddListener(ButtonPressed);

            loadButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = title;
            loadButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = desc;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            sceneChanger.GetComponent<SceneChanger>().FadeToScene(0);
        }
    }

    private (string, string) LoadPart(string file)
    {
        XDocument xDocument = XDocument.Load(file);
        XElement[] elements = xDocument.Root.Elements().ToArray();
        string title = elements[0].Value;
        string description = elements[1].Value;

        return (title, description);
    }

    private void Load(string file)
    {
        XDocument xDocument = XDocument.Load(file);
        IEnumerable<XElement> elements = xDocument.Root.Elements();


        XElement rootElement = XElement.Parse("<root><key>value</key></root>");
        foreach (var el in rootElement.Elements())
        {
            set.Add(el.Name.LocalName, el.Value);
        }
    }

    private void ButtonPressed()
    {
        GameObject pressedButton = EventSystem.current.currentSelectedGameObject;

        title = pressedButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        desc = pressedButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;

        GameObject[] setLoadButtons = GameObject.FindGameObjectsWithTag("SetLoadButton");

        foreach (GameObject obj in setLoadButtons)
        {
            Destroy(obj);
        }

        sceneChanger.GetComponent<SceneChanger>().FadeToScene(3);
    }
}
