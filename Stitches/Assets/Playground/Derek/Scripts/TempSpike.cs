using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSpike : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.name == "Player")
        {
            col.GetComponent<Derek_PC>().TakeDamage(3);
        }
    }
}
