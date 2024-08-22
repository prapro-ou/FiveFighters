using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFastBullet : EnemySpeedChangeBullet
{
    private Player _player;

    [SerializeField]
    private float _fastValue;

    [SerializeField]
    private GameObject _heartFastEffect;

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
            _player.CurrentSpeed *= _fastValue;
            GameObject effect = Instantiate(_heartFastEffect, _player.transform.position, Quaternion.identity, _player.transform);
            IsSpeedChanged = true;
            Destroy(effect, 1.5f);
            Invoke("ReturnOriginalSpeed", 3f);
        }
    }

    private void ReturnOriginalSpeed()
    {
        _player.CurrentSpeed = _player.DefaultSpeed;
        IsSpeedChanged = false;
        Debug.Log($"Return original speed");
    }
}