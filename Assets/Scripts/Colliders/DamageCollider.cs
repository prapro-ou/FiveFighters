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

    private int _touchCount;

    public int TouchCount
    {
        get {return _touchCount;}
        set {_touchCount = value;}
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
    private float _damageInvincibleTimeDecreaseRatio;

    private float _currentDamageInvincibleTime;
    
    public float CurrentDamageInvincibleTime
    {
        get {return _currentDamageInvincibleTime;}
        set {_currentDamageInvincibleTime = value;}
    }

    [SerializeField]
    private float _skillInvincibleTime;

    public float SkillInvincibleTime
    {
        get {return _skillInvincibleTime;}
        set {_skillInvincibleTime = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        InInvincible = false;
        InvincibleNumber = 0;

        CurrentDamageInvincibleTime = DamageInvincibleTime;

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        TouchCount += 1;

        if(InInvincible) {return;}

        if(_player.IsDead) {return;}

        EnemyBullet enemyBullet = collider.gameObject.GetComponent<EnemyBullet>();

        _player.TakeDamage(enemyBullet.DamageValue);

        StartCoroutine(StartInvincible(CurrentDamageInvincibleTime));
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        TouchCount -= 1;
    }

    public void BeInvincibleWithSkill()
    {
        StartCoroutine(StartInvincible(SkillInvincibleTime));
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
            
            if(TouchCount <= 0)
            {
                _ResetDamageInvincibleTime();
            }
            else
            {
                CurrentDamageInvincibleTime *= _damageInvincibleTimeDecreaseRatio;
                Debug.Log($"CurrentDamageInvincibleTime is decreased: {CurrentDamageInvincibleTime}");
            }
        }
        else
        {
            InvincibleNumber -= 1;
            Debug.Log("InvincibleCoroutine ends but other is running");
        }
    }

    private void _ResetDamageInvincibleTime()
    {
        CurrentDamageInvincibleTime = DamageInvincibleTime;
        Debug.Log($"Reset CurrentDamageInvincibleTime: {CurrentDamageInvincibleTime}");
    }
}
