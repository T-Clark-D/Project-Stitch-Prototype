using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    [SerializeField] GameObject mPlayer;
    private Animator m_playerAnim;
    PlayerController mPlayerController;
    [SerializeField] GameObject mGrappleHook;
    Vector3 respawnPoint;
    // For HP 
    [SerializeField] Image[] currHP;
    int currIndex = 2;
    [SerializeField] Image[] lossHP;
    int lossIndex = 5;
    HookController mHook;
    float mInvulTime;
    bool mInvul = false;
    float mTime;
    bool dead = false;
    public float invulDelay;


    // Start is called before the first frame update
    void Start()
    {
        HPSetup();
        mHook = mGrappleHook.GetComponent<HookController>();

        m_playerAnim = mPlayer.GetComponent<Animator>();

        mPlayerController = mPlayer.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mInvul)
        {
            mTime += Time.deltaTime;
            if (mTime >= invulDelay)
            {
                mTime = 0.0f;
                mInvul = false;
            }
        }
        if (dead)
        {
            mPlayer.SetActive(false);
            mHook.RetractHook();
            // can maybe instantiate some particle effects for death
            mTime += Time.deltaTime;
            if (mTime >= 2.0f)
            {
                Respawn();
                resetHP();
                currIndex = 2;
                lossIndex = 5;
                HPSetup();
                dead = false;
                mTime = 0.0f;
            }
        }
    }

    public void addHP()
    {
        if (currIndex < 7)
        {
            currIndex++;
            currHP[currIndex].enabled = true;
        }
        if (lossIndex > 0)
        {
            lossHP[lossIndex].enabled = false;
            lossIndex--;
            lossHP[lossIndex].enabled = true;
        }


        mPlayerController.PlayLifePickup();
    }

    IEnumerator DamageFlash()
    {
        m_playerAnim.SetTrigger("damageFlash");
        yield return new WaitForSeconds(0.33f);
        m_playerAnim.SetTrigger("damageFlash");
        yield return new WaitForSeconds(0.33f);
        m_playerAnim.SetTrigger("damageFlash");
    }

    public void takeDamage()
    {
        if (!mInvul)
        {
            mHook.RetractHook();
            if (currIndex <= 0)
            {
                currHP[currIndex].enabled = false;
            }
            else
            {
                currHP[currIndex].enabled = false;
                currIndex--;
            }
            if (lossIndex >= 8)
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

            StartCoroutine(DamageFlash());
            mPlayerController.PlayHurtSound();

            if(lossIndex >= 8)
            {
                if(!mPlayerController.GetLowLifeLoopState())
                {
                    mPlayerController.StartLowLifeLoop();
                }
            }
            else
            {
                if (mPlayerController.GetLowLifeLoopState())
                {
                    mPlayerController.StopLowLifeLoop();
                }
            }
        }
    }

    public void Respawn()
    {
        mPlayer.SetActive(true);
        // can maybe instantiate some particle effects for respawn
        mPlayer.transform.position = respawnPoint;
    }

    public void setRespawnPoint(Vector3 worldPoint)
    {
        respawnPoint = worldPoint;
    }

    public void HPSetup()
    {
        for (int i = 0; i < lossHP.Length; i++)
        {
            if (i == lossIndex)
            {
                continue;
            }
            lossHP[i].enabled = false;
        }
        for (int j = currHP.Length - 1; j > currIndex; j--)
        {
            currHP[j].enabled = false;
        }
    }

    public void resetHP()
    {
        for (int i = 0; i < lossHP.Length; i++)
        {
            lossHP[i].enabled = true;
        }
        for (int j = 0; j < currHP.Length; j++)
        {
            currHP[j].enabled = true;
        }
    }

    public void kill()
    {
        resetHP();
        for (int i = 0; i < lossHP.Length; i++)
        {
            lossHP[i].enabled = false;
        }
        for (int j = 0; j < currHP.Length; j++)
        {
            currHP[j].enabled = false;
        }
        dead = true;
    }
}
