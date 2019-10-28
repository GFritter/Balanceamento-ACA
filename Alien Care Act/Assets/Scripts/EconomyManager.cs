using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EconomyManager : MonoBehaviour
{
    int Scraps = 0;
    public Text scrap_amount;

    public photonHandler pHandler;

    public 

    // Start is called before the first frame update
    void Start()
    {
        pHandler = GameObject.Find("photonDontDestroy").GetComponent<photonHandler>();
    }
    
    void UpdateScraps(int s)
    {
        //if (PhotonNetwork.isMasterClient)
        //{
        pHandler.SetScrapsForAll(s);
        //}
    }

    public void SetScraps(int s)
    {
        Scraps = s;
    }

    // Update is called once per frame
    void Update()
    {
        scrap_amount.text = "" + Scraps;
    }

    public void RemoveHalfScraps()
    {
        Scraps = Scraps / 2;
        UpdateScraps(Scraps);
    }

    public void AddScraps(int s)
    {
        Scraps += s;
        UpdateScraps(Scraps);
    }

    public int GetScraps()
    {
        return Scraps;
    }

    public bool BuyBuilding(int value)
    {
        if(value > Scraps)
        {
            return false;
        }
        else
        {
            Scraps -= value;
            UpdateScraps(Scraps);
            return true;
        }
    }
}
