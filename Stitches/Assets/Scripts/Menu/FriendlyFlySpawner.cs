using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyFlySpawner : MonoBehaviour
{
    public GameObject friendlyFly;
    private GameObject[] friendlyFlyArray = new GameObject[3];
    public Transform[] FriendlyFlyPositions;

    public float m_timeToRespawn = 3f;
    private bool[] m_timerStateArray;
    private float[] m_timeElapsedArray;

    // Start is called before the first frame update
    void Start()
    {
        friendlyFlyArray[0] = Instantiate(friendlyFly, FriendlyFlyPositions[0].position, Quaternion.identity);
        friendlyFlyArray[1] = Instantiate(friendlyFly, FriendlyFlyPositions[1].position, Quaternion.identity);
        friendlyFlyArray[2] = Instantiate(friendlyFly, FriendlyFlyPositions[2].position, Quaternion.identity);

        m_timeElapsedArray = new float[friendlyFlyArray.Length];
        m_timerStateArray = new bool[friendlyFlyArray.Length];

        for (int i = 0; i < friendlyFlyArray.Length; i++)
        {
            m_timeElapsedArray[i] = 0f;
            m_timerStateArray[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < friendlyFlyArray.Length; i++)
        {
            if(m_timerStateArray[i])
            {
                m_timeElapsedArray[i] += Time.deltaTime;

                if (m_timeElapsedArray[i] >= m_timeToRespawn)
                {
                    friendlyFlyArray[i] = Instantiate(friendlyFly, FriendlyFlyPositions[i].position, Quaternion.identity);

                    m_timeElapsedArray[i] = 0f;
                    m_timerStateArray[i] = false;
                }
            }
            else
            {
                if(friendlyFlyArray[i] == null)
                {
                    m_timerStateArray[i] = true;
                }
            }
        }
    }
}
