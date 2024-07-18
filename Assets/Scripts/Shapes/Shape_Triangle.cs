using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape_Triangle : PlayerShape
{
    private Player _player;
    private GameObject _triangleDestroyField;
    

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
        Debug.Log($"tri");
    }

    public override void SpecialSkill()
    {
        Debug.Log($"ShiftSkill {name}");
    }

    public override void ShiftSkill()
    {
        _triangleDestroyField = Instantiate(_destroyField.gameObject, _player.transform.position, Quaternion.identity, _player.transform);
        Destroy(_triangleDestroyField, 3.0f);
        Debug.Log($"ShiftSkill {name}");
    }
}
