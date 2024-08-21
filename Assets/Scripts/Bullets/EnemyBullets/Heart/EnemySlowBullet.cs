using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlowBullet : EnemySpeedChangeBullet
{
    private Player _player;

    [SerializeField]
    private float _slowValue;

    [SerializeField]
    private GameObject _heartSlowEffect;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.gameObject.tag == "DamageCollider") && !IsSpeedChanged)
        {
            _player.Speed *= _slowValue;
            GameObject effect = Instantiate(_heartSlowEffect, _player.transform.position, Quaternion.Euler(180, 0, 0), _player.transform);
            IsSpeedChanged = true;
            Destroy(effect, 1.5f);
            Invoke("ReturnOriginalSpeed", 3f);
        }
    }

    public void ReturnOriginalSpeed()
    {
        _player.Speed = _originalSpeed;
        IsSpeedChanged = false;
        Debug.Log($"Return original speed");
    }
}