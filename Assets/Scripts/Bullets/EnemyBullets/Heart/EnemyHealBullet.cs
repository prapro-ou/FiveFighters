using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealBullet : MonoBehaviour
{
    private Player _player;

    [SerializeField]
    private int _healValue;

    public int HealValue
    {
        get {return _healValue;}
        set {_healValue = value;}
    }
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
        if (collider.gameObject.tag == "DamageCollider")
        {
            _player.HitPoint += HealValue;
            Debug.Log($"Heal PlayerHP {HealValue}");
        }
    }
}
