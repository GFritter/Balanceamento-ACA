using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int player_class;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetPlayerClass()
    {
        return player_class;
    }

    public void SetPlayerClass(string pClass)
    {
        if(pClass == "Engineer")
        {
            player_class = 0;
        }
        else
        {
            player_class = 1;
        }
    }
}
