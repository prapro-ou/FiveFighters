using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimarySquareBullet : PlayerBullet
{
    private Player _player;

    private Enemy _enemy;

    private Vector2 vec;

    private Vector3 enemy_pos;

    private Vector3 player_pos;

    private PlayerBullet _playerbullet;

    private Rigidbody2D rb;

    [SerializeField]
    private GameObject _squareEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();

        player_pos = _player.transform.position;

        enemy_pos = _enemy.transform.position;

        rb = GetComponent<Rigidbody2D>();

        rb.velocity = new Vector2(enemy_pos.x - player_pos.x, enemy_pos.y - player_pos.y);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public override void DestroyWithParticle()
    {
        Instantiate(_squareEffectPrefab, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
