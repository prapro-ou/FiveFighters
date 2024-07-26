using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryTriangleSubBullet : PlayerBullet
{
    private Player _player;
    private Rigidbody2D rb;

    [SerializeField]
    private float _subBulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0.0f, _subBulletSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
