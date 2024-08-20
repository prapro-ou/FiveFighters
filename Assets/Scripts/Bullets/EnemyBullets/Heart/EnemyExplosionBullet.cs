using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosionBullet : EnemyBullet
{
    private SpriteRenderer _spriteRenderer;

    private Collider2D _explosionCollider;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _explosionCollider = GetComponent<Collider2D>();

        //colliderを無効化
        _explosionCollider.enabled = false;

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
            float blinkInterval = Mathf.Lerp(maxBlinkInterval, minBlinkInterval, t);  // 二次関数的に縮小
        
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.5f);
            yield return new WaitForSeconds(blinkInterval);
        
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f);
            yield return new WaitForSeconds(blinkInterval);

            currentDuration += blinkInterval * 2;
        }

        _explosionCollider.enabled = true;
        
        // Instantiate(explosionEffect, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.2f); //ここで爆発

        _explosionCollider.enabled = false;

        Destroy(this.gameObject);
    }
}
