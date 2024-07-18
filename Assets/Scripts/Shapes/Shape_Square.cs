using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape_Square : PlayerShape
{
[SerializeField]
    private Player _player;

    private GameObject _squareDestroyField;

    [SerializeField]
    private GameObject _squarePrimaryBullet;


    private PlayerBullet _playerbullet;

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
        
        Instantiate(_squarePrimaryBullet, vec, Quaternion.identity);
    }

    public override void SpecialSkill()
    {
        Debug.Log($"ShiftSkill {name}");
    }

    public override void ShiftSkill()
    {
        _squareDestroyField = Instantiate(_destroyField.gameObject, _player.transform.position, Quaternion.identity, _player.transform);
        Destroy(_squareDestroyField, 3.0f);
        Instantiate(_squareSpecialBullet, new Vector3(_player.transform.localPosition.x + 0.0f, _player.transform.localPosition.y + 1.5f, _player.transform.localPosition.z + 0.0f), Quaternion.identity);
        Debug.Log($"ShiftSkill {name}");
    }
}
