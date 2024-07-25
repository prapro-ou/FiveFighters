using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReduceGrazeCountBullet : EnemyBullet
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

    void OnTriggerEnter2D(Collider2D colider)
    {
        _player.PrimaryGrazeCount -= 20;
    }
}
