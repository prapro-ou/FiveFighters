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
                    CurrentState = DiamondState.Wait;
                    break;
                }
                case DiamondState.Attack3:
                //ランダム方向へのビーム
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
                //弾速が違う弾が混ざった射撃
                {
                    Debug.Log("Attack:" + CurrentState);
                    CurrentState = DiamondState.Wait;
                    break;
                }
            }
        }
    }

    private IEnumerator _3VerticesShoot()
    {
        //直進させるための力
        Vector3 power = new Vector3(0, -7.0f, 0);
        //自機(敵)の位置を取得
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);
        //中心部分の弾を生成
        EnemyBullet bullet = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);
        //右側部分の弾を生成
        pos.x += 1.2f;
        EnemyBullet bullet2 = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);
        //左側部分の弾を生成
        pos.x -= 2.4f;
        EnemyBullet bullet3 = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);

        //弾を発射
        bullet.GetComponent<Rigidbody2D>().AddForce(power, ForceMode2D.Impulse);
        bullet2.GetComponent<Rigidbody2D>().AddForce(power, ForceMode2D.Impulse);
        bullet3.GetComponent<Rigidbody2D>().AddForce(power, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.3f);
        yield return null;
    }

    private IEnumerator _BeamShoot()
    {
        //適当な力
        Vector3 power = new Vector3(3.0f, -7.0f, 0);

       //ランダムな角度
        var dir = Random.insideUnitCircle.normalized;

        //敵の位置に弾を生成
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, _beamPrefab.transform.position.z);
        EnemyBullet beam = Instantiate(_beamPrefab, pos, Quaternion.Euler(dir));

        //弾を発射
        beam.GetComponent<Rigidbody2D>().AddForce(power * dir, ForceMode2D.Impulse);

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

        //弾を発射 正規化した位置ベクトルに乗算して速さを調整．
        bullet.GetComponent<Rigidbody2D>().AddForce(power.normalized * 12.0f, ForceMode2D.Impulse);

        yield return null;
    }

    private IEnumerator _RandomShoot()
    {
        //攻撃開始時点での敵の位置を保存
        Vector3 startPos = this.transform.position;

        //画面中央に移動．今のままだと瞬間移動する．
        this.transform.position += new Vector3(0,-2.5f,0);

        yield return new WaitForSeconds(1.5f);

        //乱射
        for(int i=0;i<20;++i)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);
            EnemyBullet bullet = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);
            Vector3 power = new Vector3(3.0f, -7.0f, 0);
            var dir = Random.insideUnitCircle.normalized;
            bullet.GetComponent<Rigidbody2D>().AddForce(power * dir, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1);

        //元の位置に戻る
        this.transform.position = startPos;

        yield return null;
    }

    private IEnumerator _FanShoot()
    {
        //各方向 c(直進) r(右) l(左)へ向かう力
        Vector3 power_c = new Vector3(0, -7.0f, 0);
        Vector3 power_r = new Vector3(2.0f, -7.0f, 0);
        Vector3 power_l = new Vector3(-2.0f, -7.0f, 0);

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
}
