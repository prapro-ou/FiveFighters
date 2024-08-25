using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum HeartState
{
    Wait,
    Attack1,
    Attack2,
    Attack3,
    Attack4,
    Attack5,
    Attack6
}

public class Enemy_Heart : Enemy
{
    private Player _player;

    private SpriteRenderer _spriteRenderer;

    private SoundManager _soundManager;

    private CameraManager _cameraManager;

    [SerializeField]
    private EnemyBullet _circleBulletPrefab; 

    private HeartState _currentState;

    public HeartState CurrentState
    {
        get {return _currentState;}
        set {_currentState = value;}
    }

    private int _numberOfAttacks;

    public int NumberOfAttacks
    {
        get {return _numberOfAttacks;}
        set {_numberOfAttacks = value;}
    }

    private List<int> _remainingAttacks;

    [SerializeField]
    private int _attackCooltime;

    public int AttackCooltime
    {
        get {return _attackCooltime;}
        set {_attackCooltime = value;}
    }

    [SerializeField]
    private GameObject _healHeartBullet;

    [SerializeField]
    private GameObject _damageHeartBullet;

    [SerializeField]
    private EnemyBullet _slowHeartBullet;

    [SerializeField]
    private EnemyBullet _fastHeartBullet;

    [SerializeField]
    private EnemyExplosionBullet _explosionHeartBullet;

    [SerializeField]
    private EnemyBullet _stanHeartBullet;

    [SerializeField]
    private EnemyStuckBullet _halfHeartBullet;

    [SerializeField]
    private GameObject _heartSpawnEffect;

    [SerializeField]
    private GameObject _heartDeathEffect;

    [SerializeField]
    private GameObject _heartSimpleEfect;

    // Start is called before the first frame update
    void Start()
    {
        CurrentState = HeartState.Wait;
        NumberOfAttacks = System.Enum.GetValues(typeof(HeartState)).Length - 1;
        // Debug.Log("NumberOfAttacks: " + NumberOfAttacks);

        _remainingAttacks = Enumerable.Range(1, NumberOfAttacks).ToList();

        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.CurrentEnemy = this;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void StartAttacking()
    {
        StartCoroutine(_StartAttackingCycle());
    }

    private IEnumerator _StartAttackingCycle()
    {
        while(true)
        {
            switch(CurrentState)
            {
                case HeartState.Wait:
                {
                    int random = Random.Range(0, _remainingAttacks.Count);

                    Debug.Log($"EnemyAttack: {_remainingAttacks[random]}");
                    CurrentState = (HeartState)System.Enum.GetValues(typeof(HeartState)).GetValue(_remainingAttacks[random]);
                    _remainingAttacks.Remove(_remainingAttacks[random]);

                    if(_remainingAttacks.Count == 0)
                    {
                        _remainingAttacks = Enumerable.Range(1, NumberOfAttacks).ToList();
                    }

                    yield return new WaitForSeconds(AttackCooltime);
                    break;
                }
                case HeartState.Attack1:
                {
                    yield return StartCoroutine(_3BurstShot());
                    CurrentState = HeartState.Wait;
                    break;
                }
                case HeartState.Attack2:
                {
                    yield return StartCoroutine(_LineShot());
                    CurrentState = HeartState.Wait;
                    break;
                }
                case HeartState.Attack3:
                {
                    yield return StartCoroutine(_SlowOrFastShot());
                    CurrentState = HeartState.Wait;
                    break;
                }
                case HeartState.Attack4:
                {
                    yield return StartCoroutine(_FlashAndExplosionShot());
                    CurrentState = HeartState.Wait;
                    break;
                }
                case HeartState.Attack5:
                {
                    yield return StartCoroutine(_BulletRainShot());
                    CurrentState = HeartState.Wait;
                    break;
                }
                case HeartState.Attack6:
                {
                    yield return StartCoroutine(_StuckAttack());
                    CurrentState = HeartState.Wait;
                    break;
                }
            }
        }
    }

    //出現したときに、StartAttackingよりも先に実行されるコルーチン。これが終了してからStartAttackingメソッドが実行される。
    //登場したときの演出をこのメソッドに記述しよう。
    public override IEnumerator StartSpawnAnimation()
    {
        Debug.Log("StartSpawnAnimation");

        // エネミーを画面外の下から登場させる
        Vector3 startPosition = new Vector3(transform.position.x, -5f, transform.position.z);
        Vector3 endPosition = transform.position;
        Vector3 startScale = new Vector3(0.1f, 0.1f, 1f);
        Vector3 endScale = new Vector3(0.7f, 0.7f, 1f);

        transform.position = startPosition;

        float duration = 2f;
        float elapsedTime = 0f;

        // カメラをズームインさせながらエネミーを登場させる
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(_cameraManager.SetSizeOnCurve(3.5f, duration));

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            elapsedTime += Time.deltaTime;
            GameObject simpleEffect = Instantiate(_heartSimpleEfect, transform.position, Quaternion.identity);
            Destroy(simpleEffect, 0.3f);
            yield return null;
        }

        yield return StartCoroutine(_cameraManager.MoveToPointOnCurve(transform.position));

        GameObject spawnEffect = Instantiate(_heartSpawnEffect, transform.position, Quaternion.identity);

        // エフェクトを徐々に小さくする処理
        float shrinkDuration = 1f;
        float shrinkElapsedTime = 0f;
        Vector3 initialScale = spawnEffect.transform.localScale;
        
        // 色の初期値と最終値
        Color startColor = Color.white; // #FFFFFF
        Color endColor = new Color(1f, 0.54f, 0.8f); // #FF8ACC

        _PlaySound("Spawn5");
        while (shrinkElapsedTime < shrinkDuration)
        {
            float t = shrinkElapsedTime / shrinkDuration;
            
            _spriteRenderer.color = Color.Lerp(startColor, endColor, t);
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            spawnEffect.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);
            shrinkElapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(spawnEffect);

        _PlaySound("Spawn3");
        // 振動演出を加える
        yield return StartCoroutine(_cameraManager.Vibrate(0.3f, 0.2f));

        // エネミーが定位置に到達した後にカメラを元に戻す
        StartCoroutine(_cameraManager.SetSizeOnCurve(5f));
        yield return StartCoroutine(_cameraManager.MoveToPointOnCurve(Vector3.zero));

        yield return new WaitForSeconds(1f); // アニメーション後の待機時間
    }

