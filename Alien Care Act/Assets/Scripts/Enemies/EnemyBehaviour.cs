﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum mode { idle, roaming, approaching, attacking };

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 75;

    [SerializeField]
    private float health = 75;
    private float movementSpeed;
    public GameObject healthBar;
    [SerializeField]
    private float attackSpeed;
    private float nextAtk;
    private float nextTargetChange;

    private photonHandler pHandler;

    private EnemyManager enemyMgr;

    public Collider2D aggroRange;
    public int damageAmount = 5;

    [SerializeField]
    private Collider2D[] colliders;

    [SerializeField]
    private Vector2 idleTarget;

    private List<GameObject> PossibleTargets;
    GameObject Target = null;

    GameObject Nexus;

    [SerializeField]
    mode status;

    
    public I_EBehaviour behaviour;

    private bool stop;
    private bool knocking;

    // Start is called before the first frame update
    void Start()
    {
        PossibleTargets = new List<GameObject>();
        pHandler = GameObject.Find("photonDontDestroy").GetComponent<photonHandler>();
        enemyMgr = GameObject.Find("photonDontDestroy").GetComponent<EnemyManager>();
        movementSpeed = 2;
        attackSpeed = 0.7f;
        nextAtk = 0.3f;

        healthBar.GetComponent<Slider>().maxValue = maxHealth;

        Nexus = GameObject.Find("map_holder").transform.Find("Nexus").gameObject;

        colliders = new Collider2D[25];

        status = mode.idle;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Turret" || collision.gameObject.tag == "Barrier" || collision.gameObject.tag == "Nexus")
        {
            PossibleTargets.Add(collision.gameObject);
            if (Target == null)
            {
                Target = behaviour.behaviour_GetPreferredTarget(PossibleTargets);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Turret" || collision.gameObject.tag == "Barrier" || collision.gameObject.tag == "Nexus")
        {
            PossibleTargets.Remove(collision.gameObject);
            if (Target == collision.gameObject)
            {
                Target = behaviour.behaviour_GetPreferredTarget(PossibleTargets);
            }
        }
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (PhotonNetwork.isMasterClient)
        {
            StateMachine();
        }
        UpdateHealthBar();
    }

    void Follow()
    {
        if (DistanceToTarget() < 10)
        {
            transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, movementSpeed * Time.deltaTime);
        }
        else
        {
            Search();
        }
    }

    void Search()
    {
        transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, movementSpeed / 2 * Time.deltaTime);
    }

    void HandleAI()
    {
        if (!knocking)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if (Time.time > nextTargetChange)
        {
            nextTargetChange += 3f;
            Target = behaviour.behaviour_GetPreferredTarget(PossibleTargets);
        }

        if (Target != null)
        {
            if (DistanceToTarget() < 3)
            {
                attack();
            }
            else if (DistanceToTarget() < 10)
            {
                Follow();
            }
            else
            {
                Search();
            }
        }
        else
        {
            Target = Nexus;
        }
    }

    float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, Target.transform.position);
    }

    void attack()
    {
        if (Time.time > nextAtk)
        {
            nextAtk = Time.time + attackSpeed;

            applyDamage(Target);
        }
    }

    void applyDamage(GameObject target)
    {
        if (target.GetActive())
        {
            switch (target.tag)
            {
                case "Player":
                    target.GetComponent<PlayerLife>().ApplyDamage(damageAmount);
                    break;
                case "Nexus":
                    target.GetComponent<NexusManager>().ApplyDamage(damageAmount);
                    break;
                case "Turret":
                    target.GetComponent<BuildingProperties>().ApplyDamage(damageAmount);
                    break;
                case "Barrier":
                    target.GetComponent<BuildingProperties>().ApplyDamage(damageAmount);
                    break;
            }
        }
    }

    private void DropScrap()
    {
        PhotonNetwork.Instantiate("Scrap", gameObject.transform.position, gameObject.transform.rotation, 0);
    }

    private void damageEnemy(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            DropScrap();
            enemyMgr.RemoveEnemy();
            PhotonNetwork.Destroy(gameObject);
        }
        UpdateHealthBar();
    }

    public void takeDamage(float amount)
    {
        if (PhotonNetwork.isMasterClient)
        {
            damageEnemy(amount);
        }
    }

    public void applyKnockback(Vector2 force)
    {
        if (!knocking)
        {
            knocking = true;
            GetComponent<Rigidbody2D>().AddForce(force);
            StartCoroutine("KnockbackEnd");
        }
    }

    private IEnumerator KnockbackEnd()
    {
        yield return new WaitForSeconds(0.15f);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        knocking = false;
    }

    private void UpdateHealthBar()
    {
        if (health < maxHealth && healthBar.activeSelf == false)
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


    public void StateMachine()
    {
        switch (status)
        {

            case (mode)0: idle(); break;
            case (mode)1: roam(); break;
            case (mode)2: approach(); break;
            case (mode)3: attack(); break;




        }


    }


    void idle()
    {
        idleTarget = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
        status = mode.roaming;


    }

    void roam()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, idleTarget, movementSpeed * Time.deltaTime);

        aggroRange.GetContacts(colliders);
        if (colliders[0] != null)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] == null)
                    break;

                else if (colliders[i].transform.gameObject.tag == "Player" || colliders[i].transform.gameObject.tag == "Buildings")
                {
                    Target = colliders[i].transform.gameObject;
                    status = mode.approaching;

                    return;
                }

                else if (colliders[i].transform.gameObject.tag == "wall")
                {
                    Debug.Log("bati na parede");
                    colliders = new Collider2D[10];
                    status = mode.idle;
                    return;
                }
            }
        }


        if (transform.position.AlmostEquals(idleTarget, movementSpeed))
        {
            status = mode.idle;
        }

    }

    void approach()
    {
        if (Vector2.Distance(transform.position, Target.transform.position) > 3)
        {
            transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, movementSpeed * Time.deltaTime);

            aggroRange.GetContacts(colliders);
            if (colliders[0] != null)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i] == null)
                        break;

                    else if (colliders[i].transform.gameObject.tag == "wall")
                    {
                        Debug.Log("bati na parede");
                        colliders = new Collider2D[10];
                        status = mode.idle;
                        Target = null;
                        return;
                    }
                }
            }

            else
            {
                status = mode.attacking;
            }
        }
    }


    void attacking()
    {
        if (Time.time > nextAtk)
        {
            nextAtk = Time.time;
            //fun;'ao de ataque

            Debug.Log("Ataquei sim");
        }
    }


}
