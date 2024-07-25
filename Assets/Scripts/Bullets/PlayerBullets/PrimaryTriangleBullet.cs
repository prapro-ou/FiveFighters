using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryTriangleBullet : PlayerBullet
{
    private Player _player;

    [SerializeField]
    private Vector2 vec;

    private Vector3 player_pos;

    private Vector3 bullet_pos;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        player_pos = _player.transform.position;

        bullet_pos = this.gameObject.transform.position;

        rb = GetComponent<Rigidbody2D>();

        if(player_pos.x > bullet_pos.x)
        {
            rb.velocity = new Vector2(-vec.x, vec.y);
        }
        else
        {
            rb.velocity = new Vector2(vec.x, vec.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
