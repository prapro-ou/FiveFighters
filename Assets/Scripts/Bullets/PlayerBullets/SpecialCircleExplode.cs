using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCircleExplode : MonoBehaviour
{
    private Animator _explodeAnim;

    [SerializeField]
    private int _explosionDamage;

    private SoundManager _soundManager;

    // Start is called before the first frame update
    void Start()
    {
        _explodeAnim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            collider.gameObject.GetComponent<Enemy>().TakeDamage(_explosionDamage);
            Debug.Log($"TakeDamageEnemy{_explosionDamage}");
        }

        if (collider.gameObject.tag == "EnemyBullet")
        {
            Destroy(collider.gameObject);
            Debug.Log($"DestroyEnemyBullet");

            _PlaySound("DestroyBullet");
        }
    }

    private void _PlaySound(string name)
    {
        if(_soundManager == null)
        {
            _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        }

        _soundManager.PlaySound(name);
    }
}
