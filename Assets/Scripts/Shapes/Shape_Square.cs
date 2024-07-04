using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape_Square : PlayerShape
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
        Debug.Log($"ShiftSkill {name}");
    }
}