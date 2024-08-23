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
    private CameraManager _cameraManager;

    private SoundManager _soundManager;

    private Player _player;

    [SerializeField]
    private EnemyBullet _circleBulletPrefab;

    [SerializeField]
    private EnemyBullet _laserPrefab;

    [SerializeField]
    private EnemyBullet _reduceBulletPrefab;

    [SerializeField]
    private EnemyBullet _diamondBullet;

    [SerializeField]
    private Enemy_Diamond_LeftCanon _leftCanonPrefab;

    [SerializeField]
    private Enemy_Diamond_RightCanon _rightCanonPrefab;

    [SerializeField]
    private Enemy_Diamond_Bit _bitPrefab;

    [SerializeField]
    private GameObject _generateEffectPrefab;

    [SerializeField]
    private GameObject _flashEffectPrefab;

    [SerializeField]
    private GameObject _explodePrefab;

    [SerializeField]
    private Enemy_Scope _scopePrefab;

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

    private Coroutine _currentCoroutine;

    public Coroutine CurrentCoroutine
    {
        get {return _currentCoroutine;}
        set {_currentCoroutine = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentState = DiamondState.Wait;
        NumberOfAttacks = System.Enum.GetValues(typeof(DiamondState)).Length - 1;

        _remainingAttacks = Enumerable.Range(1, NumberOfAttacks).ToList();

        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.CurrentEnemy = this;

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
                //3つの頂点から平行に射撃.1セットごとに移動．
                {
                    Debug.Log("Attack:" + CurrentState);
                    CurrentCoroutine = StartCoroutine(_3VerticesShoot());
                    yield return StartCoroutine(_3VerticesShoot());
                    yield return StartCoroutine(_3VerticesShoot());
                    yield return new WaitForSeconds(1.5f);
                    yield return StartCoroutine(_Move('l'));
                    yield return StartCoroutine(_3VerticesShoot());
                    yield return StartCoroutine(_3VerticesShoot());
                    yield return new WaitForSeconds(1.5f);
                    yield return StartCoroutine(_Move('r'));
                    yield return StartCoroutine(_3VerticesShoot());
                    yield return StartCoroutine(_3VerticesShoot());
                    yield return new WaitForSeconds(1.5f);
                    yield return StartCoroutine(_Move('r'));
                    yield return StartCoroutine(_3VerticesShoot());
                    yield return StartCoroutine(_3VerticesShoot());
                    yield return new WaitForSeconds(1.5f);
                    yield return StartCoroutine(_Move('l'));
                    yield return StartCoroutine(_3VerticesShoot());
                    yield return StartCoroutine(_3VerticesShoot());

                    CurrentState = DiamondState.Wait;
                    break;
                }
                case DiamondState.Attack2:
                //分身設置．時間差で射撃
                {
                    Debug.Log("Attack:" + CurrentState);
                    CurrentCoroutine = StartCoroutine(_GenerateCanon());
                    EnemyBullet shapeBullet = Instantiate(_diamondBullet, this.transform.position + new Vector3(0, -2.0f, 0), Quaternion.identity);
                    yield return CurrentCoroutine;
                    CurrentState = DiamondState.Wait;
                    break;
                }
                case DiamondState.Attack3:
                //砲台を近くに展開し，三点からビーム．角度を変えてもう一回．
                {
                    Debug.Log("Attack:" + CurrentState);
                    CurrentCoroutine = StartCoroutine(_LaserShoot());
                    yield return CurrentCoroutine;
                    CurrentState = DiamondState.Wait;
                    break;
                }
                case DiamondState.Attack4:
                //高速かすり値減少弾10発 発射時点のプレイヤーの位置に飛ぶ．
                {
                    Debug.Log("Attack:" + CurrentState);
                    CurrentCoroutine = StartCoroutine(_AimingRapidShoot());
                    yield return CurrentCoroutine;
                    CurrentState = DiamondState.Wait;
                    break;
                }
                case DiamondState.Attack5:
                //画面中央で全方位に射撃.ホーミングするビームも混入
                {
                    Debug.Log("Attack:" + CurrentState);
                    CurrentCoroutine = StartCoroutine(_RandomShoot());
                    yield return CurrentCoroutine;
                    CurrentState = DiamondState.Wait;
                    break;
                }
                case DiamondState.Attack6:
                //3つの頂点から扇形に射撃
                {
                    Debug.Log("Attack:" + CurrentState);
                    CurrentCoroutine = StartCoroutine(_FanShoot());
                    yield return CurrentCoroutine;
                    CurrentState = DiamondState.Wait;
                    break;
                }
                case DiamondState.Attack7:
                //弾速が違う弾が混ざった射撃を10発 1発ごとにホーミング
                {
                    Debug.Log("Attack:" + CurrentState);
                    CurrentCoroutine = StartCoroutine(_ChangeSpeedShoot());
                    yield return CurrentCoroutine;
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

        Instantiate(_flashEffectPrefab, new Vector3(0f, 3.0f, 0), Quaternion.identity);
        Instantiate(_flashEffectPrefab, new Vector3(0f, 3.0f, 0), Quaternion.Euler(0, 0, 90));
        Instantiate(_flashEffectPrefab, new Vector3(0f, 3.0f, 0), Quaternion.Euler(0, 0, 180));
        Instantiate(_flashEffectPrefab, new Vector3(0f, 3.0f, 0), Quaternion.Euler(0, 0, 270));

        for(int i = 0; i <= 100; ++i){
            if(i == 30)
                _PlaySound("Spawn4");

            transform.localScale = new Vector3(0.02f * i, 0.02f * i, 0.01f * i);
            transform.localEulerAngles = new Vector3(0, 0, 3.6f * i);
            yield return new WaitForSeconds(0.025f);
        }
        Vector3 startPos = _cameraManager.transform.position;
        StartCoroutine(_cameraManager.SetSizeOnCurve(2.0f));
        yield return StartCoroutine(_cameraManager.MoveToPointOnCurve(this.transform.position));
        StartCoroutine(_cameraManager.SetSizeOnCurve(5f));
        yield return StartCoroutine(_cameraManager.MoveToPointOnCurve(startPos, 0.5f));

        yield return new WaitForSeconds(3);
        yield break;
    }


    //死亡したときに、GameManagerによって実行されるコルーチン。
    //死亡したときの演出をこのメソッドに記述しよう。アニメーション自体はスクリプトで書かず、アニメーター(アニメーション)コンポーネントで実装することもできる。
    public override IEnumerator StartDeathAnimation()
    {
        Debug.Log("StartDeathAnimation");

        StopCoroutine(CurrentCoroutine);

        Color startColor = Color.white;
        Color endColor = Color.black;
        SpriteRenderer rend = this.GetComponent<SpriteRenderer>();

        for(float i = 0.0f; i < 30.0f; ++i)
        {
            rend.material.color = Color.Lerp(startColor, endColor, i / 30.0f);

            yield return new WaitForSeconds(0.1f);
        }

        _PlaySound("Explosion1");
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
        _PlaySound("NormalBullet");
        bullet.GetComponent<Rigidbody2D>().velocity = power;
        bullet2.GetComponent<Rigidbody2D>().velocity = power;
        bullet3.GetComponent<Rigidbody2D>().velocity = power;

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
            Vector3 pos = new Vector3(transform.position.x - 4.0f, transform.position.y - (1.0f * rnd_y) , _laserPrefab.transform.position.z);
            _PlaySound("Spawn3");
            Instantiate(_generateEffectPrefab, pos, Quaternion.Euler(0, 0, 270));
            Enemy_Diamond_LeftCanon canon = Instantiate(_leftCanonPrefab, pos, Quaternion.Euler(0, 0, 270));
        }
        else
        {
            Vector3 pos = new Vector3(transform.position.x + 4.0f, transform.position.y - (1.0f * rnd_y) , _laserPrefab.transform.position.z);
            _PlaySound("Spawn3");
            Instantiate(_generateEffectPrefab, pos, Quaternion.Euler(0, 0, 90));
            Enemy_Diamond_RightCanon canon = Instantiate(_rightCanonPrefab, pos, Quaternion.Euler(0, 0, 90));
        }

        yield return null;
    }

    private IEnumerator _LaserShoot()
    {
        //座標設定
        Vector3 pos_l = new Vector3(transform.position.x - 2.0f, transform.position.y, transform.position.z);
        Vector3 pos_r = new Vector3(transform.position.x + 2.0f, transform.position.y, transform.position.z);

        for(int i = 0; i < 4; ++i)
        {
            //砲台登場演出
            GameObject leftflash = Instantiate(_flashEffectPrefab, pos_l + new Vector3(0.5f, 0, 0), Quaternion.identity);
            GameObject rightflash = Instantiate(_flashEffectPrefab, pos_r + new Vector3(0.5f, 0, 0), Quaternion.identity);

            yield return new WaitForSeconds(0.2f);

            //プレイヤーの方を向けるための角度計算
            Vector3 aim_c = (_player.transform.position - transform.position).normalized;
            if(aim_c == Vector3.zero)
                aim_c = _player.transform.position - transform.position;
            float angle_c = Mathf.Atan2(aim_c.x, aim_c.y);

            Vector3 aim_l = (_player.transform.position - pos_l - new Vector3(0.9f, 0, 0)).normalized;
            if(aim_l == Vector3.zero)
                aim_l = _player.transform.position - pos_l - new Vector3(0.9f, 0, 0);
            float angle_l = Mathf.Atan2(aim_l.x, aim_l.y);

            Vector3 aim_r = (_player.transform.position - pos_r + new Vector3(0.9f, 0, 0)).normalized;
            if(aim_r == Vector3.zero)
                aim_r = _player.transform.position - pos_r + new Vector3(0.9f, 0, 0);
            float angle_r = Mathf.Atan2(aim_r.x, aim_r.y);

            //砲台生成
            _PlaySound("Spawn3");
            Enemy_Diamond_Bit leftbit = Instantiate(_bitPrefab, pos_l, Quaternion.AngleAxis(angle_l * Mathf.Rad2Deg, Vector3.back));
            Enemy_Diamond_Bit rightbit = Instantiate(_bitPrefab, pos_r, Quaternion.AngleAxis(angle_r * Mathf.Rad2Deg, Vector3.back));

            //照準表示
            Enemy_Scope scope = Instantiate(_scopePrefab, _player.transform.position, Quaternion.identity);

            yield return new WaitForSeconds(1.0f);

            Vector3 power_c = aim_c * 6.0f;
            Vector3 power_l = aim_l * 6.0f;
            Vector3 power_r = aim_r * 6.0f;

            for(int j = 0; j < 3; ++j)
            {
                //ビームを生成
                EnemyBullet laser_l = Instantiate(_laserPrefab, pos_l, Quaternion.AngleAxis(angle_l * Mathf.Rad2Deg, Vector3.back));
                EnemyBullet laser_c = Instantiate(_laserPrefab, transform.position, Quaternion.AngleAxis(angle_c * Mathf.Rad2Deg, Vector3.back));
                EnemyBullet laser_r = Instantiate(_laserPrefab, pos_r, Quaternion.AngleAxis(angle_r * Mathf.Rad2Deg, Vector3.back));

                //ビームを発射
                _PlaySound("Laser");
                laser_l.GetComponent<Rigidbody2D>().velocity = power_l;
                laser_c.GetComponent<Rigidbody2D>().velocity = power_c;
                laser_r.GetComponent<Rigidbody2D>().velocity = power_r;

                yield return new WaitForSeconds(0.4f);
            }

            //退場演出
            GameObject leftflash2 = Instantiate(_flashEffectPrefab, pos_l + new Vector3(0.5f, 0, 0), Quaternion.identity);
            GameObject rightflash2 = Instantiate(_flashEffectPrefab, pos_r + new Vector3(0.5f, 0, 0), Quaternion.identity);

            //砲台消滅
            Destroy(leftbit.gameObject);
            Destroy(rightbit.gameObject);
        }

        yield return null;
    }

    private IEnumerator _AimingRapidShoot()
    {
        //弾を敵の位置に生成
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);

        for(int i = 0; i < 10; i++)
        {
            EnemyBullet bullet = Instantiate(_reduceBulletPrefab, pos, Quaternion.identity);

            //プレイヤーの位置を取得し，敵の位置と減算することで敵から自機へ向かうベクトルを生成し，正規化
            Vector3 target = _player.transform.position;
            Vector3 power = (target - this.transform.position).normalized;

            //正規化の結果零ベクトルとなった場合，正規化しない
            if(power == Vector3.zero)
                power = (target - this.transform.position);

            //生成した弾を点滅させる(初回のみ)
            if(i == 0)
                yield return StartCoroutine(_Flashing(bullet));

            //照準を表示
            Enemy_Scope scope = Instantiate(_scopePrefab, target, Quaternion.identity);

            //弾を発射 正規化した位置ベクトルに乗算して速さを調整．
            _PlaySound("NormalBullet");
            if(bullet == null)
                break;
            else
                bullet.GetComponent<Rigidbody2D>().velocity = power.normalized * 8.0f;

            yield return new WaitForSeconds(0.4f);
        }

        yield return null;
    }

    private IEnumerator _Flashing(EnemyBullet bullet)
    {
        var sr = bullet.GetComponent<SpriteRenderer>();

        //0.2秒おきにSpriteRendererの有効・無効を切り替えて点滅させる
        for(int i = 0; i < 15; i++)
        {
            if(bullet == null)
                break;

            if(i % 2 == 1)
                sr.enabled = false;
            else
                sr.enabled = true;

            yield return new WaitForSeconds(0.2f);
        }

        yield return null;
    }

    private IEnumerator _RandomShoot()
    {
        //攻撃開始時点での敵の位置を保存
        Vector3 startPos = this.transform.position;

        //画面中央に移動
        yield return StartCoroutine(_Move('f'));
        yield return new WaitForSeconds(1.5f);

        for(int i = 0; i <= 200; ++i)
        {
            transform.localEulerAngles = new Vector3(0, 0, 3.6f * i);
            yield return new WaitForSeconds(0.005f);
        }

        Vector3[] directions = new Vector3[]
        {
            Vector3.up,
            new Vector3(1, 1, 0).normalized,
            Vector3.right,
            new Vector3(1, -1, 0).normalized,
            Vector3.down,
            new Vector3(-1, -1, 0).normalized,
            Vector3.left,
            new Vector3(-1, 1, 0)
        };

        for(int i = 0; i < 20; ++i)
        {
            _PlaySound("NormalBullet");
            foreach(Vector3 direction in directions)
            {
                float angle = Mathf.Atan2(direction.x, direction.y);
                EnemyBullet bullet = Instantiate(_circleBulletPrefab, this.transform.position, Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.back));
                bullet.GetComponent<Rigidbody2D>().velocity = direction * 6.0f;

                yield return new WaitForSeconds(0.05f);
            }

            if(i % 5 == 0)
            {
                Vector3 aim = (_player.transform.position - this.transform.position).normalized;
                //正規化の結果零ベクトルとなった場合，正規化しない．
                if(aim == Vector3.zero)
                    aim = _player.transform.position - this.transform.position;

                Enemy_Scope scope = Instantiate(_scopePrefab, _player.transform.position, Quaternion.identity);

                float angle_l = Mathf.Atan2(aim.x, aim.y);
                EnemyBullet laser = Instantiate(_laserPrefab, this.transform.position, Quaternion.AngleAxis(angle_l * Mathf.Rad2Deg, Vector3.back));

                _PlaySound("Laser");
                laser.GetComponent<Rigidbody2D>().velocity = aim * 5.0f;
            }
        }
        //元の位置に戻る
        yield return StartCoroutine(_Move('b'));
        yield return new WaitForSeconds(1);

        yield return null;
    }

    private IEnumerator _Move(int n)
    {
        //進行方向管理用
        float mode_x = 0;
        float mode_y = 0;

        int cnt = 0;

        //引数が0だったら前進，1だったら後退．
        switch(n)
        {
            case 'f':
                mode_x = 0;
                mode_y = -1;
                cnt = 25;
                break;
            case 'r':
                mode_x = 1;
                mode_y = 0;
                cnt = 15;
                break;
            case 'b':
                mode_x = 0;
                mode_y = 1;
                cnt = 25;
                break;
            case 'l':
                mode_x = -1;
                mode_y = 0;
                cnt =15;
                break;
        }

        for(int i = 0; i <= cnt; ++i)
        {
            this.transform.position +=  new Vector3(0.1f * mode_x, 0.1f * mode_y, 0);
            yield return null;
        }

        yield break;
    }

    private IEnumerator _FanShoot()
    {
        for(int i = 0; i < 3; ++i){
            //各方向 c(直進) r(右) l(左)へ向かう力
            Vector3 power_c = new Vector3(0, -5.0f, 0);
            Vector3 power_r = new Vector3(4.0f - 0.7f * i, -5.0f, 0);
            Vector3 power_l = new Vector3(-4.0f + 0.7f * i , -5.0f, 0);

            Vector3 pos = transform.position;

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
            _PlaySound("NormalBullet");
            bullet_c1.GetComponent<Rigidbody2D>().velocity = power_c;
            bullet_c2.GetComponent<Rigidbody2D>().velocity = power_c;
            bullet_c3.GetComponent<Rigidbody2D>().velocity = power_c;

            //各部分の右側へ向かう弾を発射
            _PlaySound("NormalBullet");
            bullet_r1.GetComponent<Rigidbody2D>().velocity = power_r;
            bullet_r2.GetComponent<Rigidbody2D>().velocity = power_r;
            bullet_r3.GetComponent<Rigidbody2D>().velocity = power_r;

            //各部分の左側へ向かう弾を発射
            _PlaySound("NormalBullet");
            bullet_l1.GetComponent<Rigidbody2D>().velocity = power_l;
            bullet_l2.GetComponent<Rigidbody2D>().velocity = power_l;
            bullet_l3.GetComponent<Rigidbody2D>().velocity = power_l;

            yield return new WaitForSeconds(1);
        }
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
            Vector3 power = (target - this.transform.position).normalized;
            if(power == Vector3.zero)
                power = target - this.transform.position;

            //照準を表示
            Enemy_Scope scope = Instantiate(_scopePrefab, target, Quaternion.identity);

            //弾を発射 正規化した位置ベクトルに乱数を乗算して速さを調整．
            _PlaySound("NormalBullet");
            bullet.GetComponent<Rigidbody2D>().velocity = power * rnd;

            yield return new WaitForSeconds(0.4f);
        }

        yield return null;
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
