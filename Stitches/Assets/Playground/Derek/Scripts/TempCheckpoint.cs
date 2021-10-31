using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCheckpoint : MonoBehaviour
{
    SpriteRenderer mSpriteRenderer;
    [SerializeField] Transform mRespawnPoint;
    [SerializeField] TempHP health;
    [SerializeField] GameObject playerPrefab;

    void Start()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(health.mPlayerDead)
        {
            Respawn();
            health.mPlayerDead = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.name == "Player")
        {
            mSpriteRenderer.color = Color.red;
        }
    }

    void Respawn()
    {
        Instantiate(playerPrefab, mRespawnPoint.position, mRespawnPoint.rotation);
    }
}
