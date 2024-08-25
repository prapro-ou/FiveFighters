using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starstar_bullet : MonoBehaviour
{
    private SoundManager _soundManager;

    [SerializeField]
    private EnemyBullet _starBulletPrefab;

    [SerializeField]
    private EnemyBullet _circleBulletPrefab;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return StartCoroutine(_Explosion());

        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator _Explosion()
    {
        float waittime = Random.Range(3, 10);

        yield return new WaitForSeconds(0.1f * waittime);

        Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);

        EnemyBullet bullet1 = Instantiate(_starBulletPrefab, pos, Quaternion.identity);
        EnemyBullet bullet2 = Instantiate(_starBulletPrefab, pos, Quaternion.identity);
        EnemyBullet bullet3 = Instantiate(_starBulletPrefab, pos, Quaternion.identity);
        
        Vector3 power1 = new Vector3(0, -4.0f, 0);
        Vector3 power2 = new Vector3(3.46f, 2.0f, 0);
        Vector3 power3 = new Vector3(-3.46f, 2.0f, 0);

        bullet1.GetComponent<Rigidbody2D>().velocity = power1;
        bullet2.GetComponent<Rigidbody2D>().velocity = power2;
        bullet3.GetComponent<Rigidbody2D>().velocity = power3;

        _PlaySound("Rupture");

        yield return null;
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
