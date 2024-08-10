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

    [SerializeField]
    private AnimationCurve _vibrateCurveX;

    [SerializeField]
    private AnimationCurve _vibrateCurveY;

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

    public IEnumerator MoveToPointOnCurve(Vector3 endPoint, float duration = 1f)
    {
        Vector3 startPoint = _cameraTransform.position;

        for(float i = 0; i <= duration; i += Time.deltaTime)
        {
            Vector3 lerpVec3 = Vector3.Lerp(startPoint, endPoint, _curve.Evaluate(i / duration));
            lerpVec3.z = _cameraTransform.position.z;
            _cameraTransform.position = lerpVec3;
            yield return null;
        }
    }

    public void SetSize(float size)
    {
        _camera.orthographicSize = size;
    }

    public IEnumerator SetSizeOnCurve(float endSize, float duration = 1f)
    {
        float startSize = _camera.orthographicSize;
        float lerpSize = 0f;

        for(float i = 0; i <= duration; i += Time.deltaTime)
        {
            lerpSize = Mathf.Lerp(startSize, endSize, _curve.Evaluate(i / duration));
            _camera.orthographicSize = lerpSize;
            yield return null;
        }
    }

    public IEnumerator Vibrate(float duration, float power)
    {
        Vector3 startPosition = _camera.transform.position;

        Vector3 nextPosition = startPosition;

        for(float i = 0; i <= duration; i += Time.deltaTime)
        {
            nextPosition.x = startPosition.x + (_vibrateCurveX.Evaluate(i / duration) * power);
            nextPosition.y = startPosition.y + (_vibrateCurveY.Evaluate(i / duration) * power);
            _camera.transform.position = nextPosition;
            yield return null;
        }

        _camera.transform.position = startPosition;
    }

    public IEnumerator RotateOnCurve(float angle, float duration = 1f)
    {
        Quaternion startRotation = _camera.transform.rotation;
        Quaternion endRotation = _camera.transform.rotation * Quaternion.Euler(0, 0, angle);
        Quaternion lerpRotation;

        for(float i = 0; i <= duration; i += Time.deltaTime)
        {
            lerpRotation = Quaternion.Lerp(startRotation, endRotation, _curve.Evaluate(i / duration));
            _camera.transform.rotation = lerpRotation;
            yield return null;
        }
    }
}
