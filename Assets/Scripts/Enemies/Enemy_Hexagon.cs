using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum HexagonState
{
    Wait,
    // Attack1,
    // Attack2,
    // Attack3,
    // Attack4,
    Attack5
    // Attack6
}

public class Enemy_Hexagon : Enemy
{
    private Player _player;

    [SerializeField]
    private Animator _animator;

    private GameObject _hexagonWall;

    private GameObject _hexagonLine;

    private CameraManager _cameraManager;

    [SerializeField]
    private AnimationCurve _movingCurve;

    [SerializeField]
    private AnimationCurve _beatingCurve;

    [SerializeField]
    private AnimationCurve _vibrateCurveX;
    
    [SerializeField]
    private AnimationCurve _vibrateCurveY;

    [SerializeField]
    private EnemyBullet _enemyHexagonBullet;

    [SerializeField]
    private EnemyBullet _hexagonTriangleBullet;

    [SerializeField]
    private ShrinkHexagonBullet _shrinkHexagonBulletPrefab; 

    [SerializeField]
    private MoveHexagonBullet _moveHexagonBulletPrefab; 

    [SerializeField]
    private GameObject _hexagonWallPrefab;

    [SerializeField]
    private GameObject _hexagonLinePrefab;

    [SerializeField]
    private EnemyBullet _hexagonLaserPrefab;

    [SerializeField]
    private EnemyBullet _hexagonLaserCautionPrefab;

    [SerializeField]
    private GameObject _hexagonCautionEffectPrefab;

    [SerializeField]
    private GameObject _hexagonTeleportEffectPrefab;

    private HexagonState _currentState;

    public HexagonState CurrentState
    {
        get {return _currentState;}
        set {_currentState = value;}
    }

    private float _currentPosition;

    public float CurrentPosition
    {
        get {return _currentPosition;}
        set {_currentPosition = value;}
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
                // case HexagonState.Attack1:
                // {
                //     yield return StartCoroutine(_ShrinkHexagon());
                //     CurrentState = HexagonState.Wait;
                //     break;
                // }
                // case HexagonState.Attack2:
                // {
                //     yield return StartCoroutine(_HexagonLaser());
                //     CurrentState = HexagonState.Wait;
                //     break;
                // }
                // case HexagonState.Attack3:
                // {
                //     yield return StartCoroutine(_RotatingFire());
                //     CurrentState = HexagonState.Wait;
                //     break;
                // }
                // case HexagonState.Attack4:
                // {
                //     yield return StartCoroutine(_DiagonalFire());
                //     CurrentState = HexagonState.Wait;
                //     break;
                // }
                case HexagonState.Attack5:
                {
                    yield return StartCoroutine(_WallFire());
                    CurrentState = HexagonState.Wait;
                    break;
                }
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

        transform.position = Vector3.zero;

        yield return new WaitForSeconds(2f);

        Vector3[] cautionPositions = {new Vector3(0, 4, 0), new Vector3(3.4f, 2f, 0), new Vector3(3.4f, -2f, 0), new Vector3(0, -4, 0), new Vector3(-3.4f, -2f, 0), new Vector3(-3.4f, 2f, 0)};
        for(int ic = 0; ic < 6; ic++)
        {
            Instantiate(_hexagonCautionEffectPrefab, cautionPositions[ic], Quaternion.identity);
        }

        yield return new WaitForSeconds(1.5f);

        Rigidbody2D rb; 
        Vector3[] positions = {new Vector3(0, 10, 0), new Vector3(8.66f, 5, 0), new Vector3(8.66f, -5, 0), new Vector3(0, -10, 0), new Vector3(-8.66f, -5, 0), new Vector3(-8.66f, 5, 0)};
        for(int i = 0; i < 6; i++)
        {
            EnemyBullet bullet = Instantiate(_hexagonTriangleBullet, positions[i], Quaternion.Euler(0, 0, 180 + (60 * i)));
            rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = positions[i].normalized * -10;
            Destroy(bullet.gameObject, 1f);
        }
        yield return new WaitForSeconds(1f);

        // Instantiate(_hexagonTeleportEffectPrefab, Vector3.zero, Quaternion.identity);
        StartCoroutine(_BeatOnCurve(0.5f, 1f));

        // StartCoroutine(_cameraManager.Vibrate(0.5f, 1f));

        _animator.SetTrigger("Appear");

        StartCoroutine(_cameraManager.SetSizeOnCurve(3f, 0.2f));

        // StartCoroutine(_cameraManager.RotateOnCurve(10, 10f));

        yield return StartCoroutine(_cameraManager.MoveToPointOnCurve(transform.position));

        // StartCoroutine(_cameraManager.RotateOnCurve(-10, 0.2f));
        
        StartCoroutine(_cameraManager.SetSizeOnCurve(5f));

        yield return StartCoroutine(_MoveToPointOnCurve(new Vector3(0, 3, 0), 0.5f));

        CurrentPosition = 0;

        yield return new WaitForSeconds(1f);
    }

