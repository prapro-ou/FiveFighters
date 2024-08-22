using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkHexagonBullet : EnemyBullet
{
    [SerializeField]
    private float _shrinkRatio;

    [SerializeField]
    private float _rotateSpeed;

    private bool _isRight;
    
    public bool IsRight
    {
        get {return _isRight;}
        set {_isRight = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(_Shrink());
        StartCoroutine(_Rotate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator _Shrink()
    {
        // Debug.Log("Shrink");

        for(float i = 3f; i > 0; i -= (Time.deltaTime * _shrinkRatio))
        {
            transform.localScale = new Vector3(i, i, 1);
            yield return null;
        }

        Destroy(this.gameObject);
    }

    private IEnumerator _Rotate()
    {
        if(IsRight)
        {
            for(;;)
            {
                transform.Rotate(0, 0, Time.deltaTime * _rotateSpeed);
                yield return null;
            }
        }
        else
        {
            for(;;)
            {
                transform.Rotate(0, 0, -Time.deltaTime * _rotateSpeed);
                yield return null;
            }
        }
    }
}
