using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    private BuildingProperties bProperties;

    List<GameObject> PossibleTargets;
    GameObject Target = null;

    public Transform bullet_start_point;
    public Transform TurretHead;

    public LayerMask layerMask;

    public float TurretFireRate = 0.5f;
    public float turret_damage = 5.0f;
    public float turret_knockback = 75.0f;

    float NextFire = 0;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            PossibleTargets.Add(collision.gameObject);
            if (Target == null)
            {
                Target = GetClosestTarget();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            PossibleTargets.Remove(collision.gameObject);
            if (Target == collision.gameObject)
            {
                Target = GetClosestTarget();
            }
        }
    }

    private void Start()
    {
        PossibleTargets = new List<GameObject>();
        bProperties = this.GetComponent<BuildingProperties>();
    }

    private void Update()
    {
        if (PhotonNetwork.isMasterClient)
        {
            HandleAttacks();
        }
    }

    private void HandleAttacks()
    {
        if (Target != null && (Time.time > NextFire))
        {
            LookToTarget(Target);
            GetComponent<PhotonView>().RPC("ShootAtTarget", PhotonTargets.All, Target.transform.position);
        }
        else if (PossibleTargets.Count > 0)
        {
            UpdatePossibleTargets();
            Target = GetClosestTarget();
        }
    }

    [PunRPC]
    private void ShootAtTarget(Vector3 target)
    {
        NextFire = Time.time + TurretFireRate;
        
        RaycastHit2D hit2D;
        Vector2 Direction = target - bullet_start_point.position;
        hit2D = Physics2D.Raycast(bullet_start_point.position, Direction, 1000f, layerMask);

        SpawnShot(hit2D.point);

        if (PhotonNetwork.isMasterClient)
        {
            if (hit2D.collider.tag == "Enemy")
            {
                EnemyBehaviour enemy = hit2D.collider.gameObject.GetComponent<EnemyBehaviour>();
                if (enemy != null)
                {
                    enemy.takeDamage(turret_damage);
                    enemy.applyKnockback(Direction.normalized * turret_knockback);
                }
            }
            //// ----------- TBD ( A turret vai atirar no Tumor ou não? )
            //if (hit2D.collider.tag == "Tumor")
            //{
            //    TumorLife tumor = hit2D.collider.gameObject.GetComponent<TumorLife>();
            //    if (tumor != null)
            //    {
            //        tumor.damageTumor(turret_damage);
            //    }
            //}
        }
    }

    void SpawnShot(Vector3 end_position)
    {
        bullet_start_point.GetComponent<Laser>().SpawnEffects(end_position);
    }

    private void LookToTarget(GameObject t)
    {
        Vector2 direction = new Vector2(transform.position.x - t.transform.position.x,
                                        transform.position.y - t.transform.position.y);

        TurretHead.transform.up = direction;
    }


    private void UpdatePossibleTargets()
    {
        for (int i = 0; i < PossibleTargets.Count; i++)
        {
            if (PossibleTargets[i] == null)
            {
                PossibleTargets.RemoveAt(i);
            }
        }
    }
    
    
    private GameObject GetClosestTarget()
    {
        GameObject closest = null;
        float closestDistance = 999;
        for (int i = 0; i < PossibleTargets.Count; i++)
        {
            if (Vector2.Distance(PossibleTargets[i].gameObject.transform.position, gameObject.transform.position) < closestDistance)
            {
                closest = PossibleTargets[i];
                closestDistance = Vector2.Distance(PossibleTargets[i].gameObject.transform.position, gameObject.transform.position);
            }
        }
        return closest;
    }

    private void ApplyDamage(float damage)
    {
        bProperties.ApplyDamage(damage);
    }
}
