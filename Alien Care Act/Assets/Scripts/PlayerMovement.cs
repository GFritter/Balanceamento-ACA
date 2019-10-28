using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Photon.MonoBehaviour
{
    public float movement_speed = 5f;

    public PhotonView photon_View;

    private Vector3 selfPos;

    private GameObject sceneCamera;
    public GameObject playerCamera;

    private void Awake()
    {
        if (photon_View.isMine)
        {
            sceneCamera = GameObject.Find("Main Camera");
            sceneCamera.SetActive(false);
            playerCamera.SetActive(true);
        }
        
    }

    private void Update()
    {
        if (photon_View.isMine)
        {
            handleInputs();
        }
        else
        {
            smoothNetMovement();
        }
    }

    private void handleInputs()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.position += move * movement_speed * Time.deltaTime;
    }

    private void smoothNetMovement()
    {
        transform.position = Vector3.Lerp(transform.position, selfPos, Time.deltaTime * 8);
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            selfPos = (Vector3)stream.ReceiveNext();
        }
    }
}
