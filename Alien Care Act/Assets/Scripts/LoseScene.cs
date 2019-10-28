using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseScene : MonoBehaviour
{
    private photonConnect pConnect;
    // Start is called before the first frame update
    void Start()
    {
        pConnect = GameObject.Find("photonDontDestroy").GetComponent<photonConnect>();
    }
    
    public void OnClickMainMenu()
    {
        pConnect.BackToMainMenu();
    }
}
