using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    public photonHandler pHandler;

    public GameObject tumor_parent;

    private void Start()
    {
        pHandler = GameObject.Find("photonDontDestroy").GetComponent<photonHandler>();
    }

    private void Update()
    {
        if (tumor_parent.transform.childCount == 0)
        {
            EndGame(true);
        }
    }

    public void EndGame(bool win)
    {
        pHandler.EndGame(win);
    }
}
