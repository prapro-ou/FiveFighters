using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHexagonBullet : EnemyBullet
{
    [SerializeField]
    private AnimationCurve _movingCurve;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator MoveToPointOnCurve(Vector3 endPoint, float duration = 1f)
    {
        Vector3 startPoint = transform.position;

        for(float i = 0; i <= duration; i += Time.deltaTime)
        {
            Vector3 lerpVec3 = Vector3.Lerp(startPoint, endPoint, _movingCurve.Evaluate(i / duration));
            lerpVec3.z = transform.position.z;
            transform.position = lerpVec3;
            yield return null;
        }
    }
}
