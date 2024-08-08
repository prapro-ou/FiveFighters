using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Diamond_LeftCanon : MonoBehaviour
{
    [SerializeField]
    private EnemyBullet _beamPrefab;

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
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, _beamPrefab.transform.position.z);
        Vector3 power = new Vector3(5.0f, 0, 0);

        yield return new WaitForSeconds(5);

        EnemyBullet beam = Instantiate(_beamPrefab, pos, Quaternion.Euler(0, 0, 90));
        beam.GetComponent<Rigidbody2D>().AddForce(power, ForceMode2D.Impulse);

        yield return new WaitForSeconds(3);

        Destroy(this.gameObject);

        yield break;
    }
}
