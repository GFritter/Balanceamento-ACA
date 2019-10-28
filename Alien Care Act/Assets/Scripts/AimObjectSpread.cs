using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimObjectSpread : MonoBehaviour
{
    public float max_spread;
    public float min_spread;
    public float minimum_spread_distance;
    public float maximum_spread_distance;

    public Transform spreadUp;
    public Transform spreadDown;
    public Transform player;

    void Update()
    {

        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if ((Vector3.Distance(player.position, transform.position) < minimum_spread_distance))
        {
            spreadUp.position = transform.position + (transform.up * min_spread);
            spreadDown.position = transform.position + (transform.up * -min_spread);
        }

        else if ((Vector3.Distance(player.position, transform.position) > maximum_spread_distance))
        {
            spreadUp.position = transform.position + (transform.up * max_spread);
            spreadDown.position = transform.position + (transform.up * -max_spread);
        }
        else
        {

            float total_distance = maximum_spread_distance - minimum_spread_distance;
            float amount_in_range = (Vector3.Distance(player.position, transform.position) - minimum_spread_distance);
            float percentage_in_range = amount_in_range / total_distance;

            spreadUp.position = transform.position + (transform.up * (min_spread + (percentage_in_range * max_spread)));
            spreadDown.position = transform.position + (transform.up * (-min_spread + (percentage_in_range * -max_spread)));

        }

    }

    public Vector3 GetSpreadPoint()
    {
        float up_distance = Vector3.Distance(spreadUp.position, transform.position);
        float down_distance = -Vector3.Distance(spreadDown.position, transform.position);
        float amount_range = Random.Range(down_distance, up_distance);

        Vector3 point;

        if (amount_range < 0)
        {
            float random_percentage = amount_range / down_distance;
            point = transform.position + (transform.up * (random_percentage * down_distance));
        }
        else
        {
            float random_percentage = amount_range / up_distance;
            point = transform.position + (transform.up * (random_percentage * up_distance));
        }

        return point;
    }
}
