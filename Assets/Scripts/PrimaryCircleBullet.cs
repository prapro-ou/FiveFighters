using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryCircleBullet : PlayerBullet
{
    private Player _player;

    [SerializeField]
    private Vector2 vec;

    private PlayerBullet _playerbullet;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(vec.x, vec.y);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
