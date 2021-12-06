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
       for(int i = 0; i <3 ; i++){
            if(friendlyFlyArray[i] == null)
            {
                Wait(i);
            }
        }
    }
    IEnumerator Wait(int i)
    {
        friendlyFlyArray[i] = Instantiate(friendlyFly, FriendlyFlyPositions[i].position, Quaternion.identity);
        friendlyFlyArray[i].SetActive(false);
        yield return new WaitForSeconds(3);
        friendlyFlyArray[i].SetActive(true);
    }
}
