using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionPanel : MonoBehaviour
{
    public Image selectedConstruction;
    void Update()
    {
        selectedConstruction.sprite = GameObject.Find("Engineer(Clone)").GetComponent<EngineerConstruction>().selectedConstruction.GetComponent<SpriteRenderer>().sprite;
    
    }

}
