using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryCircleBullet : PlayerBullet
{
    private Player _player;

    [SerializeField]
    private Vector2 vec;

    private Rigidbody2D rb;

    [SerializeField]
    private GameObject _circleEffectPrefab;

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

    public override void DestroyWithParticle()
    {
        Instantiate(_circleEffectPrefab, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
