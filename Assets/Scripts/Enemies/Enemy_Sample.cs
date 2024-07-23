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

    private HexagonState _currentState;

    public HexagonState CurrentState
    {
        get {return _currentState;}
        set {_currentState = value;}
    }

    [SerializeField]
    private int _numberOfAttacks;

    public int NumberOfAttacks
    {
        get {return _numberOfAttacks;}
        set {_numberOfAttacks = value;}
    }

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
        _currentState = HexagonState.Wait;

        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.CurrentEnemy = this;

        StartCoroutine(_AwakeRandomAttack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator _AwakeRandomAttack()
    {
        while(true)
        {
            switch(CurrentState)
            {
                case SampleState.Wait:
                {
                    int random = Random.Range(1, NumberOfAttacks);
                    CurrentState = (SampleState)System.Enum.GetValues(typeof(SampleState)).GetValue(random);
                    yield return new WaitForSeconds(AttackCooltime);
                    break;
                }
                case SampleState.Attack1:
                {
                    yield return StartCoroutine(_SingleShoot());
                    CurrentState = SampleState.Wait;
                    break;
                }
                case HexagonState.Attack2:
                {
                    yield return StartCoroutine(_BurstShoot());
                    CurrentState = SampleState.Wait;
                    break;
                }
            }
        }
    }

    //以下二つの攻撃行動はサンプル。
    //各攻撃行動は、IEnumerator型のメソッドとして定義する。"yield return new WaitForSeconds(second)"のsecondの秒数だけこのメソッドが実行される。
    //攻撃処理はその上に書く。メソッド内で”yield return StartCoroutine(_METHOD());”とすることで、そのメソッドが終わるまで待機ができる。
    private IEnumerator _SingleShoot()
    {
        Debug.Log("Start SingleShot");

        //ここでIEnumerator型のメソッドをyield return StartCoroutine(...)で呼ぶ。
        yield return StartCoroutine(_ShootCircleBullet());

        yield return new WaitForSeconds(1);

        Debug.Log("Finish SingleShot");
    }

    private IEnumerator _BurstShoot()
    {
        Debug.Log("Start TripleShot");

        yield return StartCoroutine(_ShootCircleBullet());

        yield return new WaitForSeconds(0.2f);

        yield return StartCoroutine(_ShootCircleBullet());

        yield return new WaitForSeconds(0.2f);

        yield return StartCoroutine(_ShootCircleBullet());

        yield return new WaitForSeconds(0.2f);

        yield return new WaitForSeconds(1);

        Debug.Log("Finish TripleShot");
    }

    private IEnumerator _ShootCircleBullet()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);
        EnemyBullet bullet = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);

        //弾に力を与える処理など

        yield return null;
    }
}
