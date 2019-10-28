using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (collision.collider.gameObject.tag == "Bullet")
            {
                Vector2 distanceTravelled = (collision.collider.gameObject.GetComponent<Bullet>().startPoint - transform.position);
                GetComponent<EnemyBehaviour>().takeDamage(collision.collider.gameObject.GetComponent<Bullet>().bulletDamage);

                if (collision.collider.gameObject.GetComponent<PhotonView>().isMine)
                {
                    collision.collider.gameObject.GetComponent<Bullet>().enabled = false;
                }
            }
            else
            {
                Debug.Log("Tag: " + collision.collider.gameObject.tag);
            }
        }
    }
}
