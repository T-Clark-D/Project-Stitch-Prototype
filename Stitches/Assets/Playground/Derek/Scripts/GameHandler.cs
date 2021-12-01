using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] int mHP;
    [SerializeField] float mInvulTime;
    float mTime;
    bool mInvul = false;
    Vector3 respawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(mInvul)
        {
            mTime += Time.deltaTime;
            if(mTime >= mInvulTime)
            {
                mInvul = false;
            }
        }
    }
    
    public void takeDamage(int dmg)
    {
        if(!mInvul)
        {
            mHP -= dmg;
            mInvul = true;
        }
    }

    public void setRespawnPoint(Vector3 worldPoint)
    {
        respawnPoint = worldPoint;
    }
}
