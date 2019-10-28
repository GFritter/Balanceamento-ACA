using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tumor : MonoBehaviour
{
    float SpawnRate = 2.0f;

    float SpawnTime;

    public List<GameObject> PossibleSpawns;

    private EnemyManager enemyMgr;

    private void Start()
    {
        enemyMgr = GameObject.Find("photonDontDestroy").GetComponent<EnemyManager>();
    }

    void Update()
    {
        if (Time.time > SpawnTime && enemyMgr.CanAddEnemy())
        {   
            //SpawnRate por contagem total de tumores
            SpawnRate = (transform.parent.childCount * 0.4f);
            SpawnTime = Time.time + SpawnRate;
            //Spawn a random possible enemy
            SpawnEnemy(PossibleSpawns[Random.Range(0, PossibleSpawns.Count - 1)]);
        }
    }

    void SpawnEnemy(GameObject enemy)
    {
        if(PhotonNetwork.isMasterClient)
            PhotonNetwork.Instantiate(enemy.name, transform.position, Quaternion.identity, 0);
    }
}
