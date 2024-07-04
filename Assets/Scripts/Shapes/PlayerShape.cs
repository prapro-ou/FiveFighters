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
