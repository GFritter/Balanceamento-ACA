using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLobbyPanel : MonoBehaviour
{
    public Text player_class;

    public string host_name, client_name;

    public void SetClass(string p_class)
    {
        player_class.text = p_class;
    }

    public string GetClass()
    {
        return player_class.text;
    }

    public string GetHostName()
    {
        return host_name;
    }

    public string GetClientName()
    {
        return client_name;
    }

    public void SetHostName(string name)
    {
        host_name = name;
    }

    public void SetClientName(string name)
    {
        client_name = name;
    }
}
