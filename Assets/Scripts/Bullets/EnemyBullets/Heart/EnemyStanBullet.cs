using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStanBullet : EnemySpeedChangeBullet
{
    private Player _player;

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
            _player.Speed *= 0.01f;
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
