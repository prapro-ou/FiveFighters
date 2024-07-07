using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    private bool _inInvincible;

    public bool InInvincible
    {
        get {return _inInvincible;}
        set {_inInvincible = value;}
    }

    private float _invincibleTime;

    public float InvincibleTime
    {
        get {return _invincibleTime;}
        set {_invincibleTime = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(InInvincible) {return;}

        EnemyBullet enemyBullet = collider.gameObject.GetComponent<EnemyBullet>();

        _player.TakeDamage(enemyBullet.DamageValue);

        StartCoroutine("StartInvincible", InvincibleTime);
    }

    private IEnumerator StartInvincible(float time)
    {
        InInvincible = true;

        yield return new WaitForSeconds(time);

        InInvincible = false;
    }
}
