using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    private SpriteRenderer _spriteRenderer;

    private bool _inInvincible;

    public bool InInvincible
    {
        get {return _inInvincible;}
        set {_inInvincible = value;}
    }

    private int _invincibleNumber;

    public int InvincibleNumber
    {
        get {return _invincibleNumber;}
        set {_invincibleNumber = value;}
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
        InvincibleNumber = 0;

        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        Debug.Log("InInvincible");
        InInvincible = true;

        InvincibleNumber += 1;

        for(float i = 0; i <= time; i += 0.1f)
        {
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.5f);
            yield return new WaitForSeconds(0.05f);

            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f);
            yield return new WaitForSeconds(0.05f);
        }
        
        if(InvincibleNumber <= 1)
        {
            InInvincible = false;
            InvincibleNumber -= 1;
            Debug.Log("Not InInvincible");
        }
        else
        {
            InvincibleNumber -= 1;
            Debug.Log("InvincibleCoroutine ends but other is running");
        }
    }
}
