using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerShape : MonoBehaviour
{
    [SerializeField]
    private Sprite _mySprite;

    public Sprite MySprite
    {
        get {return _mySprite;}
        set {_mySprite = value;}
    }

    [SerializeField]
    private Color _myColor;

    public Color MyColor
    {
        get {return _myColor;}
        set {_myColor = value;}
    }

    [SerializeField]
    private int _primaryAttackCost;

    public int PrimaryAttackCost
    {
        get {return _primaryAttackCost;}
        set {_primaryAttackCost = value;}
    }

    [SerializeField]
    private int _primaryAttackDamage;

    public int PrimaryAttackDamage
    {
        get {return _primaryAttackDamage;}
        set {_primaryAttackDamage = value;}
    }

    [SerializeField]
    public DestroyField _destroyField;

    [SerializeField]
    private int _specialSkillCost;

    public int SpecialSkillCost
    {
        get {return _specialSkillCost;}
        set {_specialSkillCost = value;}
    }
    [SerializeField]
    private Vector3 _grazeColliderSize;

    public Vector3 GrazeColliderSize
    {
        get {return _grazeColliderSize;}
        set {_grazeColliderSize = value;}
    } 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void PrimaryAttack();

    public abstract void SpecialSkill();

    public abstract void ShiftSkill();
}
