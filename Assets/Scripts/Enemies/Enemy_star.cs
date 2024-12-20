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
    Attack3,
    Attack4,
    Attack5
}

public class Enemy_Star : Enemy
{

    private SoundManager _soundManager;

    private Player _player;

    [SerializeField]
    private EnemyBullet _circleBulletPrefab;

    [SerializeField]
    private EnemyBullet _rainBulletPrefab;

    [SerializeField]
    private EnemyBullet _starBulletPrefab;

    [SerializeField]
    private EnemyBullet _starstarBulletPrefab;

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

    [SerializeField]
    private EnemyBullet _deathBulletPrefab;

    private bool end;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
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
                //ランダム弾
                {
                    Debug.Log("Attack:" + CurrentState);
                    yield return StartCoroutine(_random());
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
                    for(int i=0;i<2;i++){ 
                        yield return StartCoroutine(_commet_right());
                        yield return StartCoroutine(_commet_left());
                    }

                    yield return new WaitForSeconds(1);
                    CurrentState = StarState.Wait;
                    break;
                }
                case StarState.Attack4:
                //ランダム2回分裂
                {
                    Debug.Log("Attack:" + CurrentState);
                    yield return StartCoroutine(_random_star());

                    CurrentState = StarState.Wait;
                    break;
                }

                case StarState.Attack5:
                //雨
                {
                    Debug.Log("Attack:" + CurrentState);
                    yield return StartCoroutine(_rain());
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

        _PlaySound("Spawn3");

        yield return new WaitForSeconds(4); //Sample
    }


    //死亡したときに、GameManagerによって実行されるコルーチン。
    //死亡したときの演出をこのメソッドに記述しよう。アニメーション自体はスクリプトで書かず、アニメーター(アニメーション)コンポーネントで実装することもできる。
    public override IEnumerator StartDeathAnimation()
    {

        yield return new WaitForSeconds(0.5f); //Sample

        anim.SetBool("Star_Death", true);

        _PlaySound("DeathStar");

        yield return StartCoroutine(_death());

        Destroy(this.gameObject);

        yield return new WaitForSeconds(0.5f); //Sample
    }


    private IEnumerator _death()
    {
       for(int i = 0; i < 150; i++){
            StartCoroutine(_death_ballet());
            yield return new WaitForSeconds(0.01f);
       }

       yield return null;
    }

    private IEnumerator _death_ballet()
    {
        int rnd_x = Random.Range(-20, 20);
        int rnd_y = Random.Range(25, 50);

        //自機の位置に弾を生成
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);
        EnemyBullet bullet = Instantiate(_deathBulletPrefab, pos, Quaternion.identity);

        Vector3 power;

        power = new Vector3(rnd_x * 0.1f, rnd_y * 0.1f, 0);

            //弾を発射
        bullet.GetComponent<Rigidbody2D>().velocity = power;

        yield return null;
    }
    private IEnumerator _random()
    {
       for(int i = 0; i < 500; i++){
            StartCoroutine(_random_ballet());
            _PlaySound("NormalBullet");
            yield return new WaitForSeconds(0.03f);
       }
       yield return new WaitForSeconds(0.5f);

       yield return null;
    }

    private IEnumerator _random_ballet()
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

        for(int i = 0; i < 40 ; i++)
        {
            EnemyBullet bullet = Instantiate(_circleBulletPrefab, pos, Quaternion.identity);

            //プレイヤーの位置を取得し，敵の位置と減算することで敵から自機へ向かうベクトルを生成
            Vector3 target = _player.transform.position;
            Vector3 power = target - this.transform.position;

            //弾を発射 正規化した位置ベクトルに乗算して速さを調整．
            bullet.GetComponent<Rigidbody2D>().velocity = power.normalized * 4.0f;

            yield return new WaitForSeconds(0.4f);
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

            _PlaySound("NormalBullet");

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
            EnemyBullet bullet = Instantiate(_starBulletPrefab, pos, Quaternion.identity);

            Vector3 power;

            
            power = new Vector3(-4.0f, -4.5f, 0);

            //弾を発射
            bullet.GetComponent<Rigidbody2D>().velocity = power;

            _PlaySound("Commet");

            yield return new WaitForSeconds(0.25f);

        }
        yield return new WaitForSeconds(1);


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
            EnemyBullet bullet = Instantiate(_starBulletPrefab, pos, Quaternion.identity);

            Vector3 power;

            
            power = new Vector3(4.0f, -4.5f, 0);

            //弾を発射
            bullet.GetComponent<Rigidbody2D>().velocity = power;

            _PlaySound("Commet");

            yield return new WaitForSeconds(0.25f);

        }

        yield return new WaitForSeconds(1);


        yield return null;
    }

    private IEnumerator _random_star()
    {
       for(int i = 0; i < 30; i++){
            StartCoroutine(_random_ballet_star());
            _PlaySound("NormalBullet");
            yield return new WaitForSeconds(0.6f);
       }
       yield return new WaitForSeconds(2.0f);

       yield return null;
    }

    private IEnumerator _random_ballet_star()
    {
        int rnd_x = Random.Range(0, 2);
        int rnd_y = Random.Range(0, 2);

        //自機の位置に弾を生成
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);
        EnemyBullet bullet = Instantiate(_starstarBulletPrefab, pos, Quaternion.identity);

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

        yield return null;
    }

private IEnumerator _rain()
    {
       for(int i = 0; i < 220; i++){
            StartCoroutine(_rain_ballet());
            yield return new WaitForSeconds(0.08f);
       }
       yield return new WaitForSeconds(0.5f);

       yield return null;
    }

    private IEnumerator _rain_ballet()
    {
        int rnd_x = Random.Range(0, 2);
        int rnd_y = Random.Range(0, 2);

        _PlaySound("Rain");

        //自機の位置に弾を生成
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, _circleBulletPrefab.transform.position.z);
        EnemyBullet bullet = Instantiate(_rainBulletPrefab, pos, Quaternion.identity);

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
