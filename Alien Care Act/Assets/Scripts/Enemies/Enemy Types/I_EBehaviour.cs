using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Behaviours/Basic")]
public class I_EBehaviour : ScriptableObject
{
    public EnemyBehaviour owner;

    public virtual void setOwner(EnemyBehaviour e)
    {
        owner = e;
    }

    public virtual void behaviour_Attack()
    {

    }


    public virtual List<GameObject> behaviour_SetTargetList(List<GameObject> PossibleTargets)
    {

        return PossibleTargets;
    }

    public virtual GameObject behaviour_GetPreferredTarget(List<GameObject> PossibleTargets)
    {
        return PossibleTargets[0];
    }



}
