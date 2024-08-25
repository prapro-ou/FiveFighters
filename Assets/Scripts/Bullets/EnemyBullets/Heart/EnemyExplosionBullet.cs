using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosionBullet : MonoBehaviour
{
    private Player _player;

    private SpriteRenderer _spriteRenderer;

    private Collider2D _explosionCollider;

    private CameraManager _cameraManager;

    private SoundManager _soundManager;

    [SerializeField]
    private int _explosionValue;

    private bool _isExplode;

    [SerializeField]
    private GameObject _heartExplosionEffect;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _explosionCollider = GetComponent<Collider2D>();

        _explosionCollider.enabled = false;
        _isExplode = false;
        // エリアを出現させ、点滅処理を開始
        StartCoroutine(_FlashAndExplosion());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator _FlashAndExplosion()
    {
        float totalDuration = 5.0f;  // 点滅が継続する合計時間
        float currentDuration = 0f;  // 経過時間
        float maxBlinkInterval = 0.5f;  // 初期の点滅間隔
        float minBlinkInterval = 0.05f;  // 最短の点滅間隔
        // 点滅処理
        while (currentDuration < totalDuration)
        {
            float t = currentDuration / totalDuration;  // 経過時間の割合 (0～1)
            float blinkInterval = Mathf.Lerp(maxBlinkInterval, minBlinkInterval, t);

            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.5f);
            yield return new WaitForSeconds(blinkInterval);
        
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f);
            yield return new WaitForSeconds(blinkInterval);

            currentDuration += blinkInterval * 2;
        }
        
        _explosionCollider.enabled = true;
        yield return new WaitForSeconds(0.05f);
        GameObject effect = Instantiate(_heartExplosionEffect, transform.position, Quaternion.identity);
        _PlaySound("Explosion5");
        _isExplode = true; //ここで爆発
        yield return new WaitForSeconds(0.1f);
        _isExplode = false; //ここで鎮火
        _spriteRenderer.color = new Color(0f, 0f, 0f, 0.7f); //_spriteRenderer.color = Color.black, α値を少し下げる
        yield return new WaitForSeconds(3f);

        Destroy(this.gameObject);
        Destroy(effect);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (_isExplode)
        {
            _player.TakeDamage(_explosionValue); //爆発ダメージ
            Debug.Log($"Heart explosion damage {_explosionValue}");
            _isExplode = false;
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
