using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EngineerConstruction : MonoBehaviour
{
    public Camera mainCamera;
    public PhotonView pView;

    public GameObject cPanel;

    public GameObject selectedConstruction;
    private int constructionSelection = 0;

    public List<GameObject> possibleConstructions;

    public GameObject buildablePlaceIndicator;

    private EconomyManager eManager;

    public bool building;


    void Start()
    {
        Init();
    }

    void Init()
    {
        cPanel = GameObject.Find("Canvas").transform.Find("ConstructionPanel").gameObject;
        eManager = GameObject.Find("LevelManager").GetComponent<EconomyManager>();
        UpdateConstructions();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
    }

    void CheckInputs()
    {
        if (pView.isMine)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                building = !building;
                if (cPanel == null || mainCamera == null)
                {
                    Init();
                }
                cPanel.SetActive(building);
                buildablePlaceIndicator.SetActive(building);
            }
            if (building)
            {
                ShowBuildable();
                if (Input.GetMouseButtonDown(0))
                {
                    ScreenMouseRay();
                }
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    if (constructionSelection < possibleConstructions.Count - 1)
                    {
                        constructionSelection = constructionSelection + 1;
                    }
                    else
                    {
                        constructionSelection = 0;
                    }
                    UpdateConstructions();
                }
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    if (constructionSelection > 0)
                    {
                        constructionSelection = constructionSelection - 1;
                    }
                    else
                    {
                        constructionSelection = possibleConstructions.Count - 1;
                    }
                    UpdateConstructions();
                }
            }
            
        }
    }
    
    void UpdateConstructions()
    {
        selectedConstruction = possibleConstructions[constructionSelection];
    }

    void Construction(GameObject whereToBuild, GameObject construction)
    {
        if (eManager.BuyBuilding(construction.GetComponent<BuildingProperties>().GetValue()))
        {
            whereToBuild.tag = construction.tag;
            PhotonNetwork.Instantiate(construction.name, whereToBuild.transform.position, whereToBuild.transform.rotation, 0);
        }
    }

    void ShowBuildable()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5f;

        Vector2 v = mainCamera.ScreenToWorldPoint(mousePosition);

        Collider2D[] col = Physics2D.OverlapPointAll(v);

        if (col.Length > 0)
        {
            foreach (Collider2D c in col)
            {
                if (c.tag == "Buildable" && eManager.GetScraps()<=200)
                {
                    buildablePlaceIndicator.GetComponent<SpriteRenderer>().color = Color.red;
                    buildablePlaceIndicator.transform.position = c.gameObject.transform.position;
                }
                else if(c.tag=="Buildable")
                {
                    buildablePlaceIndicator.GetComponent<SpriteRenderer>().color = Color.green;
                    buildablePlaceIndicator.transform.position = c.gameObject.transform.position;
                }
                
            }
        }
    }

    void ScreenMouseRay()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5f;

        Vector2 v = mainCamera.ScreenToWorldPoint(mousePosition);

        if (Vector2.Distance(v, transform.position) < 5)
        {
            Collider2D[] col = Physics2D.OverlapPointAll(v);

            if (col.Length > 0)
            {
                foreach (Collider2D c in col)
                {
                    if (c.tag != "Buildable")
                    {
                        Debug.Log("Can't Build Here");
                    }
                    else
                    {
                        Debug.Log(c.tag);
                        Construction(c.gameObject, selectedConstruction);
                    }
                }
            }
        }
    }

}
