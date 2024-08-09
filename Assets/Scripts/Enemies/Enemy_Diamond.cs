using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum DiamondState
{
    Wait,
    Attack1,
    Attack2,
    Attack3,
    Attack4,
    Attack5,
    Attack6,
    Attack7
}

public class Enemy_Diamond : Enemy
{
    private Player _player;

    [SerializeField]
    private EnemyBullet _circleBulletPrefab;

    [SerializeField]
    private EnemyBullet _beamPrefab;

    [SerializeField]
    private EnemyBullet _reduceBulletPrefab;

    [SerializeField]
    private Enemy_Diamond_LeftCanon _leftCanonPrefab;

    [SerializeField]
    private Enemy_Diamond_RightCanon _rightCanonPrefab;

    [SerializeField]
    private GameObject _bitPrefab;

    [SerializeField]
    private GameObject _generateEffectPrefab;

    [SerializeField]
    private GameObject _flashEffectPrefab;

    [SerializeField]
    private GameObject _explodePrefab;

    private DiamondState _currentState;

    public DiamondState CurrentState
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

    // Start is called before the first frame update
    void Start()
    {
        CurrentState = DiamondState.Wait;
        NumberOfAttacks = System.Enum.GetValues(typeof(DiamondState)).Length - 1;

        _remainingAttacks = Enumerable.Range(1, NumberOfAttacks).ToList();

        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.CurrentEnemy = this;
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
                case DiamondState.Wait:
                {
                    int random = Random.Range(0, _remainingAttacks.Count);

                    Debug.Log(_remainingAttacks[random]);
                    CurrentState = (DiamondState)System.Enum.GetValues(typeof(DiamondState)).GetValue(_remainingAttacks[random]);
                    _remainingAttacks.Remove(_remainingAttacks[random]);

                    if(_remainingAttacks.Count == 0)
                    {
                        _remainingAttacks = Enumerable.Range(1, NumberOfAttacks).ToList();
                    }

                    yield return new WaitForSeconds(AttackCooltime);
                    break;
                }
                case DiamondState.Attack1:
                //3つの頂点から平行に射撃
                {
                    Debug.Log("Attack:" + CurrentState);
                    yield return StartCoroutine(_3VerticesShoot());
                    yield return StartCoroutine(_3VerticesShoot());
                    yield return StartCoroutine(_3VerticesShoot());
                    CurrentState = DiamondState.Wait;
                    break;
                }
                case DiamondState.Attack2:
                //分身設置
                {
                    Debug.Log("Attack:" + CurrentState);
                    yield return StartCoroutine(_GenerateCanon());
                    CurrentState = DiamondState.Wait;
                    break;
                }
                case DiamondState.Attack3:
                //砲台を近くに展開し，三点からビーム
                {
                    Debug.Log("Attack:" + CurrentState);
                    yield return StartCoroutine(_BeamShoot());
                    CurrentState = DiamondState.Wait;
                    break;
                }
                case DiamondState.Attack4:
                //高速かすり値減少弾 発射時点のプレイヤーの位置に飛ぶ．
                {
                    Debug.Log("Attack:" + CurrentState);
                    yield return StartCoroutine(_AimingRapidShoot());
                    CurrentState = DiamondState.Wait;
                    break;
                }
                case DiamondState.Attack5:
                //画面中央で乱れ撃ち
                {
                    Debug.Log("Attack:" + CurrentState);
                    yield return StartCoroutine(_RandomShoot());
                    CurrentState = DiamondState.Wait;
                    break;
                }
                case DiamondState.Attack6:
                //3つの頂点から扇形に射撃
                {
                    Debug.Log("Attack:" + CurrentState);
                    yield return StartCoroutine(_FanShoot());
                    CurrentState = DiamondState.Wait;
                    break;
                }
                case DiamondState.Attack7:
                //弾速が違う弾が混ざった射撃を10発 1発ごとにホーミング
                {
                    Debug.Log("Attack:" + CurrentState);
                    yield return StartCoroutine(_ChangeSpeedShoot());
                    CurrentState = DiamondState.Wait;
                    break;
                }
            }
        }
    }

    //出現したときに、StartAttackingよりも先に実行されるコルーチン。これが終了してからStartAttackingメソッドが実行される。
    //登場したときの演出をこのメソッドに記述しよう。アニメーション自体はスクリプトで書かず、アニメーターコンポーネントで実装することもできる。
    public override IEnumerator StartSpawnAnimation()
    {
        Debug.Log("StartSpawnAnimation");

        yield return new WaitForSeconds(5); //Sample
    }


    //死亡したときに、GameManagerによって実行されるコルーチン。
    //死亡したときの演出をこのメソッドに記述しよう。アニメーション自体はスクリプトで書かず、アニメーター(アニメーション)コンポーネントで実装することもできる。
    public override IEnumerator StartDeathAnimation()
    {
        Debug.Log("StartDeathAnimation");

        Color startColor = Color.white;
        Color endColor = Color.black;
        SpriteRenderer rend = this.GetComponent<SpriteRenderer>();

        for(float i = 0.0f; i < 30.0f; ++i)
        {
            rend.material.color = Color.Lerp(startColor, endColor, i / 30.0f);

            yield return new WaitForSeconds(0.1f);
        }
        Instantiate(_explodePrefab, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.5f); //Sample

        Destroy(this.gameObject);

        yield return new WaitForSeconds(1); //Sample
    }

    private IEnumerator _3VerticesShoot()
    {
        //直進させるための力
        Vector3 power = new Vector3(0, -5.0f, 0);
        //自機(敵)の位置を取得
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);
        //中心部分の弾を生成
        EnemyBullet bullet = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);
        //右側部分の弾を生成
        pos.x += 1.3f;
        EnemyBullet bullet2 = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);
        //左側部分の弾を生成
        pos.x -= 2.6f;
        EnemyBullet bullet3 = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);

        //弾を発射
        bullet.GetComponent<Rigidbody2D>().AddForce(power, ForceMode2D.Impulse);
        bullet2.GetComponent<Rigidbody2D>().AddForce(power, ForceMode2D.Impulse);
        bullet3.GetComponent<Rigidbody2D>().AddForce(power, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.3f);
        yield return null;
    }

    private IEnumerator _GenerateCanon()
    {
        //左か右かを決めるための1か2を生成
        int rnd_x = Random.Range(1, 3);
        //y座標設定のための1から5の乱数を生成
        int rnd_y = Random.Range(1, 6);

        //遠隔砲台の生成位置を調整 rnd_xが1なら左側，2なら右側に生成
        //弾の発射，砲台の削除は砲台側で制御する．
        if(rnd_x == 1)
        {
            Vector3 pos = new Vector3(transform.position.x - 4.0f, transform.position.y - (1.0f * rnd_y) , _beamPrefab.transform.position.z);
            Instantiate(_generateEffectPrefab, pos, Quaternion.Euler(0, 0, 270));
            Enemy_Diamond_LeftCanon canon = Instantiate(_leftCanonPrefab, pos, Quaternion.Euler(0, 0, 270));
        }
        else
        {
            Vector3 pos = new Vector3(transform.position.x + 4.0f, transform.position.y - (1.0f * rnd_y) , _beamPrefab.transform.position.z);
            Instantiate(_generateEffectPrefab, pos, Quaternion.Euler(0, 0, 90));
            Enemy_Diamond_RightCanon canon = Instantiate(_rightCanonPrefab, pos, Quaternion.Euler(0, 0, 90));
        }

        yield return null;
    }

    private IEnumerator _BeamShoot()
    {
        //下向きに進む力
        Vector3 power = new Vector3(0, -5.0f, 0);

        Vector3 power_l = new Vector3(5.0f, -5.0f, 0);
        Vector3 power_r = new Vector3(-5.0f, -5.0f, 0);

        //座標設定
        Vector3 pos_l = new Vector3(transform.position.x - 2.0f, transform.position.y, transform.position.z);
        Vector3 pos_r = new Vector3(transform.position.x + 2.0f, transform.position.y, transform.position.z);

        //砲台登場演出
        GameObject leftflash = Instantiate(_flashEffectPrefab, pos_l + new Vector3(0.5f, 0, 0), Quaternion.identity);
        GameObject rightflash = Instantiate(_flashEffectPrefab, pos_r + new Vector3(0.5f, 0, 0), Quaternion.identity);

        yield return new WaitForSeconds(0.2f);

        //砲台を生成
        GameObject leftbit = Instantiate(_bitPrefab, pos_l, Quaternion.Euler(0, 0, 180));
        GameObject rightbit = Instantiate(_bitPrefab, pos_r, Quaternion.Euler(0, 0, 180));

        yield return new WaitForSeconds(1.5f);

        for(int i = 0; i < 3; ++i)
        {
            //ビームを生成
            EnemyBullet beam_l = Instantiate(_beamPrefab, pos_l, Quaternion.identity);
            EnemyBullet beam_c = Instantiate(_beamPrefab, transform.position, Quaternion.identity);
            EnemyBullet beam_r = Instantiate(_beamPrefab, pos_r, Quaternion.identity);

            //ビームを発射
            beam_l.GetComponent<Rigidbody2D>().AddForce(power, ForceMode2D.Impulse);
            beam_c.GetComponent<Rigidbody2D>().AddForce(power, ForceMode2D.Impulse);
            beam_r.GetComponent<Rigidbody2D>().AddForce(power, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.3f);
        }

        GameObject leftflash2 = Instantiate(_flashEffectPrefab, pos_l + new Vector3(0.5f, 0, 0), Quaternion.identity);
        GameObject rightflash2 = Instantiate(_flashEffectPrefab, pos_r + new Vector3(0.5f, 0, 0), Quaternion.identity);
        Destroy(leftbit.gameObject);
        Destroy(rightbit.gameObject);
        GameObject leftflash3 = Instantiate(_flashEffectPrefab, pos_l + new Vector3(0.5f, 0, 0), Quaternion.identity);
        GameObject rightflash3 = Instantiate(_flashEffectPrefab, pos_r + new Vector3(0.5f, 0, 0), Quaternion.identity);

        yield return new WaitForSeconds(0.2f);

        //砲台を生成
        GameObject leftbit2 = Instantiate(_bitPrefab, pos_l, Quaternion.Euler(0, 0, 225));
        GameObject rightbit2 = Instantiate(_bitPrefab, pos_r, Quaternion.Euler(0, 0, 135));

        yield return new WaitForSeconds(1.5f);

        for(int i = 0; i < 3; ++i)
        {
            //ビームを生成
            EnemyBullet beam_l2 = Instantiate(_beamPrefab, pos_l, Quaternion.Euler(0,0,45));
            EnemyBullet beam_c2 = Instantiate(_beamPrefab, transform.position, Quaternion.identity);
            EnemyBullet beam_r2 = Instantiate(_beamPrefab, pos_r, Quaternion.Euler(0,0,135));

            //ビームを発射
            beam_l2.GetComponent<Rigidbody2D>().AddForce(power_l, ForceMode2D.Impulse);
            beam_c2.GetComponent<Rigidbody2D>().AddForce(power, ForceMode2D.Impulse);
            beam_r2.GetComponent<Rigidbody2D>().AddForce(power_r, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.3f);
        }

        GameObject leftflash4 = Instantiate(_flashEffectPrefab, pos_l + new Vector3(0.5f, 0, 0), Quaternion.identity);
        GameObject rightflash4 = Instantiate(_flashEffectPrefab, pos_r + new Vector3(0.5f, 0, 0), Quaternion.identity);
        Destroy(leftbit2.gameObject);
        Destroy(rightbit2.gameObject);
        GameObject leftflash5 = Instantiate(_flashEffectPrefab, pos_l + new Vector3(0.5f, 0, 0), Quaternion.identity);
        GameObject rightflash5 = Instantiate(_flashEffectPrefab, pos_r + new Vector3(0.5f, 0, 0), Quaternion.identity);

        yield return null;
    }

    private IEnumerator _AimingRapidShoot()
    {
        //弾を敵の位置に生成
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);
        EnemyBullet bullet = Instantiate(_reduceBulletPrefab, pos, Quaternion.identity);

        //プレイヤーの位置を取得し，敵の位置と減算することで敵から自機へ向かうベクトルを生成
        Vector3 target = _player.transform.position;
        Vector3 power = target - this.transform.position;

        //生成した弾を点滅させる
        yield return StartCoroutine(_Flashing(bullet));

        //弾を発射 正規化した位置ベクトルに乗算して速さを調整．
        bullet.GetComponent<Rigidbody2D>().AddForce(power.normalized * 8.0f, ForceMode2D.Impulse);

        yield return null;
    }

    private IEnumerator _Flashing(EnemyBullet bullet)
    {
        var v = bullet.GetComponent<SpriteRenderer>();

        //0.2秒おきにSpriteRendererの有効・無効を切り替えて点滅させる
        for(int i = 0; i < 15; i++)
        {
            if(i % 2 == 1)
                v.enabled = false;
            else
                v.enabled = true;

            yield return new WaitForSeconds(0.2f);
        }

        yield return null;
    }

    private IEnumerator _RandomShoot()
    {
        //攻撃開始時点での敵の位置を保存
        Vector3 startPos = this.transform.position;

        //画面中央に移動
        yield return StartCoroutine(_Move(0));
        yield return new WaitForSeconds(1.5f);

        //乱射
        for(int i=0; i < 20; ++i)
        {
            //自機の位置に弾を生成
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);
            EnemyBullet bullet = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);

            Vector3 power = new Vector3(2.0f, -5.0f, 0);

            //ランダムな方向
            var dir = Random.insideUnitCircle.normalized;

            //弾を発射
            bullet.GetComponent<Rigidbody2D>().AddForce(power * dir, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.2f);
        }

        //元の位置に戻る
        yield return StartCoroutine(_Move(1));
        yield return new WaitForSeconds(1);

        yield return null;
    }

    private IEnumerator _Move(int n)
    {
        //進行方向管理用
        float mode = 1.0f;

        //引数が0だったら前進，1だったら後退．
        if(n == 1)
            mode *= -1.0f;

        for(int i = 0; i <= 25; ++i)
        {
            this.transform.position +=  new Vector3(0, -0.1f * mode, 0);
            yield return null;
        }

        yield break;
    }

    private IEnumerator _FanShoot()
    {
        //各方向 c(直進) r(右) l(左)へ向かう力
        Vector3 power_c = new Vector3(0, -5.0f, 0);
        Vector3 power_r = new Vector3(4.0f, -5.0f, 0);
        Vector3 power_l = new Vector3(-4.0f, -5.0f, 0);

        Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);

        //中心部分の弾を生成
        EnemyBullet bullet_c1 = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);
        EnemyBullet bullet_r1 = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);
        EnemyBullet bullet_l1 = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);

        //右側部分の弾を生成
        pos.x += 1.2f;
        EnemyBullet bullet_c2 = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);
        EnemyBullet bullet_r2 = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);
        EnemyBullet bullet_l2 = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);

        //左側部分の弾を生成
        pos.x -= 2.4f;
        EnemyBullet bullet_c3 = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);
        EnemyBullet bullet_r3 = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);
        EnemyBullet bullet_l3 = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);

        //各部分の直進する弾を発射
        bullet_c1.GetComponent<Rigidbody2D>().AddForce(power_c, ForceMode2D.Impulse);
        bullet_c2.GetComponent<Rigidbody2D>().AddForce(power_c, ForceMode2D.Impulse);
        bullet_c3.GetComponent<Rigidbody2D>().AddForce(power_c, ForceMode2D.Impulse);

        //各部分の右側へ向かう弾を発射
        bullet_r1.GetComponent<Rigidbody2D>().AddForce(power_r, ForceMode2D.Impulse);
        bullet_r2.GetComponent<Rigidbody2D>().AddForce(power_r, ForceMode2D.Impulse);
        bullet_r3.GetComponent<Rigidbody2D>().AddForce(power_r, ForceMode2D.Impulse);

        //各部分の左側へ向かう弾を発射
        bullet_l1.GetComponent<Rigidbody2D>().AddForce(power_l, ForceMode2D.Impulse);
        bullet_l2.GetComponent<Rigidbody2D>().AddForce(power_l, ForceMode2D.Impulse);
        bullet_l3.GetComponent<Rigidbody2D>().AddForce(power_l, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1);
        yield return null;
    }

    private IEnumerator _ChangeSpeedShoot()
    {
        for(int i = 0; i < 10; ++i)
        {
            //5から9の乱数を生成
            int rnd = Random.Range(5,10);

            //弾を敵の位置に生成
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);
            EnemyBullet bullet = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);

            //プレイヤーの位置を取得し，敵の位置と減算することで敵から自機へ向かうベクトルを生成
            Vector3 target = _player.transform.position;
            Vector3 power = target - this.transform.position;

            //弾を発射 正規化した位置ベクトルに乱数を乗算して速さを調整．
            bullet.GetComponent<Rigidbody2D>().AddForce(power.normalized * rnd, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.4f);
        }

        yield return null;
    }
}
