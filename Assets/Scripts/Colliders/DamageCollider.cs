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

    [SerializeField]
    private float _damageInvincibleTime;

    public float DamageInvincibleTime
    {
        get {return _damageInvincibleTime;}
        set {_damageInvincibleTime = value;}
    }

    [SerializeField]
    private float _dashInvincibleTime;

    public float DashInvincibleTime
    {
        get {return _dashInvincibleTime;}
        set {_dashInvincibleTime = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        InInvincible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(InInvincible) {return;}

        if(_player.IsDead) {return;}

        EnemyBullet enemyBullet = collider.gameObject.GetComponent<EnemyBullet>();

        _player.TakeDamage(enemyBullet.DamageValue);

        StartCoroutine(StartInvincible(DamageInvincibleTime));
    }

    public void BeInvincibleWithDash()
    {
        StartCoroutine(StartInvincible(DashInvincibleTime));
    }

    private IEnumerator StartInvincible(float time)
    {
        InInvincible = true;

        yield return new WaitForSeconds(time);

        InInvincible = false;
    }
}
