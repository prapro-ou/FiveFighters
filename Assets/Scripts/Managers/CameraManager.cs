using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private Transform _cameraTransform;

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

    public void MoveToPoint(Vector3 endPoint)
    {
        _cameraTransform.position = new Vector3(endPoint.x, endPoint.y, _cameraTransform.position.z);
    }

    public IEnumerator MoveToPointOnCurve(Vector3 endPoint)
    {
        Vector3 startPoint = _cameraTransform.position;

        for(float i = 0; i <= 1; i += Time.deltaTime)
        {
            Vector3 lerpVec3 = Vector3.Lerp(startPoint, endPoint, _curve.Evaluate(i));
            lerpVec3.z = _cameraTransform.position.z;
            _cameraTransform.position = lerpVec3;
            yield return null;
        }
    }

    public void SetSize(float size)
    {
        _camera.orthographicSize = size;
    }
    public IEnumerator SetSizeOnCurve(float endSize)
    {
        float startSize = _camera.orthographicSize;
        float lerpSize = 0f;

        for(float i = 0; i <= 1; i += Time.deltaTime)
        {
            lerpSize = Mathf.Lerp(startSize, endSize, _curve.Evaluate(i));
            _camera.orthographicSize = lerpSize;
            yield return null;
        }
    }
}