    //死亡したときに、GameManagerによって実行されるコルーチン。
    //死亡したときの演出をこのメソッドに記述しよう。アニメーション自体はスクリプトで書かず、アニメーター(アニメーション)コンポーネントで実装することもできる。
    public override IEnumerator StartDeathAnimation()
    {
        float dissolveDuration = 2f; // 消滅するまでの時間

        foreach(GameObject bullet in GameObject.FindGameObjectsWithTag("HeartStuckBullet"))
        {
            Destroy(bullet.gameObject);
        }

        Debug.Log("StartDeathAnimation");

        Vector3 startPosition = transform.position;
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0f;
        float sinkSpeed = 1f;
        
        Color startColor = new Color(1f, 0.54f, 0.8f); // #FF8ACC
        Color endColor = Color.white; // #FFFFFF

        GameObject effect = Instantiate(_heartDeathEffect, transform.position, Quaternion.identity);

        _PlaySound("DeathHeart");
        // 消滅アニメーションを開始
        while (elapsedTime < dissolveDuration)
        {
            float t = elapsedTime / dissolveDuration;

            _spriteRenderer.color = Color.Lerp(startColor, endColor, t);

            // スケールを徐々に縮小
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            // 少しずつ透明にする
            Color newColor = _spriteRenderer.color;
            newColor.a = Mathf.Lerp(1f, 0f, t);
            _spriteRenderer.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(effect);

        // 最後に位置とスケールを完全にセット
        transform.position = startPosition + Vector3.down * sinkSpeed * (dissolveDuration - elapsedTime);
        transform.localScale = Vector3.zero;

        // オブジェクトを削除
        Destroy(gameObject);
    }

    private IEnumerator _3BurstShot()
    {
        Debug.Log("Start 3BurstShot");

        for (int k = 0; k < 3; k++)
        {
            _PlaySound("Caution1");
            Color color = _spriteRenderer.color;
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.1f);
            yield return new WaitForSeconds(0.2f);

            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1.0f);
            yield return new WaitForSeconds(0.2f);
        }

