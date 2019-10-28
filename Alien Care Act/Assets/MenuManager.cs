using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public GameObject gameMenu;
    public GameObject deathMenu;
    public photonHandler pHandler;
    public GameObject tempCamera;

    void Start()
    {
        pHandler = GameObject.Find("photonDontDestroy").GetComponent<photonHandler>();
    }

    void Update()
    {
        CheckInputs();
    }

    public void ActivateDeathMenu(bool flag)
    {
        tempCamera.SetActive(flag);
        deathMenu.SetActive(flag);
    }

    public void OnClickRespawn()
    {
        pHandler.RespawnPlayer(pHandler.GetMyPlayerName());
    }

    void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            gameMenu.SetActive(!gameMenu.activeInHierarchy);
        }
    }
    public void QuitInGame()
    {
        pHandler.OnClickQuitInGame();
    }
}
