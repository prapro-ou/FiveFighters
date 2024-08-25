using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape_Circle : PlayerShape
{
    private Player _player;

    private GameObject _circleDestroyField;

    private SoundManager _soundManager;
    
    private DamageCollider _damageCollider;

    [SerializeField]
    private PrimaryCircleBullet _primaryCircleBullet;

    [SerializeField]
    private GameObject _specialCircleBullet;

    // private PlayerBullet _playerbullet;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PrimaryAttack()
    {
        Vector3 vec = _player.transform.position;
        
        PlayerBullet bullet = Instantiate(_primaryCircleBullet, vec, Quaternion.identity);

        bullet.DamageValue = PrimaryAttackDamage;
    }

    public override void SpecialSkill()
    {
        _PlaySound("ShootSpecialCircleBullet");

        Instantiate(_specialCircleBullet.gameObject, _player.transform.position, Quaternion.identity);
        Debug.Log($"SpecialSkill {name}");
    }

    public override void ShiftSkill()
    {
        _PlaySound("Explosion2");

        _circleDestroyField = Instantiate(_destroyField.gameObject, _player.transform.position, Quaternion.identity);

        if(_damageCollider == null)
        {
            _damageCollider = GameObject.Find("DamageCollider").GetComponent<DamageCollider>();
        }

        _damageCollider.BeInvincibleWithSkill();

        Destroy(_circleDestroyField, 1.5f);
        Debug.Log($"ShiftSkill {name}");
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
