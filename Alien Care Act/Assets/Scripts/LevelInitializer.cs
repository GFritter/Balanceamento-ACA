using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.isMasterClient)
        {
            GameObject T1 = PhotonNetwork.Instantiate("Tumor", new Vector3(20, 30, 0), Quaternion.identity, 0);
            T1.transform.SetParent(transform);
            T1.transform.localScale = new Vector2(1.5f, 1.5f);
            GameObject T2 = PhotonNetwork.Instantiate("Tumor", new Vector3(30, 20, 0), Quaternion.identity, 0);
            T2.transform.SetParent(transform);
            T2.transform.localScale = new Vector2(1.5f, 1.5f);
            GameObject T3 = PhotonNetwork.Instantiate("Tumor", new Vector3(-30, 25, 0), Quaternion.identity, 0);
            T3.transform.SetParent(transform);
            T3.transform.localScale = new Vector2(1.5f, 1.5f);
            GameObject T4 = PhotonNetwork.Instantiate("Tumor", new Vector3(-30, -30, 0), Quaternion.identity, 0);
            T4.transform.SetParent(transform);
            T4.transform.localScale = new Vector2(1.5f, 1.5f);
            GameObject T5 = PhotonNetwork.Instantiate("Tumor", new Vector3(30, -30, 0), Quaternion.identity, 0);
            T5.transform.SetParent(transform);
            T5.transform.localScale = new Vector2(1.5f, 1.5f);
        }        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