    //死亡したときに、GameManagerによって実行されるコルーチン。
    //死亡したときの演出をこのメソッドに記述しよう。アニメーション自体はスクリプトで書かず、アニメーター(アニメーション)コンポーネントで実装することもできる。
    public override IEnumerator StartDeathAnimation()
    {
        Debug.Log("StartDeathAnimation");

        if(_hexagonWall != null)
        {
            Destroy(_hexagonWall);
            _hexagonWall = null;
        }

        if(_hexagonLine != null)
        {
            Destroy(_hexagonLine);
            _hexagonLine = null;
        }

        yield return new WaitForSeconds(1.5f); //Sample

        _animator.SetTrigger("Defeat");
        StartCoroutine(_Vibrate(2.5f, 0.2f));

        yield return new WaitForSeconds(2.5f);

        Destroy(this.gameObject);

        yield return new WaitForSeconds(1); //Sample
    }

    //以下二つの攻撃行動はサンプル。
    //各攻撃行動は、IEnumerator型のメソッドとして定義する。"yield return new WaitForSeconds(second)"のsecondの秒数だけこのメソッドが実行される。
    //攻撃処理はその上に書く。メソッド内で”yield return StartCoroutine(_METHOD());”とすることで、そのメソッドが終わるまで待機ができる。
    private IEnumerator _ShrinkHexagon()
    {
        ShrinkHexagonBullet bullet;
        Quaternion rotation;

        Debug.Log("Start ShrinkHexagon");

        if(CurrentPosition != 1)
        {
            //中央移動
            yield return StartCoroutine(_MoveToPointOnCurve(Vector3.zero, 1f));
            CurrentPosition = 1;
        }

        yield return new WaitForSeconds(1f);

        for(int times = 0; times < 2; times++)
        {
            for(int i = 0; i < 3; i++)
            {
                //弾
                bool direction = Random.Range(0,2) == 0;

                if(direction) {StartCoroutine(_RotateOnCurve(360f, 1f));}
                else {StartCoroutine(_RotateOnCurve(-360f, 1f));}
                
                rotation = Quaternion.Euler(0, 0, Random.Range(0, 6) * 60);
                bullet = Instantiate(_shrinkHexagonBulletPrefab, Vector3.zero, rotation);

                bullet.IsRight = direction;

                yield return new WaitForSeconds(1.5f);
            }
            yield return new WaitForSeconds(2f);
        }
        
        yield return new WaitForSeconds(1f);
    
        Debug.Log("Finish ShringHexagon");
    }

    private IEnumerator _HexagonLaser()
    {
        Debug.Log("Start HexagonLaser");

        if(CurrentPosition != 2)
        {
            //画面上移動
            yield return StartCoroutine(_MoveToPointOnCurve(new Vector3(0, 4.5f, 0), 1f));
            CurrentPosition = 2;
        }

        yield return StartCoroutine(_TeleportPlayer(Vector3.zero));

        _hexagonWall = _SummonWall();
        _hexagonWall.GetComponent<Animator>().SetTrigger("Appear");

        yield return new WaitForSeconds(1f);

        Transform[] pointTransforms = _hexagonWall.GetComponentsInChildren<Transform>();
        
        Quaternion[] rotations = {Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 60), Quaternion.Euler(0, 0, 120)};

        for(int i = 0; i < 3; i++)
        {
            Vector3 position = pointTransforms[Random.Range(0,6)].position;
            Quaternion rotation = rotations[Random.Range(0,3)];
            StartCoroutine(_Laser(position, rotation));
            StartCoroutine(_BeatOnCurve(1f, 0.5f));
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

            StartCoroutine(_BeatOnCurve(1f, 0.5f));
            
            yield return new WaitForSeconds(2.5f);
        }

        yield return new WaitForSeconds(1f);

        _hexagonWall.GetComponent<Animator>().SetTrigger("Disappear");

        Destroy(_hexagonWall, 1f);
        _hexagonWall = null;

