using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSquareBullet : MonoBehaviour
{
    private Player _player;

    [SerializeField]
    private GameObject _squareSpecialBullet;

    private int _direction;
    public int Direction
    {
        get {return _direction;}
        set {_direction = value;}
    }

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
        for(float i = 0.1f ; i < 0.5f ; i += 0.01f)
        {
            this.transform.localScale = new Vector3(i * 5, i, 1);
            yield return null;
        }
        BulletOperator();
        yield break;
    }

    private void BulletOperator()
    {
        if(_direction == 0)
            StartCoroutine("MoveToAhead");
        else if(_direction == 1)
            StartCoroutine("MoveToLeft");
        else if(_direction == 2)
            StartCoroutine("MoveToBack");
        else if(_direction == 3)
            StartCoroutine("MoveToRight");
    }

    private IEnumerator MoveToAhead()
    {
        for(float j = 0.1f; j < 5.0f; j += 0.1f)
        {
            this.transform.localPosition += new Vector3(0.0f, 0.1f, 0.0f);
            yield return null;
        }
        Destroy(this.gameObject);
    }

    private IEnumerator MoveToLeft()
    {
        for(float j = 0.1f; j < 5.0f; j += 0.1f)
        {
            this.transform.localPosition += new Vector3(-0.1f, 0.0f, 0.0f);
            yield return null;
        }
        Destroy(this.gameObject);
    }

    private IEnumerator MoveToBack()
    {
        for(float j = 0.1f; j < 5.0f; j += 0.1f)
        {
            this.transform.localPosition += new Vector3(0.0f, -0.1f, 0.0f);
            yield return null;
        }
        Destroy(this.gameObject);
    }

    private IEnumerator MoveToRight()
    {
        for(float j = 0.1f; j < 5.0f; j += 0.1f)
        {
            this.transform.localPosition += new Vector3(0.1f, 0.0f, 0.0f);
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
