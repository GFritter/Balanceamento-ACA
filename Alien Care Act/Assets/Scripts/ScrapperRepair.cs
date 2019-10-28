using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapperRepair : MonoBehaviour
{
    private Camera mainCamera;
    public PhotonView pView;

    public LayerMask layerMask;

    public int repairAmount = 3;
    public float repairAmount_buff = 1;
    
    public GameObject aim_object;
    public GameObject repair_object;

    bool repairing = false;

    void Start()
    {
        mainCamera = transform.Find("Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
    }

    void CheckInputs()
    {
        if (pView.isMine)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                repairing = !repairing;
                UpdateMouseIcon(repairing);
            }
            if (Input.GetMouseButton(0) && repairing)
            {
                ScreenMouseRay();
            }
            UpdateRepairIconPosition();
        }
    }

    void UpdateRepairIconPosition()
    {
        repair_object.transform.position = aim_object.transform.position;
    }

    void UpdateMouseIcon(bool status)
    {
        if (status)
        {
            aim_object.SetActive(false);
            repair_object.SetActive(true);
        }
        else
        {
            aim_object.SetActive(true);
            repair_object.SetActive(false);
        }
    }

    public bool GetRepairStatus()
    {
        return repairing;
    }

    void BuffRepairAmount(float buff)
    {
        repairAmount_buff += buff;
    }

    void Repair(GameObject construction)
    {
        float repair_value = repairAmount * repairAmount_buff;

        if (PhotonNetwork.isMasterClient)
        {
            construction.GetComponent<BuildingProperties>().Repair(repair_value);
        }
        else
        {
            construction.GetComponent<BuildingProperties>().CallRepair(repair_value);
        }
        
    }

    void ScreenMouseRay()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5f;
        
        Vector2 v = mainCamera.ScreenToWorldPoint(mousePosition);

        if (Vector2.Distance(v, transform.position) < 5)
        {
            Collider2D[] col = Physics2D.OverlapPointAll(v, layerMask);

            if (col.Length > 0)
            {
                foreach (Collider2D c in col)
                {
                    if (c.tag == "Turret" || c.tag == "Barrier")
                    {
                        Repair(c.gameObject);
                    }
                }
            }
        }
    }
}
