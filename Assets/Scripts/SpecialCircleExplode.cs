using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCircleExplode : MonoBehaviour
{
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
