using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy_Hexagon : Enemy
{
    private Player _player;

    private List<System.Func<IEnumerator>> _attackFactories;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.CurrentEnemy = this;

        _attackFactories = new List<System.Func<IEnumerator>>();

        _AddAttacks();

        StartCoroutine(_AwakeRandomAttacks());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void _AddAttacks()
    {
        //_attackFactories.Add(() => _[MethodName]()); で攻撃の種類の追加
        _attackFactories.Add(() => _SingleShot1());
        _attackFactories.Add(() => _SingleShot2());
        //_attackFactories......
    }

    private IEnumerator _AwakeRandomAttacks()
    {
        while(true)
        {
            int random = Random.Range(0,_attackFactories.Count);
            Debug.Log(random);
            yield return StartCoroutine(_attackFactories[random]());
        }
    }

    //各攻撃は、IEnumerator型のメソッドとして定義する。"yield return new WaitForSeconds(second)"のsecondの秒数だけこのメソッドが実行される。
    //射撃処理はその上に書く。その中でyield return StartCoroutine(_IENUMERATORMETHOD());とすることで、IEnumerator内での待機ができる。
    private IEnumerator _SingleShot1()
    {
        Debug.Log("Awake SingleShot1");

        yield return new WaitForSeconds(1);

        Debug.Log("Finish SingleShot1");
    }

    private IEnumerator _SingleShot2()
    {
        Debug.Log("Awake SingleShot2");

        yield return new WaitForSeconds(1);

        Debug.Log("Finish SingleShot2");
    }
}
