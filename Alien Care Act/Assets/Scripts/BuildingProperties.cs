using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingProperties : MonoBehaviour
{
    public int Value;

    [SerializeField]
    private float health = 100;
    public float MaxHealth = 100;

    public GameObject disabledIndicator;

    public GameObject healthBar;
    // Start is called before the first frame update
    public string ScriptToDisable;

    private void Start()
    {
        healthBar.GetComponent<Slider>().maxValue = MaxHealth;
    }

    private void Update()
    {
        UpdateHealthBar();
    }

    public int GetValue()
    {
        return Value;
    }

    [PunRPC]
    public void Repair(float amount)
    {
        health += amount;
        if (health >= MaxHealth)
        {
            health = MaxHealth;
            if (!IsBuildingEnabled())
            {
                GetComponent<PhotonView>().RPC("EnableBuilding", PhotonTargets.All);
                if (healthBar.activeSelf == true)
                    healthBar.SetActive(false);
            }
        }
    }
    public void CallRepair(float amount)
    {
        GetComponent<PhotonView>().RPC("Repair", PhotonTargets.MasterClient, amount);
    }

    public void ApplyDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            if (IsBuildingEnabled())
            {
                GetComponent<PhotonView>().RPC("DisableBuilding", PhotonTargets.All);
            }
        }
        UpdateHealthBar();
    }

    public bool IsBuildingEnabled()
    {
        if (ScriptToDisable == "TurretBehaviour" && GetComponent<TurretBehaviour>().enabled == true)
        {
            return true;
        }
        else if (ScriptToDisable == "BarrierBehaviour" && GetComponent<BarrierBehaviour>().enabled == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [PunRPC]
    public void EnableBuilding()
    {
        if (ScriptToDisable == "TurretBehaviour")
        {
            GetComponent<TurretBehaviour>().enabled = true;
        }
        else if (ScriptToDisable == "BarrierBehaviour")
        {
            GetComponent<BarrierBehaviour>().enabled = true;
        }
        disabledIndicator.SetActive(false);
    }
    
    [PunRPC]
    public void DisableBuilding()
    {
        if (ScriptToDisable == "TurretBehaviour")
        {
            GetComponent<TurretBehaviour>().enabled = false;
        }
        else if (ScriptToDisable == "BarrierBehaviour")
        {
            GetComponent<BarrierBehaviour>().enabled = false;
        }
        disabledIndicator.SetActive(true);
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
