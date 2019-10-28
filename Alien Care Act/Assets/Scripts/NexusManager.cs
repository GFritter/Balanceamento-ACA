using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusManager : MonoBehaviour
{
    public float health;

    private EndGameManager egManager;
    // Start is called before the first frame update
    void Start()
    {
        egManager = GameObject.Find("LevelManager").GetComponent<EndGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyDamage(float damage)
    {
        if (PhotonNetwork.isMasterClient)
        {
            health -= damage;
            if(health <= 0)
            {
                egManager.EndGame(false);
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
