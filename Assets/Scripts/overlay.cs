using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class overlay : MonoBehaviour
{

    public GameObject player;
    public string[][] templates;
    public GameObject dropdownParent;

    private int dropdownCount = 0;
    private Vector3 dropdownLocalPos = new Vector3(0f, 0f, 0f);

    public GameObject mapParent;
    private GameObject map_go;
    private GameObject map_go_new;

    public bool started = false;
    private bool added = false;
    private bool deleted = false;

    public string mapPrefabName;
    public string mapBackgroundName;

    public bool dead = false;

    public GameObject firstHeart;

    public int choice;

    void PlayMusic(int MusicNumber)
    {
        if (MusicNumber == 1)
        {
            AudioSource audioSource = GameObject.Find("BGM1").GetComponent<AudioSource>();
            audioSource.Play();
        }
        else if (MusicNumber == 2)
        {
            AudioSource audioSource = GameObject.Find("BGM2").GetComponent<AudioSource>();
            audioSource.Play();
        }
        else if (MusicNumber == 3)
        {
            AudioSource audioSource = GameObject.Find("BGM3").GetComponent<AudioSource>();
            audioSource.Play();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        ReadInTemplates("rehabitation_template");
        AddTemplateSlot();
        //var firstDD = GameObject.Find("Dropdown(Clone)");
        //PopulateDropdown(firstDD);
        SetUpMap();

        var player = GameObject.Find("Wizard_0").GetComponent<ModuleControl>();
        firstHeart = Instantiate(Resources.Load("Heart") as GameObject);
        firstHeart.transform.position = new Vector3(-4f, 12f, 0f);
        for (int i = 0; i < player.hp - 1; i++)
        {
            var curHeart = Instantiate(Resources.Load("Heart") as GameObject);
            curHeart.transform.SetParent(firstHeart.transform, false);
            curHeart.transform.localPosition = new Vector3(-4f * (i + 1), 12f * (i + 1), 0f);
        }
    }

    void SetUpMap()
    {
        mapPrefabName = "Map0";
        switch (mapPrefabName)
        {
            case "Map0":
                mapBackgroundName = "FieldBackground";
                break;
            case "Map1":
                mapBackgroundName = "GreenBackground";
                break;
            case "Map2":
                mapBackgroundName = "BrickBackground";
                break;
            default:
                mapBackgroundName = "FieldBackground";
                break;
        }
    }

    void SetUpMap(int choice)
    {
        mapPrefabName = "Map" + choice.ToString();
        switch (mapPrefabName)
        {
            case "Map0":
                mapBackgroundName = "FieldBackground";
                break;
            case "Map1":
                mapBackgroundName = "GreenBackground";
                break;
            case "Map2":
                mapBackgroundName = "BrickBackground";
                break;
            default:
                mapBackgroundName = "FieldBackground";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var x = (int)(GameObject.Find("Slider0-100").GetComponent<Slider>().value * 100);
        GameObject.Find("RecoverRateDynamic").GetComponent<Text>().text = x.ToString();
        if (started && !dead)
        {
            map_go.transform.Translate(Vector3.left * Time.deltaTime);
            if (map_go.transform.localPosition.x <= -75 && !added)
            {
                added = true;
                map_go_new = Instantiate(Resources.Load(mapPrefabName)) as GameObject;
                map_go_new.transform.SetParent(map_go.transform, false);
                var locPos = new Vector3(0f, 0f, 0f);
                //locPos.x += map_go.GetComponent<Renderer>().bounds.size.x;
                locPos.x += GameObject.Find(mapBackgroundName).GetComponent<Renderer>().bounds.size.x * 3;
                map_go_new.transform.localPosition = locPos;
                map_go_new.transform.localScale = new Vector3(1f, 1f, 1f);
                deleted = false;
            }
            if (map_go.transform.localPosition.x <= -130 && !deleted)  // fix gap -> to dynamic
            {
                deleted = true;
                map_go_new.transform.SetParent(mapParent.transform, true);
                Destroy(map_go);
                map_go = map_go_new;
                map_go_new = null;
                added = false;
            }
        }
        if (dead)
        {
            Destroy(map_go);
            Destroy(map_go_new);
            var bg = GameObject.Find("BH-field0");
            bg.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }


    }

    void PopulateDropdown(GameObject dropdown)
    {
        List<string> templates_tba = new List<string>();
        for (int i = 1; i < templates.Length; i++)
        {
            templates_tba.Add(templates[i][0]);
        }
        dropdown.GetComponent<Dropdown>().ClearOptions();
        dropdown.GetComponent<Dropdown>().AddOptions(templates_tba);
    }

    public void AddTemplateSlot()
    {
        dropdownCount += 1;
        var newPos = UpdateDropdownLocalPos(dropdownCount);
        var newSlot = Instantiate(Resources.Load("Dropdown")) as GameObject;
        //var newSlot = Instantiate(dropdown);
        newSlot.transform.SetParent(dropdownParent.transform, false);
        //var newPos = dropdownLocalPos;
        newSlot.transform.localPosition = newPos;
        newSlot.name += dropdownCount;
        PopulateDropdown(newSlot);

        var input = GameObject.Find("GapAfterTemplate");
        input.name += dropdownCount;

        var text = GameObject.Find("Text");
        text.name += dropdownCount;
    }

    private Vector3 UpdateDropdownLocalPos(int count)
    {
        var newPos = dropdownLocalPos;
        newPos.y -= count * 30;
        //dropdownLocalPos = newPos;
        return newPos;
    }

    public void StartDemo()
    {
        dropdownParent.GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("AddTemplateSlot").GetComponent<CanvasGroup>().alpha = 0;

        var bg = GameObject.Find("BH-field0");
        bg.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);

        GameObject.Find("Demo").GetComponent<CanvasGroup>().alpha = 0;

        // read from first dropdown for now
        choice = GameObject.Find("Dropdown(Clone)1").GetComponent<Dropdown>().value;
        SetUpMap(choice);
        PlayMusic(choice + 1);  // don't know which motherfucker choose to index from 1

        map_go = Instantiate(Resources.Load(mapPrefabName)) as GameObject;
        map_go.transform.SetParent(mapParent.transform, false);
        map_go.transform.localPosition = new Vector3(0f, 0f, 0f);
        //StartCoroutine(mapScroll());
        started = true;
    }

    private IEnumerator mapScroll()
    {
        map_go.transform.Translate(Vector3.left * Time.deltaTime);
        yield return null;
    }

    void ReadInTemplates(string path)
    {
        //var contents = File.ReadAllText(Application.dataPath + path).Split('\n');
        var contents = Resources.Load(path).ToString().Split('\n');
        var csv = from line in contents
                  select line.Split(',').ToArray();
        templates = csv.ToArray();
        Debug.Log("Template Read In successful: " + templates[0][0]);
    }
}