        Debug.Log("Finish HexagonLaser");
    }

    private IEnumerator _RotatingFire()
    {
        Debug.Log("Start RotatingFire");

        float delay = 0.1f;
        float duration = 3f;
        float speed;

        if(CurrentPosition != 1)
        {
            yield return StartCoroutine(_MoveToPointOnCurve(Vector3.zero, 1f));
            CurrentPosition = 1;
        }

        yield return new WaitForSeconds(1f);

        for(int times = 0; times < 3; times++)
        {
            speed = Random.Range(30f, 60f);
            int direction = (Random.Range(0, 2) == 0) ? -1 : 1;

            yield return StartCoroutine(_RotateOnCurve(direction * 180f, 1f));

            StartCoroutine(_Rotate(duration, direction * speed));

            for(float i = 0; i <= duration; i += delay)
            {
                StartCoroutine(_ShootHexagonShot(transform.position));
                yield return new WaitForSeconds(delay);
            }
        }

        yield return StartCoroutine(_ResetRotation());

        Debug.Log("Finish RotatingFire");
    }

    private IEnumerator _DiagonalFire()
    {
        Debug.Log("Start DiagonalFire");

        // float delay = 0.1f;
        // float duration = 3f;
        // // float speed;

        if(CurrentPosition != 2)
        {
            //画面上移動
            yield return StartCoroutine(_MoveToPointOnCurve(new Vector3(0, 4.5f, 0), 1f));
            CurrentPosition = 2;
        }

        yield return StartCoroutine(_TeleportPlayer(Vector3.zero));

        _hexagonWall = _SummonWall();
        _hexagonWall.GetComponent<Animator>().SetTrigger("Appear");

        yield return new WaitForSeconds(1f);

        List<Transform> pointTransforms1 = (_hexagonWall.GetComponentsInChildren<Transform>()).ToList();
        List<Transform> pointTransforms2 = (_hexagonWall.GetComponentsInChildren<Transform>()).ToList();
        pointTransforms1.RemoveAt(0);
        pointTransforms2.RemoveAt(0);

        for(int times = 0; times < 2; times++)
        {
            // シャッフル
            _ShuffleTransformList(pointTransforms1);
            _ShuffleTransformList(pointTransforms2);

            Instantiate(_hexagonCautionEffectPrefab, pointTransforms1[0].position, Quaternion.identity);
            Instantiate(_hexagonCautionEffectPrefab, pointTransforms2[0].position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);

            MoveHexagonBullet bullet1 = Instantiate(_moveHexagonBulletPrefab, pointTransforms1[0].position, Quaternion.identity);
            MoveHexagonBullet bullet2 = Instantiate(_moveHexagonBulletPrefab, pointTransforms2[0].position, Quaternion.identity);

            bullet1.GetComponent<Animator>().SetTrigger("Appear");
            bullet2.GetComponent<Animator>().SetTrigger("Appear");

            yield return new WaitForSeconds(0.5f);

            for(int i = 1; i < 6; i++)
            {
                Instantiate(_hexagonCautionEffectPrefab, pointTransforms1[i].position, Quaternion.identity);
                Instantiate(_hexagonCautionEffectPrefab, pointTransforms2[i].position, Quaternion.identity);
                yield return new WaitForSeconds(0.8f);

                StartCoroutine(_BeatOnCurve(1f, 0.3f));

                StartCoroutine(bullet1.MoveToPointOnCurve(pointTransforms1[i].position, 1f));
                yield return StartCoroutine(bullet2.MoveToPointOnCurve(pointTransforms2[i].position, 1f));
            }

            bullet1.GetComponent<Animator>().SetTrigger("Disappear");
            bullet2.GetComponent<Animator>().SetTrigger("Disappear");

            Destroy(bullet1.gameObject, 1f);
            Destroy(bullet2.gameObject, 1f);
        }

        yield return new WaitForSeconds(1f);

        _hexagonWall.GetComponent<Animator>().SetTrigger("Disappear");

        Destroy(_hexagonWall, 1f);
        _hexagonWall = null;

        Debug.Log("Finish DiagonalFire");
    }

    private IEnumerator _WallFire()
    {
        Debug.Log("Start WallFire");

        float delay = 0.3f;
        // float duration = 3f;
        float speed = 3f;

        if(CurrentPosition != 3)
        {
            yield return StartCoroutine(_MoveToPointOnCurve(new Vector3(0, 4, 0), 1f));
            CurrentPosition = 3;
        }

        _hexagonLine = _SummonLine();
        _hexagonLine.GetComponent<Animator>().SetTrigger("Appear");

        yield return new WaitForSeconds(1f);

        List<Transform> pointTransforms = (_hexagonLine.GetComponentsInChildren<Transform>()).ToList();
        pointTransforms.RemoveAt(0);

        _ShuffleTransformList(pointTransforms);

        StartCoroutine(_BeatOnCurve(6f, 0.3f));
        StartCoroutine(_RotateOnCurve(720f, 6f));

        for(int i = 0; i < 20; i++)
        {
            StartCoroutine(_ShootHexagonShotWithCaution(pointTransforms[i % pointTransforms.Count].position, speed));
            yield return new WaitForSeconds(delay);
        }

        _hexagonLine.GetComponent<Animator>().SetTrigger("Disappear");

        Destroy(_hexagonLine, 1f);
        _hexagonLine = null;

        yield return new WaitForSeconds(1f);
    }

    private IEnumerator _ShootHexagonShot(Vector3 position, float speed = 8f)
    {
        Rigidbody2D rb;

        for(int i = 0; i < 6; i++)
        {
            EnemyBullet bullet = Instantiate(_enemyHexagonBullet, position, Quaternion.Euler(0, 0, (60 * i)) * transform.rotation);
            rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = bullet.transform.right * speed;
        }
        
        yield return null;
    }

    private IEnumerator _ShootHexagonShotWithCaution(Vector3 position, float speed)
    {
        Instantiate(_hexagonCautionEffectPrefab, position, Quaternion.identity);
        
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(_ShootHexagonShot(position, speed));
    }

    private GameObject _SummonWall()
    {
        GameObject wall = Instantiate(_hexagonWallPrefab, Vector3.zero, Quaternion.identity);

        return wall;
    }

    private GameObject _SummonLine()
    {
        GameObject line = Instantiate(_hexagonLinePrefab, Vector3.zero, Quaternion.identity);

        return line;
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

    private IEnumerator _MoveToPointOnCurve(Vector3 endPoint, float duration = 1f)
    {
        Vector3 startPoint = transform.position;

        for(float i = 0; i <= duration; i += Time.deltaTime)
        {
            Vector3 lerpVec3 = Vector3.Lerp(startPoint, endPoint, _movingCurve.Evaluate(i / duration));
            lerpVec3.z = transform.position.z;
            transform.position = lerpVec3;
            yield return null;
        }
    }
    
    private IEnumerator _Rotate(float duration = 1f, float speed = 1f)
    {
        Debug.Log("Start Rotate");

        for(float i = 0; i <= duration; i += Time.deltaTime)
        {
            transform.Rotate(0, 0, speed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("Finish Rotate");
    }

    private IEnumerator _ResetRotation(float duration = 1f)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.identity;
        Quaternion lerpRotation;

        for(float i = 0; i <= duration; i += Time.deltaTime)
        {
            lerpRotation = Quaternion.Lerp(startRotation, endRotation, _movingCurve.Evaluate(i / duration));
            transform.rotation = lerpRotation;
            yield return null;
        }
    }

    private IEnumerator _RotateOnCurve(float angle, float duration = 1f)
    {
        Quaternion startRotation = transform.rotation;
        float startZ = transform.rotation.z;
        float endZ = transform.rotation.z + angle;
        float lerpZ;

        for(float i = 0; i <= duration; i += Time.deltaTime)
        {
            lerpZ = Mathf.Lerp(startZ, endZ, _movingCurve.Evaluate(i / duration));
            transform.rotation = startRotation * Quaternion.Euler(0, 0, lerpZ);
            yield return null;
        }
    }

    private IEnumerator _BeatOnCurve(float duration = 1f, float power = 1f)
    {
        Vector3 startScale = transform.localScale;
        float lerpScaleX;

        for(float i = 0; i <= duration; i += Time.deltaTime)
        {
            lerpScaleX = startScale.x + (_beatingCurve.Evaluate(i / duration) * power);
            transform.localScale = new Vector3(lerpScaleX, lerpScaleX, 1);
            yield return null;
        }
    }

    private IEnumerator _Vibrate(float duration, float power)
    {
        Vector3 startPosition = transform.position;

        Vector3 nextPosition = startPosition;

        for(float i = 0; i <= duration; i += Time.deltaTime)
        {
            nextPosition.x = startPosition.x + (_vibrateCurveX.Evaluate(i / duration) * power);
            nextPosition.y = startPosition.y + (_vibrateCurveY.Evaluate(i / duration) * power);
            transform.position = nextPosition;
            yield return null;
        }

        transform.position = startPosition;
    }

    private IEnumerator _TeleportPlayer(Vector3 position)
    {
        //Playerを親にエフェクト
        GameObject playerEffect = Instantiate(_hexagonTeleportEffectPrefab, _player.transform.position , Quaternion.identity, _player.transform);

        //中心にエフェクト
        Instantiate(_hexagonTeleportEffectPrefab, Vector3.zero, Quaternion.identity);

        yield return new WaitForSeconds(1f);

        //中心にPlayerをテレポート
        playerEffect.transform.parent = null;
        float currentSpeed = _player.CurrentSpeed;
        _player.transform.position = Vector3.zero;
        _player.CurrentSpeed = 0;

        yield return new WaitForSeconds(0.5f);
        _player.CurrentSpeed = currentSpeed;

        yield return null;
    }

    private void _ShuffleTransformList(List<Transform> list)
    {
        for (var i = list.Count - 1; i > 0; --i)
        {
            var j = Random.Range(0, i + 1);
            var tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
    }
}
