using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCircleBullet : MonoBehaviour
{
    [SerializeField]
    private GameObject _specialCircleBullet;

    [SerializeField]
    private float _specialCircleBulletSpeed;

    [SerializeField]
    private GameObject _specialCircleExplode;

    [SerializeField]
    private float _explosionTime;

    private GameObject _specialCircleExplodeObject;

    private bool _isExplosion;

    // Start is called before the first frame update
    void Start()
    {
        _isExplosion = false;
    }

    // Update is called once per frame
    void Update()
    {
        _MoveToCenter();
    }

    private void _MoveToCenter()
    {
        Vector3 screenCenter = new Vector3(0.0f, 0.0f, 0.0f);

        transform.position = Vector3.MoveTowards(transform.position, screenCenter, _specialCircleBulletSpeed*Time.deltaTime);
        
        if (Vector3.Distance(transform.position, screenCenter) < 0.1f && !_isExplosion)
        {
            _specialCircleExplodeObject = Instantiate(_specialCircleExplode.gameObject, transform.position, Quaternion.identity);
            _isExplosion = true;
            Destroy(this.gameObject);
            Destroy(_specialCircleExplodeObject, _explosionTime);
        }
    }
}
