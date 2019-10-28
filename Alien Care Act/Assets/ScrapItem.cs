using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapItem : MonoBehaviour
{
    public int scrapValue;

    EconomyManager eManager;

    private void Start()
    {
        scrapValue = Random.Range(5, 15);
        eManager = GameObject.Find("LevelManager").GetComponent<EconomyManager>();
        Destroy(gameObject, 15);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (PhotonNetwork.isMasterClient && collider.tag == "Player" && collider.gameObject.GetComponent<PlayerManager>().GetPlayerClass() == 1)
        {
            Debug.Log("Mandei Adicionar e destrui");
            eManager.AddScraps(scrapValue);
            PhotonNetwork.Destroy(gameObject);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
