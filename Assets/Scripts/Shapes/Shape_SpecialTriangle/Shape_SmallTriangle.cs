using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape_SmallTriangle : MonoBehaviour
{
    private Player _player;
    
    private SoundManager _soundManager;


    [SerializeField]
    private int _primaryAttackDamage;

    [SerializeField]
    private PrimaryTriangleSubBullet _primaryTriangleSubBullet;

    private bool _isRight;

    public bool IsRight
    {
        get {return _isRight;}
        set {_isRight = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        // IsRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(!IsRight)
        {
            _player.SmallLeftTriangle = null;
        }
        else
        {
            _player.SmallRightTriangle = null;
        }
        
        _PlaySound("Damage");
        Destroy(this.gameObject);
    }

    public void PrimaryAttack()
    {
        PlayerBullet triangleSubBullet = Instantiate(_primaryTriangleSubBullet, this.transform.position, Quaternion.identity);
        triangleSubBullet.DamageValue = _player.MyShape.PrimaryAttackDamage;
        Debug.Log($"Shape_SmallTriangle.PrimaryAttack {_primaryAttackDamage/2}");
    }

    private void _PlaySound(string name)
    {
        if(_soundManager == null)
        {
            _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        }

        _soundManager.PlaySound(name);
    }
}
