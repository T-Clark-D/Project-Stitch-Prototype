using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // TEMP


// NOTES: Can refer to Megaman.cs and LifeMeter.cs when it comes to implementing with some assets

public class TempHP : MonoBehaviour
{
    [SerializeField] int mFullHP;
    int mHP; 
    public Text healthText;
    public bool mPlayerDead;
    Derek_PC mPlayer;

    // Start is called before the first frame update
    void Start()
    {
        mPlayer = GameObject.Find("Player").GetComponent<Derek_PC>();
        mHP = mFullHP;
        mPlayerDead = false;
    }

    void Update() // Temp
    {
        healthText.text = "HP: " + mHP.ToString();
    }

    public void AddHealth(int x)
    {
        if(mHP < mFullHP)
        {
            mHP++;
        }
    }

    public void DeductHealth(int x)
    {
        if(mHP <= 0)
        {
            mPlayer.Die();
            mPlayerDead = true;
        }
        else
        {
            mHP -= x;
        }
    }
}
