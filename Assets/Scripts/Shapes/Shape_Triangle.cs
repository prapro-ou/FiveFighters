using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape_Triangle : PlayerShape
{
    private Player _player;
    private GameObject _triangleDestroyField;
    
    private SoundManager _soundManager;
    
    [SerializeField]
    private Shape_SmallTriangle _smallTriangle;
    
    private DamageCollider _damageCollider;

    // private Shape_SmallTriangle _smallRightTriangle;
    // private Shape_SmallTriangle _smallLeftTriangle;

    [SerializeField]
    private float _smallTrianglePosition;

    [SerializeField]
    private PrimaryTriangleBullet _primaryTriangleBullet;

    private PlayerBullet _playerbullet;

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
        
        PlayerBullet bullet = Instantiate(_primaryTriangleBullet, new Vector3(vec.x + 0.3f, vec.y, vec.z), Quaternion.identity);
        bullet.DamageValue = PrimaryAttackDamage;

        bullet = Instantiate(_primaryTriangleBullet, new Vector3(vec.x - 0.3f, vec.y, vec.z), Quaternion.identity);
        bullet.DamageValue = PrimaryAttackDamage;
    }

    public override void SpecialSkill()
    {
        _PlaySound("Spawn2");

        Vector3 rightPosition = new Vector3(_player.transform.position.x + _smallTrianglePosition, _player.transform.position.y + _smallTrianglePosition);
        Vector3 leftPosition  = new Vector3(_player.transform.position.x - _smallTrianglePosition, _player.transform.position.y + _smallTrianglePosition);

        //子機の本体を左右2つ生成
        if(_player.SmallRightTriangle == null)
        {
            Shape_SmallTriangle smallRightTriangle = Instantiate(_smallTriangle, rightPosition, Quaternion.identity, _player.transform);
            smallRightTriangle.IsRight = true;
            _player.SmallRightTriangle = smallRightTriangle;
        }
        
        if(_player.SmallLeftTriangle == null)
        {
            Shape_SmallTriangle smallLeftTriangle = Instantiate(_smallTriangle, leftPosition , Quaternion.identity, _player.transform);
            smallLeftTriangle.IsRight = false;
            _player.SmallLeftTriangle = smallLeftTriangle;
        }

        Debug.Log($"SpecialSkill {name}");
    }

    public override void ShiftSkill()
    {
        _PlaySound("Dash");

        _triangleDestroyField = Instantiate(_destroyField.gameObject, _player.transform.position, Quaternion.identity, _player.transform);
        Destroy(_triangleDestroyField, _player.DashTime);

        if(_damageCollider == null)
        {
            _damageCollider = GameObject.Find("DamageCollider").GetComponent<DamageCollider>();
        }

        _damageCollider.BeInvincibleWithSkill();
        
        _player.Dash();

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
