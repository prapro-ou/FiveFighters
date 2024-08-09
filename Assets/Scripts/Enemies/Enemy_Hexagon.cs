using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum HexagonState
{
    Wait,
    Attack1,
    Attack2
    // Attack3,
    // Attack4,
    // Attack5,
    // Attack6
}

public class Enemy_Hexagon : Enemy
{
    private Player _player;

    [SerializeField]
    private EnemyBullet _circleBulletPrefab; 

    [SerializeField]
    private EnemyBullet _shrinkHexagonBulletPrefab; 

    [SerializeField]
    private GameObject _hexagonWallPrefab;

    [SerializeField]
    private EnemyBullet _hexagonLaserPrefab;

    [SerializeField]
    private EnemyBullet _hexagonLaserCautionPrefab;

    [SerializeField]
    private GameObject _hexagonCautionEffectPrefab;

    private HexagonState _currentState;

    public HexagonState CurrentState
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
        CurrentState = HexagonState.Wait;
        NumberOfAttacks = System.Enum.GetValues(typeof(HexagonState)).Length - 1;
        // Debug.Log("NumberOfAttacks: " + NumberOfAttacks);

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
                case HexagonState.Wait:
                {
                    int random = Random.Range(0, _remainingAttacks.Count);

                    Debug.Log($"EnemyAttack: {_remainingAttacks[random]}");
                    CurrentState = (HexagonState)System.Enum.GetValues(typeof(HexagonState)).GetValue(_remainingAttacks[random]);
                    _remainingAttacks.Remove(_remainingAttacks[random]);

                    if(_remainingAttacks.Count == 0)
                    {
                        _remainingAttacks = Enumerable.Range(1, NumberOfAttacks).ToList();
                    }

                    yield return new WaitForSeconds(AttackCooltime);
                    break;
                }
                case HexagonState.Attack1:
                {
                    yield return StartCoroutine(_ShrinkHexagon());
                    CurrentState = HexagonState.Wait;
                    break;
                }
                case HexagonState.Attack2:
                {
                    yield return StartCoroutine(_HexagonLaser());
                    CurrentState = HexagonState.Wait;
                    break;
                }
                // case HexagonState.Attack3:
                // {
                //     yield return StartCoroutine(_BurstShoot());
                //     CurrentState = HexagonState.Wait;
                //     break;
                // }
                // case HexagonState.Attack4:
                // {
                //     yield return StartCoroutine(_BurstShoot());
                //     CurrentState = HexagonState.Wait;
                //     break;
                // }
                // case HexagonState.Attack5:
                // {
                //     yield return StartCoroutine(_BurstShoot());
                //     CurrentState = HexagonState.Wait;
                //     break;
                // }
                // case HexagonState.Attack6:
                // {
                //     yield return StartCoroutine(_BurstShoot());
                //     CurrentState = HexagonState.Wait;
                //     break;
                // }
            }
        }
    }

    //出現したときに、StartAttackingよりも先に実行されるコルーチン。これが終了してからStartAttackingメソッドが実行される。
    //登場したときの演出をこのメソッドに記述しよう。アニメーション自体はスクリプトで書かず、アニメーター(アニメーション)コンポーネントで実装することもできる。
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

        yield return new WaitForSeconds(2); //Sample

        Destroy(this.gameObject);

        yield return new WaitForSeconds(1); //Sample
    }

    //以下二つの攻撃行動はサンプル。
    //各攻撃行動は、IEnumerator型のメソッドとして定義する。"yield return new WaitForSeconds(second)"のsecondの秒数だけこのメソッドが実行される。
    //攻撃処理はその上に書く。メソッド内で”yield return StartCoroutine(_METHOD());”とすることで、そのメソッドが終わるまで待機ができる。
    private IEnumerator _ShrinkHexagon()
    {
        EnemyBullet bullet;
        Quaternion rotation;

        Debug.Log("Start ShrinkHexagon");

        //中央移動

        //回転

        //パーティクル

        for(int i = 0; i < 3; i++)
        {
            //弾
            rotation = Quaternion.Euler(0, 0, Random.Range(0, 6) * 60);
            bullet = Instantiate(_shrinkHexagonBulletPrefab, Vector3.zero, rotation);
            yield return new WaitForSeconds(1.5f);
        }

        yield return new WaitForSeconds(2f);

        for(int i = 0; i < 3; i++)
        {
            //弾
            rotation = Quaternion.Euler(0, 0, Random.Range(0, 6) * 60);
            bullet = Instantiate(_shrinkHexagonBulletPrefab, Vector3.zero, rotation);
            yield return new WaitForSeconds(1.5f);
        }
        
        yield return new WaitForSeconds(3f);
    
        Debug.Log("Finish ShringHexagon");
    }

    private IEnumerator _HexagonLaser()
    {
        Debug.Log("Start HexagonLaser");

        //Playerを親にエフェクト

        //中心にエフェクト

        //中心にPlayerをテレポート
        _player.transform.position = Vector3.zero;

        GameObject wall = _SummonWall();

        yield return new WaitForSeconds(1f);

        Transform[] pointTransforms = wall.GetComponentsInChildren<Transform>();
        
        Quaternion[] rotations = {Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 60), Quaternion.Euler(0, 0, 120)};

        for(int i = 0; i < 3; i++)
        {
            Vector3 position = pointTransforms[Random.Range(0,6)].position;
            Quaternion rotation = rotations[Random.Range(0,3)];
            StartCoroutine(_Laser(position, rotation));
            yield return new WaitForSeconds(2f);
        }

        for(int j = 0; j < 2; j++)
        {
            int rp1 = Random.Range(0,6);
            int rp2;
            int rr1 = Random.Range(0,3);;
            int rr2;
            
            while(true)
            {
                rp2 = Random.Range(0,6);
                if(rp1 != rp2)
                {
                    break;
                }
            }

            while(true)
            {
                rr2 = Random.Range(0,3);
                if(rr1 != rr2)
                {
                    break;
                }
            }

            StartCoroutine(_Laser(pointTransforms[rp1].position, rotations[rr1]));
            StartCoroutine(_Laser(pointTransforms[rp2].position, rotations[rr2]));
            
            yield return new WaitForSeconds(3f);
        }

        yield return new WaitForSeconds(1f);

        Destroy(wall);

        Debug.Log("Finish HexagonLaser");
    }

    private IEnumerator _ShootCircleBullet()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);
        EnemyBullet bullet = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);

        //弾に力を与える処理など

        yield return null;
    }

    private GameObject _SummonWall()
    {
        GameObject wall = Instantiate(_hexagonWallPrefab, Vector3.zero, Quaternion.identity);

        return wall;
    }

    private IEnumerator _Laser(Vector3 position, Quaternion rotation)
    {
        Instantiate(_hexagonCautionEffectPrefab, position, rotation);
        EnemyBullet laserCaution = Instantiate(_hexagonLaserCautionPrefab, position, rotation);

        yield return new WaitForSeconds(1f);

        EnemyBullet laser = Instantiate(_hexagonLaserPrefab, position, rotation);

        yield return new WaitForSeconds(1.5f);

        Destroy(laserCaution.gameObject);
        Destroy(laser.gameObject);
    }
}
