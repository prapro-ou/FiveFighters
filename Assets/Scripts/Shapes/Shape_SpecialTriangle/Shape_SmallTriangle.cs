using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape_SmallTriangle : MonoBehaviour
{
    [SerializeField]
    private int _primaryAttackDamage;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        Destroy(this.gameObject);
    }

    public void PrimaryAttack()
    {
        Debug.Log($"Shape_SmallTriangle.PrimaryAttack {(float)_primaryAttackDamage/10}");
    }
}
