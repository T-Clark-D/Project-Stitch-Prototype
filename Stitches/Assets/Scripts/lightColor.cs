using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://stackoverflow.com/questions/56735003/unity-script-how-to-gradually-change-a-color-of-an-object-using-color-lerp
public class lightColor : MonoBehaviour
{
    public Light myLight;

    // Color Variables
    public float colorSpeed = 0.5f;
    private float tick = 0;

    [SerializeField] private Vector3 m_centralCameraPoint;
    [SerializeField] private Transform m_playerTransform;
    

    private Color m_oldColor;
    private Color m_newColor;

    void Start()
    {
        myLight = GetComponent<Light>();
        m_newColor = myLight.color;
        SetPoint(new Vector3(570, 740, -35));
    }

    void Update()
    {
        //FacePlayer();
        // Change color when player enters trigger event.
        tick += Time.deltaTime * colorSpeed;
        myLight.color = Color.Lerp(m_oldColor, m_newColor, tick);
    }

    // Switch the start and end colors on exit event.
    public void SwitchColor(Color newColor)
    {
        m_oldColor = myLight.color;
        m_newColor = newColor;
        tick = 0;
    }

    public void SetPoint(Vector3 newPoint)
    {
        m_centralCameraPoint = newPoint;
    }

    public void FacePlayer()
    {
        Vector3 faceDirection = m_playerTransform.position - m_centralCameraPoint;
        transform.rotation = Quaternion.LookRotation(faceDirection, Vector3.up);
    }

}
