using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private int _damageValue;

    public int DamageValue
    {
        get {return _damageValue;}
        set {_damageValue = value;}
    }

 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void DestroyWithParticle()
    {
        
    }
}
