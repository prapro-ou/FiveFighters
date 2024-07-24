using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _curve;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToPointImmediately(Vector3 endPoint)
    {
        transform.position = new Vector3(endPoint.x, endPoint.y, transform.position.z);
    }

    public void MoveToPoint(Vector3 endPoint)
    {
        Vector3 startPoint = transform.position;

        for(float i = 0; i <= 1; i += Time.deltaTime)
        {
            Vector3 lerpVec3 = Vector3.Lerp(startPoint, endPoint, i);
            lerpVec3.z = transform.position.z;
            transform.position = lerpVec3;
        }
    }
}
