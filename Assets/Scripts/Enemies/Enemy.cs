using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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
            _hitPoint = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        HitPoint = MaxHitPoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerBullet bullet = collider.gameObject.GetComponent<PlayerBullet>();

        TakeDamage(bullet.DamageValue);

        Destroy(bullet.gameObject);
    }

    public void TakeDamage(int value)
    {
        HitPoint -= value;
        
        Debug.Log($"Enemy::TakeDamage HP: {HitPoint}(Damage:{HitPoint})");
    }
}
