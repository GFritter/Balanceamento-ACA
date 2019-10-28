using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private int enemy_count;
    public int max_enemies = 100;
    // Start is called before the first frame update
    void Start()
    {
        enemy_count = 0;
    }

    public bool CanAddEnemy()
    {
        if ((enemy_count + 1) < max_enemies)
        {
            enemy_count += 1;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void RemoveEnemy()
    {
        enemy_count -= 1;
    }
}
