using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape_Circle : PlayerShape
{
    [SerializeField]
    private Player _player;

    private GameObject _circleDestroyField;

    [SerializeField]
    private GameObject PlayerCircleBullet;

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

        Instantiate(PlayerCircleBullet,vec,Quaternion.identity);
        _playerbullet.DamageValue = MyShape.PrimaryAttackDamage;
        
    }

    public override void SpecialSkill()
    {
        Debug.Log($"ShiftSkill {name}");
    }

    public override void ShiftSkill()
    {
        _circleDestroyField = Instantiate(_destroyField.gameObject, _player.transform.position, Quaternion.identity, _player.transform);
        Destroy(_circleDestroyField, 3.0f);
        Debug.Log($"ShiftSkill {name}");
    }
}
