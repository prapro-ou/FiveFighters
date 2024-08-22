using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum SampleState
{
    Wait,
    Attack1,
    Attack2
}

public class Enemy_Sample : Enemy
{
    private Player _player;

    [SerializeField]
    private EnemyBullet _circleBulletPrefab; 

    private SampleState _currentState;

    public SampleState CurrentState
    {
        get {return _currentState;}
        set {_currentState = value;}
    }

    private Coroutine _currentCoroutine;

    public Coroutine CurrentCoroutine
    {
        get {return _currentCoroutine;}
        set {_currentCoroutine = value;}
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
        CurrentState = SampleState.Wait;
        NumberOfAttacks = System.Enum.GetValues(typeof(SampleState)).Length - 1;
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
                case SampleState.Wait:
                {
                    int random = Random.Range(0, _remainingAttacks.Count);

                    Debug.Log($"EnemyAttack: {_remainingAttacks[random]}");
                    CurrentState = (SampleState)System.Enum.GetValues(typeof(SampleState)).GetValue(_remainingAttacks[random]);
                    _remainingAttacks.Remove(_remainingAttacks[random]);

                    if(_remainingAttacks.Count == 0)
                    {
                        _remainingAttacks = Enumerable.Range(1, NumberOfAttacks).ToList();
                    }

                    yield return new WaitForSeconds(AttackCooltime);
                    break;
                }
                case SampleState.Attack1:
                {
                    CurrentCoroutine = StartCoroutine(_SingleShoot());
                    yield return CurrentCoroutine;
                    CurrentState = SampleState.Wait;
                    break;
                }
                case SampleState.Attack2:
                {
                    CurrentCoroutine = StartCoroutine(_BurstShoot());
                    yield return CurrentCoroutine;
                    CurrentState = SampleState.Wait;
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

        yield return new WaitForSeconds(5); //Sample
    }

    //死亡したときに、GameManagerによって実行されるコルーチン。
    //死亡したときの演出をこのメソッドに記述しよう。アニメーション自体はスクリプトで書かず、アニメーター(アニメーション)コンポーネントで実装することもできる。
    public override IEnumerator StartDeathAnimation()
    {
        Debug.Log("StartDeathAnimation");

        StopCoroutine(CurrentCoroutine);

        yield return new WaitForSeconds(2); //Sample

        Destroy(this.gameObject);

        yield return new WaitForSeconds(1); //Sample
    }
    
    //以下二つの攻撃行動はサンプル。
    //各攻撃行動は、IEnumerator型のメソッドとして定義する。"yield return new WaitForSeconds(second)"のsecondの秒数だけこのメソッドが実行される。
    //攻撃処理はその上に書く。メソッド内で”yield return StartCoroutine(_METHOD());”とすることで、そのメソッドが終わるまで待機ができる。
    private IEnumerator _SingleShoot()
    {
        // Debug.Log("Start SingleShot");

        //ここでIEnumerator型のメソッドをyield return StartCoroutine(...)で呼ぶ。
        yield return StartCoroutine(_ShootCircleBullet());

        yield return new WaitForSeconds(1);

        // Debug.Log("Finish SingleShot");
    }

    private IEnumerator _BurstShoot()
    {
        // Debug.Log("Start TripleShot");

        yield return StartCoroutine(_ShootCircleBullet());

        yield return new WaitForSeconds(0.2f);

        yield return StartCoroutine(_ShootCircleBullet());

        yield return new WaitForSeconds(0.2f);

        yield return StartCoroutine(_ShootCircleBullet());

        yield return new WaitForSeconds(0.2f);

        yield return new WaitForSeconds(1);

        // Debug.Log("Finish TripleShot");
    }

    private IEnumerator _ShootCircleBullet()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);
        EnemyBullet bullet = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);

        //弾に力を与える処理など

        yield return null;
    }
}
