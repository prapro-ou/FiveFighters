using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape_Square : PlayerShape
{
    private Player _player;

    private GameObject _squareDestroyField;

    [SerializeField]
    private PrimarySquareBullet _primarySquareBullet;

    [SerializeField]
    private GameObject _squareSpecialBullet;


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
        
        PlayerBullet bullet = Instantiate(_primarySquareBullet, vec, Quaternion.identity);

        bullet.DamageValue = PrimaryAttackDamage;
    }

    public override void SpecialSkill()
    {
        GameObject bullet;

        bullet = Instantiate(_squareSpecialBullet, new Vector3(_player.transform.localPosition.x + 0.0f, _player.transform.localPosition.y + 2.0f, _player.transform.localPosition.z + 0.0f), Quaternion.identity);
        bullet.GetComponent<SpecialSquareBullet>().Direction = 0;

        bullet = Instantiate(_squareSpecialBullet, new Vector3(_player.transform.localPosition.x - 2.0f, _player.transform.localPosition.y + 0.0f, _player.transform.localPosition.z + 0.0f), Quaternion.Euler(0,0,90));
        bullet.GetComponent<SpecialSquareBullet>().Direction = 1;

        bullet = Instantiate(_squareSpecialBullet, new Vector3(_player.transform.localPosition.x + 0.0f, _player.transform.localPosition.y - 2.0f, _player.transform.localPosition.z + 0.0f), Quaternion.identity);
        bullet.GetComponent<SpecialSquareBullet>().Direction = 2;

        bullet = Instantiate(_squareSpecialBullet, new Vector3(_player.transform.localPosition.x + 2.0f, _player.transform.localPosition.y + 0.0f, _player.transform.localPosition.z + 0.0f), Quaternion.Euler(0,0,90));
        bullet.GetComponent<SpecialSquareBullet>().Direction = 3;

        Debug.Log($"ShiftSkill {name}");
    }

    public override void ShiftSkill()
    {
        GameObject bullet;

        _squareDestroyField = Instantiate(_destroyField.gameObject, _player.transform.position, Quaternion.identity, _player.transform);
        Destroy(_squareDestroyField, 0.5f);
        bullet = Instantiate(_squareSpecialBullet, new Vector3(_player.transform.localPosition.x + 0.0f, _player.transform.localPosition.y + 2.0f, _player.transform.localPosition.z + 0.0f), Quaternion.identity);
        bullet.GetComponent<SpecialSquareBullet>().Direction = 0;
    }
}