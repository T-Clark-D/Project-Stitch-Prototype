using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.name == "Player")
        {
            col.GetComponent<Derek_PC>().TakeDamage(3);
        }
    }
}
