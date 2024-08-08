using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyField : MonoBehaviour
{
    private Player _player;
    
    [SerializeField]
    private int _grazeValue;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //触れてきた弾を破壊
    void OnTriggerEnter2D(Collider2D collider)
    {
        _player.PrimaryGrazeCount += _grazeValue;
        _player.SpecialGrazeCount += _grazeValue;
        Destroy(collider.gameObject);
    }
}
