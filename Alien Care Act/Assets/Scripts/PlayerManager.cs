using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int player_class;
   
   
    public int GetPlayerClass()
    {
        return player_class;
    }

    public void SetPlayerClass(string pClass)
    {
        if(pClass == "Engineer")
        {
            player_class = 0;

            //acho que o problema é aqui
        }
        else
        {
            player_class = 1;
        }
    }
}
