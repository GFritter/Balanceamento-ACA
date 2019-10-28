using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootScr : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Transform[] bullet_start_point;
    
    public LayerMask layerMask;

    public AimObjectSpread aimObjectSpread;

    private PhotonView pView;

    public float fireRate = 0.9f;
    public float bullet_Damage = 5;
    public float bullet_Knockback = 800;
    private float nextFire;
    
    void Start()
    {
        pView = this.GetComponent<PhotonView>();
    }
    
    void Update()
    {
        if (pView.isMine)
        {
            HandleInputs();
        }
    }

    [PunRPC]
    public void Shoot(Vector3 bullet_end_point, int barrel)
    {
        RaycastHit2D hit2D;
        Vector2 Direction = bullet_end_point - bullet_start_point[barrel].position;
        hit2D = Physics2D.Raycast(bullet_start_point[barrel].position, Direction, 1000f, layerMask);

        SpawnShot(hit2D.point, barrel);

        if (PhotonNetwork.isMasterClient)
        {
            if (hit2D.collider.tag == "Enemy")
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
        if (Input.GetMouseButton(0) && Time.time > nextFire)
        {
            if (GetComponent<ScrapperRepair>().GetRepairStatus() == false)
            {
                Vector3[] random_hit_point = new Vector3[5];
                nextFire = Time.time + fireRate;
                for (int i = 0; i < 5; i++)
                {
                    random_hit_point[i] = aimObjectSpread.GetSpreadPoint();
                    pView.RPC("Shoot", PhotonTargets.All, random_hit_point[i], i);
                }
            }
        }
    }
    
    void SpawnShot(Vector3 end_position, int barrel)
    {
        bullet_start_point[barrel].GetComponent<Laser>().SpawnEffects(end_position);
    }
}