using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlowBullet : EnemySpeedChangeBullet
{
    private Player _player;

    [SerializeField]
    private float _slowValue;

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
            IsSpeedChanged = true;
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