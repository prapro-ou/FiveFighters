using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum StarState
{
    Wait,
    Attack1,
    Attack2,
    Attack3
}

public class Enemy_Star : Enemy
{
    private Player _player;

    [SerializeField]
    private EnemyBullet _circleBulletPrefab;

    private StarState _currentState;

    public StarState CurrentState
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

    private bool end;

    // Start is called before the first frame update
    void Start()
    {
        CurrentState = StarState.Wait;
        NumberOfAttacks = System.Enum.GetValues(typeof(StarState)).Length - 1;

        _remainingAttacks = Enumerable.Range(1, NumberOfAttacks).ToList();

        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.CurrentEnemy = this;

        end = false;
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
                case StarState.Wait:
                {
                    int random = Random.Range(0, _remainingAttacks.Count);

                    Debug.Log(_remainingAttacks[random]);
                    CurrentState = (StarState)System.Enum.GetValues(typeof(StarState)).GetValue(_remainingAttacks[random]);
                    _remainingAttacks.Remove(_remainingAttacks[random]);

                    if(_remainingAttacks.Count == 0)
                    {
                        _remainingAttacks = Enumerable.Range(1, NumberOfAttacks).ToList();
                    }

                    yield return new WaitForSeconds(AttackCooltime);
                    break;
                }
                case StarState.Attack1:
                //動きが反転するランダム弾
                {
                    Debug.Log("Attack:" + CurrentState);
                    yield return StartCoroutine(_random_back());
                    CurrentState = StarState.Wait;
                    break;
                }
                case StarState.Attack2:
                //ランダム弾＋追尾弾
                {
                    Debug.Log("Attack:" + CurrentState);
                    yield return StartCoroutine(_random_aiming());
                    CurrentState = StarState.Wait;
                    break;
                }

                case StarState.Attack3:
                //斜めからの流星弾
                {
                    Debug.Log("Attack:" + CurrentState);
                    for(int i=0;i<3;i++){ 
                        yield return StartCoroutine(_commet_right());
                        yield return StartCoroutine(_commet_left());
                    }

                    yield return new WaitForSeconds(1);
                    CurrentState = StarState.Wait;
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

        yield return new WaitForSeconds(0.5f); //Sample

        Destroy(this.gameObject);

        yield return new WaitForSeconds(1); //Sample
    }

    private IEnumerator _random_back()
    {
       for(int i = 0; i < 150; i++){
            StartCoroutine(_random_back_ballet());
            yield return new WaitForSeconds(0.2f);
       }
       yield return new WaitForSeconds(0.5f);

       yield return null;
    }

    private IEnumerator _random_back_ballet()
    {
        int rnd_x = Random.Range(0, 2);
        int rnd_y = Random.Range(0, 2);

        //自機の位置に弾を生成
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);
        EnemyBullet bullet = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);

        Vector3 power;

        if(rnd_x == 0)
            {
            if(rnd_y == 0)
                power = new Vector3(Random.Range(0, 8), -5.0f, 0);
            else
                power = new Vector3(Random.Range(0, 8), 5.0f, 0);
            }
            else
            {
            if(rnd_y == 0)
                power = new Vector3(Random.Range(-8, 0), -5.0f, 0);
            else
                power = new Vector3(Random.Range(-8, 0), 5.0f, 0);
            }

            //弾を発射
        bullet.GetComponent<Rigidbody2D>().velocity = power;

        //Addforceによって反転を行う

        bullet.GetComponent<Rigidbody2D>().AddForce(power * 0.1f);

        yield return null;
    }

    private IEnumerator _random_aiming()
    {
        end = false;
        StartCoroutine(_AimingShoot());
        yield return StartCoroutine(_RandomShoot());

        yield return null;
    }

    private IEnumerator _AimingShoot()
    {
        //弾を敵の位置に生成
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);

        for(int i = 0; i < 30 ; i++)
        {
            EnemyBullet bullet = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);

            //プレイヤーの位置を取得し，敵の位置と減算することで敵から自機へ向かうベクトルを生成
            Vector3 target = _player.transform.position;
            Vector3 power = target - this.transform.position;

            //弾を発射 正規化した位置ベクトルに乗算して速さを調整．
            bullet.GetComponent<Rigidbody2D>().velocity = power.normalized * 4.0f;

            yield return new WaitForSeconds(0.5f);
        }

        end = true;

        yield return null;
    }

    private IEnumerator _RandomShoot()
    {

        //乱射
        for(int i = 1; i < 3000; ++i)
        {
            int rnd_x = Random.Range(0, 2);
            int rnd_y = Random.Range(0, 2);

            //自機の位置に弾を生成
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);
            EnemyBullet bullet = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);

            Vector3 power;

            if(rnd_x == 0)
            {
                if(rnd_y == 0)
                    power = new Vector3(Random.Range(0, 8), -5.0f, 0);
                else
                    power = new Vector3(Random.Range(0, 8), 5.0f, 0);
            }
            else
            {
                if(rnd_y == 0)
                    power = new Vector3(Random.Range(-8, 0), -5.0f, 0);
                else
                    power = new Vector3(Random.Range(-8, 0), 5.0f, 0);
            }

            //弾を発射
            bullet.GetComponent<Rigidbody2D>().velocity = power;

            yield return new WaitForSeconds(0.2f);

            if(end) yield break;
        }



    }

    private IEnumerator _commet_right()
    {

        //乱射
        for(int i = 1; i < 25; ++i)
        {
            int rnd_x = Random.Range(0, 9);
            int rnd_y = Random.Range(0, 12);

            //斜め上に
            Vector3 pos = new Vector3(2.4f + rnd_x * 0.1f, 3.0f + rnd_y * 0.1f, _circleBulletPrefab.transform.position.z);
            EnemyBullet bullet = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);

            Vector3 power;

            
            power = new Vector3(-4.0f, -4.5f, 0);

            //弾を発射
            bullet.GetComponent<Rigidbody2D>().velocity = power;

            yield return new WaitForSeconds(0.2f);

        }

        yield return null;
    }

    private IEnumerator _commet_left()
    {

        //乱射
        for(int i = 1; i < 25; ++i)
        {
            int rnd_x = Random.Range(0, 9);
            int rnd_y = Random.Range(0, 12);

            //斜め上に
            Vector3 pos = new Vector3(-2.4f - rnd_x * 0.1f, 3.0f + rnd_y * 0.1f, _circleBulletPrefab.transform.position.z);
            EnemyBullet bullet = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);

            Vector3 power;

            
            power = new Vector3(4.0f, -4.5f, 0);

            //弾を発射
            bullet.GetComponent<Rigidbody2D>().velocity = power;

            yield return new WaitForSeconds(0.2f);

        }

        yield return null;
    }
}
