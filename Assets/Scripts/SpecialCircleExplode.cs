using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCircleExplode : MonoBehaviour
{
    private Animator _explodeAnim;
    // Start is called before the first frame update
    void Start()
    {
        _explodeAnim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _explodeAnim.SetBool("blExplode", true);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            Debug.Log($"TakeDamageEnemy");
        }

        if (collider.gameObject.tag == "EnemyCircleBullet")
        {
            Destroy(collider.gameObject);
            Debug.Log($"DestroyEnemyCircleBullet");
        }
    }
}
