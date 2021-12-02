using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerEffects : MonoBehaviour
{
    private SpriteRenderer sp;
    private Material mat;
    [SerializeField] float poweredDuration;
    [SerializeField] float tookDamageDuration;

    private float poweredCounter = 0;
    private float hitCounter = 0;
    public bool powered;
    public bool tookDamage;

    // Start is called before the first frame update
    void Start()
    {
        sp = gameObject.GetComponentInChildren<SpriteRenderer>();
        mat = sp.material;
        mat.SetFloat("_Glow", 0.0f);
        mat.SetFloat("_HitEffectBlend", 0f);

    }

    // Update is called once per frame
    void Update()
    {
        if (powered)
        {
            isPowered();
        }
 
        if(tookDamage)
        {
            isHit();
        }
    }

    private void isHit()
    {
        hitCounter += Time.deltaTime;
        if(hitCounter < tookDamageDuration)
        {
            mat.SetFloat("_HitEffectBlend", 0.5f);
        }
        else
        {
            isNotHit();
            hitCounter = 0;
        }
        


    }

    private void isNotHit()
    {
        mat.SetFloat("_HitEffectBlend", 0f);
        tookDamage = false;
    }

    private void isPowered()
    {
        poweredCounter += Time.deltaTime;
        if (poweredCounter < poweredDuration)
        {
            mat.SetFloat("_Glow", 2f);
        }
        else
        {
            isNotPowered();
            poweredCounter = 0;

        }


    }

    private void isNotPowered()
    {
        mat.SetFloat("_Glow", 0f);
        powered = false;
    }
}
