using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Hexagon : Enemy
{
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.CurrentEnemy = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
