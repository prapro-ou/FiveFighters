using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Diamond_LeftCanon : MonoBehaviour
{
    private SoundManager _soundManager;

    [SerializeField]
    private EnemyBullet _laserPrefab;

    [SerializeField]
    private GameObject _generateEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Shoot");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Shoot()
    {
        Vector3 pos = new Vector3(transform.position.x + 0.1f, transform.position.y, _laserPrefab.transform.position.z);
        Vector3 power = new Vector3(5.0f, 0, 0);

        yield return new WaitForSeconds(5);

        EnemyBullet beam = Instantiate(_laserPrefab, pos, Quaternion.Euler(0, 0, 90));
        beam.GetComponent<Rigidbody2D>().velocity = power;
        _PlaySound("Laser");

        yield return new WaitForSeconds(3);

        Instantiate(_generateEffectPrefab, pos, Quaternion.Euler(0, 0, 270));
        Destroy(this.gameObject);

        yield break;
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
