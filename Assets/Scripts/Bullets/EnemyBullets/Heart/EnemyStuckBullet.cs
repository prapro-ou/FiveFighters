using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStuckBullet : MonoBehaviour
{
    private Player _player;

    [SerializeField]
    private int _stuckValue;

    private bool _isDamaged;

    private float _bulletTop;

    private float _bulletBottom;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _isDamaged = false;
        _bulletTop = this.transform.position.y + 1.1f;
        _bulletBottom = this.transform.position.y -1.8f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isDamaged){
            //ここでPlayerとこいつの座標からDamageを与える
            if ((Mathf.Abs(this.transform.position.x) < 1.3f) && (Mathf.Abs(_player.transform.position.x) < Mathf.Abs(this.transform.position.x)))
            {
                if ((_player.transform.position.y < _bulletTop) && (_player.transform.position.y > _bulletBottom))
                {
                    _player.TakeDamage(_stuckValue);
                    _isDamaged = true;
                    Debug.Log($"StuckDamage {_stuckValue}");
                }
            }
        }
    }
}