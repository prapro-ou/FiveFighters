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
                    yield return StartCoroutine(_ShootCircleBullet());
                    CurrentState = DiamondState.Wait;
                    Debug.Log("Attack:" + CurrentState);
                    break;
                }
                case DiamondState.Attack2:
                //分身設置
                {
                    Debug.Log("Attack:" + CurrentState);
                    break;
                }
                case DiamondState.Attack3:
                //ランダム方向へのビーム
                {
                    Debug.Log("Attack:" + CurrentState);
                    break;
                }
                case DiamondState.Attack4:
                //かすり値減少弾
                {
                    Debug.Log("Attack:" + CurrentState);
                    break;
                }
                case DiamondState.Attack5:
                //徹甲榴弾
                {
                    Debug.Log("Attack:" + CurrentState);
                    break;
                }
                case DiamondState.Attack6:
                //3つの頂点から扇形に射撃
                {
                    Debug.Log("Attack:" + CurrentState);
                    break;
                }
                case DiamondState.Attack7:
                //弾速が違う弾が混ざった射撃
                {
                    Debug.Log("Attack:" + CurrentState);
                    break;
                }
            }
        }
    }

    private IEnumerator _ShootCircleBullet()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);
        EnemyBullet bullet = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);

        //弾に力を与える処理など

        yield return null;
    }
}
