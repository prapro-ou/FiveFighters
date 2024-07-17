using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSquareBullet : MonoBehaviour
{
    private Player _player;

    [SerializeField]
    private GameObject _squareSpecialBullet;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        StartCoroutine("ScaleUp");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Destroy(collider.gameObject);
        _player.PrimaryGrazeCount += 1;
        _player.SpecialGrazeCount += 1;
    }

    private IEnumerator ScaleUp()
    {
        for(float i = 0.1f ; i < 0.5f ; i += 0.025f)
        {
            this.transform.localScale = new Vector3(i * 5, i, 1);
            yield return new WaitForSeconds(0.05f);
        }
        StartCoroutine("MoveToCorner");
        yield break;
    }

    private IEnumerator MoveToCorner()
    {
        for(float j = 0.1f; j < 5.0f; j += 0.1f)
        {
            this.transform.localPosition += new Vector3(0.0f, 0.1f, 0.0f);
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(this.gameObject);
    }
}
