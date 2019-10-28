using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBehaviour : MonoBehaviour
{
    LayerMask ActiveLayer;
    LayerMask InactiveLayer;

    private void Start()
    {
        ActiveLayer     = LayerMask.NameToLayer("Barrier");
        InactiveLayer   = LayerMask.NameToLayer("InactiveBarrier");
    }

    private void OnEnable()
    {
        gameObject.layer = ActiveLayer;
    }

    private void OnDisable()
    {
        gameObject.layer = InactiveLayer;
    }
}
