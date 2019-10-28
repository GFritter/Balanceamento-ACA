using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class photonConnect : MonoBehaviour
{
    public string versionName = "0.1";

    public GameObject menuPanel; 

    public GameObject connectingPanel;

    public GameObject connectionPanel;

    public GameObject disconnectPanel;

    private bool rejoin, quit;

    public void ConnectToGameServers()
    {
        menuPanel.SetActive(false);
        PhotonNetwork.ConnectUsingSettings(versionName);

        Debug.Log("Connectiong to photon Game Servers...");
        connectingPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        connectionPanel.SetActive(false);
        menuPanel.SetActive(true);
        DisconnectFromMasterServer();
    }

    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        connectingPanel.SetActive(false);
        connectionPanel.SetActive(true);
    }

    private void OnJoinedLobby()
    {
        Debug.Log("On Joined Lobby");
    }

    private void OnConnectionFail(DisconnectCause cause)
    {
        if (PhotonNetwork.Server == ServerConnection.GameServer)
        {
            switch (cause)
            {
                // add other disconnect causes that could happen while joined
                case DisconnectCause.DisconnectByClientTimeout:
                    rejoin = true;
                    break;
                case DisconnectCause.ExceptionOnConnect:
                    rejoin = false;
                    break;
                case DisconnectCause.Exception:
                    rejoin = false;
                    break;
                default:
                    rejoin = false;
                    break;
            }
        }
    }

    private void OnDisconnectedFromPhoton()
    {
        if (rejoin && !PhotonNetwork.ReconnectAndRejoin())
        {
            SceneManager.LoadScene("LostConnection", LoadSceneMode.Single);
        }
        else if(rejoin && PhotonNetwork.ReconnectAndRejoin())
        {
            Debug.Log("Reconnecting...");
        }
        if (quit)
        {
            disconnectPanel.SetActive(false);
        }
    }

    private void OnFailedToConnectToPhoton()
    {
        Debug.Log("Failed to connect to Game Server");
    }

    public void DisconnectFromMasterServer()
    {
        rejoin = false;
        quit = true;
        PhotonNetwork.Disconnect();
        disconnectPanel.SetActive(true);
    }
}
