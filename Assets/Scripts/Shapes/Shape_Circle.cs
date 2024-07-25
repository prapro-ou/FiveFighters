using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape_Circle : PlayerShape
{
    [SerializeField]
    private Player _player;

    private GameObject _circleDestroyField;

    [SerializeField]
    private GameObject _circlePrimaryBullet;
    private GameObject _specialCircleBullet;

    [SerializeField]
    private GameObject _playerCircleBullet;


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
        
        Instantiate(PlayerCircleBullet, vec, Quaternion.identity);
    }

    public override void SpecialSkill()
    {
        Instantiate(_specialCircleBullet.gameObject, _player.transform.position, Quaternion.identity);
        Debug.Log($"SpecialSkill {name}");
    }

    public override void ShiftSkill()
    {
        _circleDestroyField = Instantiate(_destroyField.gameObject, _player.transform.position, Quaternion.identity);
        Destroy(_circleDestroyField, 3.0f);
        Debug.Log($"ShiftSkill {name}");
    }
}
