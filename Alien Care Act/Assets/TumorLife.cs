using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TumorLife : MonoBehaviour
{
    [SerializeField]
    private float health = 10000;
    private float MaxHealth = 10000;

    public GameObject healthBar;

    private photonHandler pHandler;

    private void Start()
    {
        pHandler = GameObject.Find("photonDontDestroy").GetComponent<photonHandler>();
        healthBar.GetComponent<Slider>().maxValue = MaxHealth;
    }

    public void damageTumor(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        UpdateHealthBar();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            float amount = collision.collider.gameObject.GetComponent<Bullet>().bulletDamage;
            damageTumor(amount);
            collision.collider.gameObject.GetComponent<Bullet>().enabled = false;
        }
    }

    private void UpdateHealthBar()
    {
        if (health < MaxHealth && healthBar.activeSelf == false)
        {
            healthBar.SetActive(true);
        }

        healthBar.GetComponent<Slider>().value = health;

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (stream.isWriting)
            {
                stream.SendNext(health);
            }
        }
        else
        {
            health = (float)stream.ReceiveNext();
            UpdateHealthBar();
        }
    }
}
