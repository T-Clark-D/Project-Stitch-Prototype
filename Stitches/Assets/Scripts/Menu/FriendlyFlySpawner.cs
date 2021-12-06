using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyFlySpawner : MonoBehaviour
{
    public GameObject friendlyFly;
    private GameObject[] friendlyFlyArray = new GameObject[3];
    public Transform[] FriendlyFlyPositions;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(friendlyFlyArray.Length);
        friendlyFlyArray[0] = Instantiate(friendlyFly, FriendlyFlyPositions[0].position, Quaternion.identity);
        Debug.Log("we inti");
        friendlyFlyArray[1] = Instantiate(friendlyFly, FriendlyFlyPositions[1].position, Quaternion.identity);
        friendlyFlyArray[2] = Instantiate(friendlyFly, FriendlyFlyPositions[2].position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
