using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape_SmallTriangle : MonoBehaviour
{
    [SerializeField]
    private int _primaryAttackDamage;

    [SerializeField]
    private PrimaryTriangleSubBullet _primaryTriangleSubBullet;

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
        PlayerBullet triangleSubBullet = Instantiate(_primaryTriangleSubBullet, this.transform.position, Quaternion.identity);
        triangleSubBullet.DamageValue = _primaryAttackDamage/2;
        Debug.Log($"Shape_SmallTriangle.PrimaryAttack {_primaryAttackDamage/2}");
    }
}
