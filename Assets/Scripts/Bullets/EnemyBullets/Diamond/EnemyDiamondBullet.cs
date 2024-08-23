using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDiamondBullet : EnemyBullet
{
    private Enemy_Diamond _diamond;

    // Start is called before the first frame update
    void Start()
    {
        _diamond = GameObject.Find("Enemy_Diamond").GetComponent<Enemy_Diamond>();
        //StartCoroutine("_Rotate");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.RotateAround(_diamond.transform.position, Vector3.forward, 10f * Time.deltaTime);
    }

    private IEnumerator _Rotate()
    {
        yield return new WaitForSeconds(0.1f);
        for(int i = 0; i < 360; ++i)
        {
            this.transform.RotateAround(_diamond.transform.position, Vector3.forward, 10.0f);
            yield return new WaitForSeconds(0.01f);
        }

        yield break;
    }
}
