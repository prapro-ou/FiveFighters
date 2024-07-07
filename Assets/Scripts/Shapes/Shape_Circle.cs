using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape_Circle : PlayerShape
{
    private Player _player;

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
        Debug.Log($"ShiftSkill {name}");
    }

    public override void SpecialSkill()
    {
        Debug.Log($"ShiftSkill {name}");
    }

    public override void ShiftSkill()
    {
        Instantiate(_destroyField.gameObject, _player.transform.position, Quaternion.identity);
        Debug.Log($"ShiftSkill {name}");
    }
}
