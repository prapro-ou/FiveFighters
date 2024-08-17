using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class Enemy : MonoBehaviour
{
    private GameManager _gameManager;

    private Player _playerForStatus;

    private Collider2D _collider;

    [SerializeField]
    private string _name;

    public string Name
    {
        get {return _name;}
        set {_name = value;}
    }

    [SerializeField]
    private int _maxHitPoint;

    private Slider _hitPointBar;

    public int MaxHitPoint
    {
        get {return _maxHitPoint;}
        set
        {
            _maxHitPoint = value;
        }
    }

    private int _hitPoint;

    public int HitPoint
    {
        get {return _hitPoint;}
        set
        {
            _hitPoint = Mathf.Clamp(value, 0, MaxHitPoint);
            
            if(_hitPointBar == null)
            {
                _hitPointBar = GameObject.Find("EnemyHpBar").GetComponent<Slider>();
            }
            _hitPointBar.value = (float)_hitPoint / (float)MaxHitPoint;

            Debug.Log($"EnemyHP: {HitPoint}");

            if(_hitPoint <= 0)
            {
                StopAllCoroutines();
                _collider.enabled = false;
                _gameManager.ClearStage();
            }
        }
    }

    [SerializeField]
    private GameObject _damageTextPrefab;

    void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _playerForStatus = GameObject.Find("Player").GetComponent<Player>();

        _collider = GetComponent<Collider2D>();

        HitPoint = MaxHitPoint;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerBullet bullet = collider.gameObject.GetComponent<PlayerBullet>();
        if(bullet != null)
        {
            bullet.DestroyWithParticle();
            TakeDamage((int)(bullet.DamageValue * _playerForStatus.PowerMultiplier));
            GenerateDamageText((int)(bullet.DamageValue * _playerForStatus.PowerMultiplier), bullet.transform.position);
        }
    }

    public void TakeDamage(int damage)
    {
        HitPoint -= damage;
    }

    public void GenerateDamageText(int damage, Vector3 pos)
    {
        _damageTextPrefab.GetComponent<TextMeshPro>().text = damage.ToString();
        GameObject text = Instantiate(_damageTextPrefab, pos, Quaternion.identity);
    }

    public abstract void StartAttacking();

    public abstract IEnumerator StartSpawnAnimation();

    public abstract IEnumerator StartDeathAnimation();
}
