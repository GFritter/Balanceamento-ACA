using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public int Health = 1000;
    private bool Alive = true;

    public GameObject PlayerHolder;
    private photonHandler pHandler;
    private MenuManager lManager;
    private EconomyManager eManager;

    
    void Start()
    {
        pHandler = GameObject.Find("photonDontDestroy").GetComponent<photonHandler>();
        lManager = GameObject.Find("LevelManager").GetComponent<MenuManager>();
        eManager = GameObject.Find("LevelManager").GetComponent<EconomyManager>();
    }
    

    public bool IsAlive()
    {
        return Alive;
    }

    public void ApplyDamage(int damage)
    {
        Health -= damage;

        if(Health <= 0)
        {
            TempRespawnPlayer();
            eManager.RemoveHalfScraps();

            //We will need to verify many things on this implementation later, for now, we will only make the player
            //get back to its initial position and lose half of his current scraps.
            //---------KillThisPlayer();
            //---------ActivateRespawnMenu(true);
        }
    }

    private void TempRespawnPlayer()
    {
        Health = 1000;
        if(gameObject.name == "Engineer (Clone)")
        {
            transform.position = pHandler.engSpawnPlace.position;
        }
        else if (gameObject.name == "Scrapper (Clone)")
        {
            transform.position = pHandler.scrapperSpawnPlace.position;
        }
    }
    
    private void ActivateRespawnMenu(bool flag)
    {
        lManager.ActivateDeathMenu(flag);
    }

    public void KillThisPlayer()
    {
        Alive = false;
        pHandler.MyPlayerDied(gameObject.name);
    }
    
    public void RespawnThisPlayer()
    {
        Alive = true;
        Health = 1000;
        pHandler.RespawnPlayer(gameObject.name);
        ActivateRespawnMenu(false);
    }
}
