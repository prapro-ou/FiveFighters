using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private GameManager _gameManager;

    private Collider2D _collider;

    [SerializeField]
    private int _maxHitPoint;

    public int MaxHitPoint
    {
        get {return _maxHitPoint;}
        set
        {
            _maxHitPoint = value;
        }
    }

    private int _hitPoint;

    public int HitPoint
    {
        get {return _hitPoint;}
        set
        {
            _hitPoint = Mathf.Clamp(value, 0, MaxHitPoint);
            Debug.Log($"EnemyHP: {HitPoint}");

            if(_hitPoint <= 0)
            {
                StopAllCoroutines();
                _collider.enabled = false;
                _gameManager.ClearStage();
            }
        }
    }

    void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _collider = GetComponent<Collider2D>();

        HitPoint = MaxHitPoint;
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
        PlayerBullet bullet = collider.gameObject.GetComponent<PlayerBullet>();
        if(bullet != null)
        {
            TakeDamage(bullet.DamageValue);
            Destroy(bullet.gameObject);
        }
    }

    public void TakeDamage(int value)
    {
        HitPoint -= value;
    }

    public abstract void StartAttacking();

    public abstract IEnumerator StartSpawnAnimation();

    public abstract IEnumerator StartDeathAnimation();
}
