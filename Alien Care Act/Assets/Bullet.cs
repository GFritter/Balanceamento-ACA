using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 1000f;
    public Vector3 startPoint;
    public float bulletKnockback = 1.0f;
    public float bulletDamage = 2.0f;

    void Start()
    {
        startPoint = transform.position;
        
        Destroy(gameObject, 2);
        GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed);
        if (GetComponent<PhotonView>().owner != PhotonNetwork.masterClient)
        {
            GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.masterClient.ID);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Wall" || collision.collider.tag == "Nexus")
        {
            enabled = false;
        }
    }
    private void OnDisable()
    {
        if (GetComponent<PhotonView>().isMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