        for (int j = 0; j < 15; j++)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);

            Vector3 target = _player.transform.position;
            Vector3 power = target - this.transform.position;

            for (int i = 0; i < 3; i++)
            {
                _PlaySound("NormalBullet");
                EnemyBullet bullet = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);
                bullet.GetComponent<Rigidbody2D>().AddForce(power.normalized * 10, ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.3f);
        }

        Debug.Log("Finish 3BurstShot");
    }

    private IEnumerator _LineShot()
    {
        Debug.Log("Start LineShot");

        for (int j = 0; j < 7; j++)
        {
            //ランダムに一つだけピンクのハートの位置を設定
            int healIndex = Random.Range(0, 5);
            Vector3 startPosition = new Vector3(-3.2f, 1.5f, 0.0f);
            GameObject[] hearts = new GameObject[5];

            for (int i = 0; i < 5; i++)
            {
                if (i == healIndex)
                {
                    //ランダムな位置にピンクのハートを生成
                    hearts[i] = Instantiate(_healHeartBullet.gameObject, startPosition + new Vector3((float)i*1.6f, 0.0f, 0.0f), Quaternion.identity);
                }
                else
                {
                    //その他の位置には紫のハートを生成
                    hearts[i] = Instantiate(_damageHeartBullet.gameObject, startPosition + new Vector3((float)i*1.6f, 0.0f, 0.0f), Quaternion.identity);
                }
            }
            // 全てのハートを同時に発射する
            foreach (GameObject heart in hearts)
            {
                Rigidbody2D rb = heart.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    _PlaySound("ShootHeartBullet2");
                    rb.velocity = new Vector2(0, -3.0f); // 同時に前方向に発射
                }
                Destroy(heart, 3.0f);
            }
            yield return new WaitForSeconds(1.2f);
        }

        Debug.Log("Finish LineShot");
    }

    private IEnumerator _SlowOrFastShot()
    {
        Debug.Log("Start SlowOrFastShot");

        for (int i = 0; i < 5; i++)
        {
            // 中央の弾を生成して発射
            ShootBullet(0);

            // 左方向に角度をつけて弾を発射
            ShootBullet(15f);

            // 右方向に角度をつけて弾を発射
            ShootBullet(-15f);

            yield return new WaitForSeconds(0.7f);
        }

        Debug.Log("Finish SlowOrFastShot");
    }

    private void ShootBullet(float angleOffset)
    {
        // 弾の種類をランダムに決定
        EnemyBullet bulletPrefab;
        int sloworfast = Random.Range(0, 2);

        if (sloworfast == 0)
        {
            bulletPrefab = _slowHeartBullet;
        }
        else
        {
            bulletPrefab = _fastHeartBullet;
        }

        // 弾の生成位置
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, bulletPrefab.transform.position.z);

        // プレイヤーへの方向
        Vector3 target = _player.transform.position;
        Vector3 direction = (target - this.transform.position).normalized;

        // 角度を適用
        direction = Quaternion.Euler(0, 0, angleOffset) * direction;

        // 弾を生成して発射
        EnemyBullet bullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(direction * 5, ForceMode2D.Impulse);

        Vector3 effectPosition = bullet.transform.position + direction * 1f;
        _PlaySound("ShootHeartBullet1");
        GameObject effect = Instantiate(_heartSimpleEfect, effectPosition, Quaternion.identity);
        Destroy(effect.gameObject, 0.5f);
    }

    private IEnumerator _FlashAndExplosionShot()
    {
        Debug.Log("Start FlashAndExplosionShot");

        for (int i = 0; i < 10; i++)
        {
            _PlaySound("ShootHeartBullet1");
            Instantiate(_explosionHeartBullet, _player.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("Finish FlashAndExplosionShot");
    }

    private IEnumerator _BulletRainShot()
    {
        Debug.Log("Start _BulletRainShot");

        for (int i = 0; i < 20; i++)
        {
            int selectBullet = Random.Range(0, 5);
            EnemyBullet bulletPrefab;

            if (selectBullet == 0)
            {
                bulletPrefab = _stanHeartBullet;
            }
            else if (selectBullet == 1)
            {
                bulletPrefab = _slowHeartBullet;
            }
            else if (selectBullet == 2)
            {
                bulletPrefab = _fastHeartBullet;
            }
            else
            {
                bulletPrefab = _circleBulletPrefab;
            }

            float spawnX = Random.Range(-4f, 4f);
            float spawnY = Random.Range(3f, 4f);
            Vector2 spawnPosition = new Vector2(spawnX, spawnY);

            // 弾の生成
            _PlaySound("NormalBullet");
            EnemyBullet bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(0.3f);

            if (bullet != null)
            {
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                if (bulletRb != null)
                {
                    bulletRb.gravityScale = 2f;
                }
            }
        }

        yield return new WaitForSeconds(2f);
        _player.CurrentSpeed = 5f;
        Debug.Log("Return original speed");      

        Debug.Log("Finish _BulletRainShot");
    }

    private IEnumerator _StuckAttack()
    {
        Debug.Log("Start _StuckAttack");

        EnemyStuckBullet bulletPrefab = _halfHeartBullet;
        for (int i = 0; i < 5; i++)
        {
            float spawnY = _player.transform.position.y;
            Vector2 spawnPosition1 = new Vector2(-5f, spawnY);
            Vector2 spawnPosition2 = new Vector2(5f, spawnY);

            _PlaySound("ShootHeartBullet3");
            EnemyStuckBullet bullet1 = Instantiate(bulletPrefab, spawnPosition1, Quaternion.identity);
            EnemyStuckBullet bullet2 = Instantiate(bulletPrefab, spawnPosition2, Quaternion.Euler(0, 180, 0));
            yield return new WaitForSeconds(0.2f);
            
            float elapsedTime = 0f;

            // 弾を一定時間動かす
            while (elapsedTime < 1.539f)
            {
                if (bullet1 != null)
                {
                    bullet1.transform.Translate(Vector3.right * 2.6f * Time.deltaTime);
                }
                if (bullet2 != null)
                {
                    bullet2.transform.Translate(Vector3.right * 2.6f * Time.deltaTime);
                }
                elapsedTime += Time.deltaTime;
                yield return null;  // 次のフレームまで待機
            }
            _PlaySound("Stuck");
            yield return StartCoroutine(_cameraManager.Vibrate(0.2f, 0.1f));

            if (bullet1 != null)
            {
                Destroy(bullet1.gameObject);
            }
            if(bullet2 != null)
            {
                Destroy(bullet2.gameObject);
            }
            //yield return new WaitForSeconds(0.2f);  // 次の弾を生成するまでの待機時間
        }

        yield return new WaitForSeconds(0.5f);

        Debug.Log("Finish _StuckAttack");

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
