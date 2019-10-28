using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class photonButton : MonoBehaviour
{
    public photonHandler pHandler;

    public InputField hostGame, joinGame, playerName;

    public void OnClickCreateRoom()
    {
        pHandler.CreateRoom();    
    }

    public void OnClickJoinRoom()
    {
        pHandler.JoinRoom();
    }

}
