using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape_Square : PlayerShape
{
    private Player _player;
    private GameObject _squareDestroyField;

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
        Debug.Log($"squ");
    }

    public override void SpecialSkill()
    {
        Debug.Log($"ShiftSkill {name}");
    }

    public override void ShiftSkill()
    {
        _squareDestroyField = Instantiate(_destroyField.gameObject, _player.transform.position, Quaternion.identity, _player.transform);
        Destroy(_squareDestroyField, 3.0f);
        Debug.Log($"ShiftSkill {name}");
    }
}
