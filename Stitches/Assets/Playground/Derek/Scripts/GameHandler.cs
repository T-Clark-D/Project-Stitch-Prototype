using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    [SerializeField] float mInvulTime;
    [SerializeField] GameObject mPlayer;
    Vector3 respawnPoint;
    // For HP 
    [SerializeField] Image[] currHP;
    int currIndex = 2;
    [SerializeField] Image[] lossHP;
    int lossIndex = 5;

    float mTime;
    bool mInvul = false;
    bool dead = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(currIndex);
        Debug.Log(lossIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if(mInvul)
        {
            mTime += Time.deltaTime;
            if(mTime >= mInvulTime)
            {
                mTime = 0.0f;
                mInvul = false;
            }
        }

        if(dead)
        {
            mPlayer.SetActive(false);
            // can maybe instantiate some particle effects for death
            mTime += Time.deltaTime;
            if(mTime >= 2.0f)
            {
                Respawn();
                currIndex = 2;
                lossIndex = 5;
                for(int i = 0; i <= currIndex; i++)
                {
                    currHP[currIndex].enabled = true;
                }
                lossHP[lossIndex].enabled = true;
                dead = false;
            }
        }
    }
    
    public void addHP()
    {
        if(currIndex < 7)
        {
            currIndex++;
            currHP[currIndex].enabled = true;
            Debug.Log("Added Button " + currIndex);

        }
        if(lossIndex > 0)
        {
            lossHP[lossIndex].enabled = false;
            Debug.Log("Removed lossButton at " + lossIndex);
            lossIndex--;
            lossHP[lossIndex].enabled = true;
            Debug.Log("Added lossButton at " + lossIndex);
        }

    }

    public void takeDamage()
    {
        if(!mInvul)
        {
            if(currIndex <=0)
            {
                currHP[currIndex].enabled = false;
            }
            else
            {
                currHP[currIndex].enabled = false;
                currIndex--;
            }
            if(lossIndex >= 8)
            {
                lossHP[lossIndex].enabled = false;
                dead = true;
            }
            else
            {
                lossHP[lossIndex].enabled = false;
                lossIndex++;
                lossHP[lossIndex].enabled = true;
            }
            mInvul = true;
        }
    }

    public void Respawn()
    {
        mPlayer.SetActive(true);
        // can maybe instantiate some particle effects for respawn
        mPlayer.transform.position = respawnPoint + transform.forward * 2;
    }

    public void setRespawnPoint(Vector3 worldPoint)
    {
        Debug.Log("Respawn Set");
        respawnPoint = worldPoint;
    }
}
