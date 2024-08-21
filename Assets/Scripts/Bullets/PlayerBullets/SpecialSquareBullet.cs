using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSquareBullet : MonoBehaviour
{
    private Player _player;

    private int _direction;
    public int Direction
    {
        get {return _direction;}
        set {_direction = value;}
    }

    private SoundManager _soundManager;

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
        _player.PrimaryGrazeCount += 3;
        _player.SpecialGrazeCount += 10;

        _PlaySound("DestroyBullet");
    }

    private IEnumerator ScaleUp()
    {
        for(float i = 0.1f ; i < 0.5f ; i += Time.deltaTime)
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
        float duration = 5.0f; // 移動にかかる総時間
        float elapsedTime = 0f; // 経過時間

        while (elapsedTime < duration)
        {
            this.transform.localPosition += Time.deltaTime * new Vector3(0.0f, 2.0f, 0.0f);
            elapsedTime += Time.deltaTime; // 経過時間を加算
            yield return null;
        }

        Destroy(this.gameObject);
    }

    private IEnumerator MoveToLeft()
    {
        float duration = 5.0f; // 移動にかかる総時間
        float elapsedTime = 0f; // 経過時間

        while (elapsedTime < duration)
        {
            this.transform.localPosition += Time.deltaTime * new Vector3(-2.0f, 0.0f, 0.0f);
            elapsedTime += Time.deltaTime; // 経過時間を加算
            yield return null;
        }

        Destroy(this.gameObject);
    }

    private IEnumerator MoveToBack()
    {
        float duration = 5.0f; // 移動にかかる総時間
        float elapsedTime = 0f; // 経過時間

        while (elapsedTime < duration)
        {
            this.transform.localPosition += Time.deltaTime * new Vector3(0.0f, -2.0f, 0.0f);
            elapsedTime += Time.deltaTime; // 経過時間を加算
            yield return null;
        }

        Destroy(this.gameObject);
    }

    private IEnumerator MoveToRight()
    {
        float duration = 5.0f; // 移動にかかる総時間
        float elapsedTime = 0f; // 経過時間

        while (elapsedTime < duration)
        {
            this.transform.localPosition += Time.deltaTime * new Vector3(2.0f, 0.0f, 0.0f);
            elapsedTime += Time.deltaTime; // 経過時間を加算
            yield return null;
        }

        Destroy(this.gameObject);
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
