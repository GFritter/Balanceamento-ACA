using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public GameObject aimObject;

    private Camera mainCamera;
    private PhotonView pView;
    
    void Start()
    {
        mainCamera = transform.Find("Camera").GetComponent<Camera>();
        pView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (pView.isMine)
        {
            Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane screenPlane = new Plane(Vector3.back, Vector3.zero);

            if (screenPlane.Raycast(cameraRay, out float rayLenght))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLenght);
                aimObject.transform.position = pointToLook;
            }
        }
        else
        {
            aimObject.SetActive(false);
        }
    }
}
