using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lobbyManager : MonoBehaviour
{
    public GameObject host_panel, client_panel;

    public photonHandler pHandler;

    public PhotonView photonView;

    public GameObject connectionPanel;

    public GameObject roomName;

    public PlayerLobbyPanel host, client;

    private void Awake()
    {
        photonView = PhotonView.Get(this);
    }

    public void OnHostReady()
    {
        if (!IsHostReady())
            photonView.RPC("RPCSetHostReady", PhotonTargets.All);
        else
            photonView.RPC("RPCSetHostUnready", PhotonTargets.All);
    }
    
    public void OnClientReady()
    {
        if (!IsClientReady())
            photonView.RPC("RPCSetClientReady", PhotonTargets.All);
        else
            photonView.RPC("RPCSetClientUnready", PhotonTargets.All);
    }

    [PunRPC]
    public void RPCSetHostUnready()
    {
        host_panel.GetComponent<Image>().color = Color.red;
    }

    [PunRPC]
    public void RPCSetClientUnready()
    {
        client_panel.GetComponent<Image>().color = Color.red;
    }


    [PunRPC]
    public void RPCSetHostReady()
    {
        host_panel.GetComponent<Image>().color = Color.green;
    }

    [PunRPC]
    public void RPCSetClientReady()
    {
        client_panel.GetComponent<Image>().color = Color.green;
    }

    [PunRPC]
    public void RPCStartGame()
    {
        pHandler.StartGame();
    }

    [PunRPC]
    public void RPCRequestHostClass()
    {
        Debug.Log("Requisitei a host_class");
        bool ready;
        if (host_panel.GetComponent<Image>().color == Color.red)
            ready = false;
        else
            ready = true;
        photonView.RPC("RPCSetHostClassInClient", PhotonTargets.Others, host.GetClass(), ready);
    }

    [PunRPC]
    public void RPCSetHostClassInClient(string host_class, bool ready_status)
    {
        Debug.Log("Setei a classe do cliente para: " + host_class);
        host.SetClass(host_class);
        if (ready_status != false)
            host_panel.GetComponent<Image>().color = Color.red;
        else
            host_panel.GetComponent<Image>().color = Color.green;
    }

    [PunRPC]
    public void RPCRequestClientClass()
    {
        photonView.RPC("RPCSetClientClassInHost", PhotonTargets.Others, client.GetClass());
    }

    [PunRPC]
    public void RPCSetClientClassInHost(string client_class)
    {
        client.SetClass(client_class);
    }

    public void OnJoinRoom(string player_name, string selected_char)
    {
        roomName.GetComponent<Text>().text = "Room: " + PhotonNetwork.room.Name;
        if (PhotonNetwork.isMasterClient)
        {
            host.SetHostName(player_name);
            host.SetClass(selected_char);
            host_panel.transform.Find("player_name").GetComponent<Text>().text = host.GetHostName();
            Debug.Log("Ativei Painel de host...");
            host_panel.SetActive(true);
            client_panel.transform.Find("client_ready").GetComponent<Button>().interactable = false;
        }
        else
        {
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                if (player.IsMasterClient)
                {
                    host.SetHostName(player.NickName);
                    photonView.RPC("RPCRequestHostClass", PhotonTargets.MasterClient);
                    if(host.GetClass() == selected_char)
                    {
                        Debug.Log("Host class: " + host.GetClass());
                        Debug.Log("selected_char: " + selected_char);
                        pHandler.GetOutOfTheRoom();
                    }
                }
            }
            host_panel.transform.Find("player_name").GetComponent<Text>().text = host.GetHostName();
            host_panel.SetActive(true);
            host_panel.transform.Find("host_ready").GetComponent<Button>().interactable = false;
            client.SetClientName(player_name);
            client.SetClass(selected_char);
            client_panel.transform.Find("player_name").GetComponent<Text>().text = client.GetClientName();
            client_panel.SetActive(true);
        }
    }

    public void OnSomeoneJoined(PhotonPlayer player)
    {
        if (!player.IsMasterClient)
        {
            client.SetClientName(player.NickName);
            photonView.RPC("RPCRequestClientClass", PhotonTargets.Others);
            client_panel.transform.Find("player_name").GetComponent<Text>().text = client.GetClientName();
            client_panel.SetActive(true);
        }
    }

    public void OnMasterLeftRoom()
    {
        Debug.Log("Master saiu, eu sou o master client já?" + PhotonNetwork.isMasterClient);
        if (!PhotonNetwork.isMasterClient) //Aqui eu não sou mais o master client ainda.
        {
            host.SetHostName(client.GetClientName());
            host.SetClass(client.GetClass());
            host_panel.transform.Find("player_name").GetComponent<Text>().text = host.GetHostName();
            client.SetClass("");
            client.SetClientName("");
            client_panel.SetActive(false);
            host_panel.transform.Find("host_ready").GetComponent<Button>().interactable = true;
            client_panel.transform.Find("client_ready").GetComponent<Button>().interactable = false;
            host_panel.GetComponent<Image>().color = Color.red;
        }
    }
    public void OnClientLeftRoom()
    {
        if (PhotonNetwork.isMasterClient)
        {
            client_panel.SetActive(false);
            client_panel.GetComponent<Image>().color = Color.red;
        }
    }

    public void OnClickLeftRoom()
    {
        ResetLobby();
        pHandler.GetOutOfTheRoom();
    }

    private bool IsClientReady()
    {
        if (client_panel.GetComponent<Image>().color == Color.green)
            return true;
        else
            return false;
    }

    private bool IsHostReady()
    {
        if (host_panel.GetComponent<Image>().color == Color.green)
            return true;
        else
            return false;
    }

    public bool CanStartGame()
    {
        if(IsHostReady() && IsClientReady())
            return true;
        else
            return false;
    }

    public void OnClickStartGame()
    {
        if (PhotonNetwork.isMasterClient && CanStartGame())
        {
            photonView.RPC("RPCStartGame", PhotonTargets.All);
        }
    }

    public void Deactivate()
    {
        host_panel.SetActive(false);
        client_panel.SetActive(false);
        connectionPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void ResetLobby()
    {
        host.SetClass("");
        host.SetClientName("");
        host.SetHostName("");
        client.SetClass("");
        client.SetClientName("");
        client.SetHostName("");
        host_panel.GetComponent<Image>().color = Color.red;
        client_panel.GetComponent<Image>().color = Color.red;
        client_panel.transform.Find("client_ready").GetComponent<Button>().interactable = true;
        host_panel.transform.Find("host_ready").GetComponent<Button>().interactable = true;
    }
}
