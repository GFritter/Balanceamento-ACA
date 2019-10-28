using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootEng : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Transform bullet_start_point;

    public LayerMask layerMask;

    public float fireRate = 0.2f;
    public float bullet_Damage = 25;
    public float bullet_Knockback = 150;

    public AimObjectSpread aimObjectSpread;

    public EngineerConstruction eConstruction;
    
    private float nextFire;

    private PhotonView pView;
    
    void Start()
    {
        pView = this.GetComponent<PhotonView>();
        eConstruction = this.GetComponent<EngineerConstruction>();
    }
    
    void Update()
    {
        if (pView.isMine)
        {
            HandleInputs();
        }
    }

    [PunRPC]
    public void Shoot(Vector3 bullet_end_point)
    {
        RaycastHit2D hit2D;
        Vector2 Direction = bullet_end_point - bullet_start_point.position;
        hit2D = Physics2D.Raycast(bullet_start_point.position, Direction, 1000f, layerMask);
        
        SpawnShot(hit2D.point);
        
        if (PhotonNetwork.isMasterClient)
        {
            if ( hit2D.collider.tag == "Enemy" )
            {
                EnemyBehaviour enemy = hit2D.collider.gameObject.GetComponent<EnemyBehaviour>();
                if (enemy != null)
                {
                    enemy.takeDamage(bullet_Damage);
                    enemy.applyKnockback(Direction.normalized * bullet_Knockback);
                }
            }
            if (hit2D.collider.tag == "Tumor")
            {
                TumorLife tumor = hit2D.collider.gameObject.GetComponent<TumorLife>();
                if (tumor != null)
                {
                    tumor.damageTumor(bullet_Damage);
                }
            }
        }
    }

    void HandleInputs()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFire && !eConstruction.building)
        {
            Vector3 random_hit_point;
            nextFire = Time.time + fireRate;
            random_hit_point = aimObjectSpread.GetSpreadPoint();
            pView.RPC("Shoot", PhotonTargets.All, random_hit_point);
        }
    }

    void SpawnShot(Vector3 end_position)
    {
        bullet_start_point.GetComponent<Laser>().SpawnEffects(end_position);
    }

}
