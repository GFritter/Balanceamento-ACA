using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Behaviours/Vanilla")]
public class Vanilla : I_EBehaviour
{

    public override void behaviour_Attack()
    {
        base.behaviour_Attack();
    }


    public override List<GameObject> behaviour_SetTargetList(List<GameObject> PossibleTargets)
    {

        FilterActiveTargets(PossibleTargets);
        return PossibleTargets;

    }

    public override GameObject behaviour_GetPreferredTarget(List<GameObject> PossibleTargets)
    {
        FilterActiveTargets(PossibleTargets);
        GameObject closest = null;
        float closestDistance = 999;
        for (int i = 0; i < PossibleTargets.Count; i++)
        {
            if (Vector2.Distance(PossibleTargets[i].gameObject.transform.position, owner.gameObject.transform.position) < closestDistance)
            {
                closest = PossibleTargets[i];
                closestDistance = Vector2.Distance(PossibleTargets[i].gameObject.transform.position, owner.gameObject.transform.position);
            }
        }
        return closest;
    }

   

    public void FilterActiveTargets(List<GameObject> PossibleTargets)
    {
        for (int i = 0; i < PossibleTargets.Count; i++)
        {
            switch (PossibleTargets[i].tag)
            {
                case "Player":
                    if (!PossibleTargets[i].GetComponent<PlayerLife>().enabled)
                    {
                        PossibleTargets.RemoveAt(i);
                    }
                    break;
                case "Nexus":
                    if (!PossibleTargets[i].GetComponent<NexusManager>().enabled)
                    {
                        PossibleTargets.RemoveAt(i);
                    }
                    break;
                case "Turret":
                    if (!PossibleTargets[i].GetComponent<BuildingProperties>().enabled)
                    {
                        PossibleTargets.RemoveAt(i);
                    }
                    break;
                case "Barrier":
                    if (!PossibleTargets[i].GetComponent<BuildingProperties>().enabled)
                    {
                        PossibleTargets.RemoveAt(i);
                    }
                    break;
            }
        }
    }

}
