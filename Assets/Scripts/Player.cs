using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private DamageCollider _damageCollider;

    [SerializeField]
    private GrazeCollider _grazeCollider;

    [SerializeField]
    private UICollider _uICollider;

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private CameraManager _cameraManager;

    [SerializeField]
    private OverlayManager _overlayManager;

    [SerializeField]
    private SoundManager _soundManager;

    [SerializeField]
    private AnimationCurve _vibrateCurveX;

    [SerializeField]
    private AnimationCurve _vibrateCurveY;

    [SerializeField]
    private GameObject _playerDefeatEffectPrefab;

    [SerializeField]
    private GameObject _playerDeathExplodeEffectPrefab;

    [SerializeField]
    private GameObject _playerTransformableEffectPrefab;

    private PlayerHpBar _playerHpBar;

    private PlayerPrimaryGrazeBar _playerPrimaryGrazeBar;

    private PlayerSpecialGrazeBar _playerSpecialGrazeBar;

    private PlayerHpText _playerHpText;

    private PlayerPrimaryText _playerPrimaryGrazeText;

    private PlayerSpecialText _playerSpecialGrazeText;

    private TMP_Text _moneyText;

    private TMP_Text _powerText;

    private PlayerPowerTile _powerTile;

    private PlayerShiftCooltimeImage _cooltimeImage;

    [SerializeField]
    private List<PlayerShape> _ownShapes;

    private int _hitPoint;

    public int HitPoint
    {
        get {return _hitPoint;}
        set
        {
            _hitPoint = Mathf.Clamp(value, 0, MaxHitPoint);

            if(_playerHpBar == null)
            {
                _playerHpBar = GameObject.Find("PlayerHpBar").GetComponent<PlayerHpBar>();
            }
            _playerHpBar.UpdateHp();

            if(_playerHpText == null)
            {
                _playerHpText = GameObject.Find("PlayerHpText").GetComponent<PlayerHpText>();
            }
            _playerHpText.UpdateHpUI();

            if(_hitPoint <= 0)
            {
                Die();
            }
        }
    }

    private PlayerShape _myShape;

    public PlayerShape MyShape
    {
        get {return _myShape;}
        set
        {
            _myShape = value;
            _ShiftShapeOfColliders(MyShape);
        }
    }

    private Shape_SmallTriangle _smallRightTriangle;

    public Shape_SmallTriangle SmallRightTriangle
    {
        get {return _smallRightTriangle;}
        set {_smallRightTriangle = value;}
    }

    private Shape_SmallTriangle _smallLeftTriangle;

    public Shape_SmallTriangle SmallLeftTriangle
    {
        get {return _smallLeftTriangle;}
        set {_smallLeftTriangle = value;}
    }

    private Enemy _currentEnemy;

    public Enemy CurrentEnemy
    {
        get {return _currentEnemy;}
        set {_currentEnemy = value;}
    }

    private Vector2 _direction;

    public Vector2 Direction
    {
        get {return _direction;}
        set {_direction = value;}
    }

    [SerializeField]
    private float _defaultSpeed;

    public float DefaultSpeed
    {
        get {return _defaultSpeed;}
        set
        {
            // _speed = Mathf.Max(0, value);
            _defaultSpeed = value;
        }
    }

    private float _currentSpeed;

    public float CurrentSpeed
    {
        get {return _currentSpeed;}
        set
        {
            // _speed = Mathf.Max(0, value);
            _currentSpeed = value;
        }
    }

    [SerializeField]
    private float _dashTime;

    public float DashTime
    {
        get {return _dashTime;}
        set {_dashTime = value;}
    }

    [SerializeField]
    private float _dashSpeed;

    public float DashSpeed
    {
        get {return _dashSpeed;}
        set {_dashSpeed = value;}
    }

    private bool _isDashing;

    public bool IsDashing
    {
        get {return _isDashing;}
        set {_isDashing = value;}
    }

    [SerializeField]
    private float _shiftCooldown;

    public float ShiftCooldown
    {
        get {return _shiftCooldown;}
        set {_shiftCooldown = value;}
    }

    private bool _isInShiftCooldown;

    public bool IsInShiftCooldown
    {
        get {return _isInShiftCooldown;}
        set {_isInShiftCooldown = value;}
    }

    [SerializeField]
    private float _slowDownRate;

    private bool _isSlowingDown;

    public bool IsSlowingDown
    {
        get {return _isSlowingDown;}
        set {_isSlowingDown = value;}
    }

    private bool _isDead;

    public bool IsDead
    {
        get {return _isDead;}
        set {_isDead = value;}
    }

    private int _money = 0;

    public int Money
    {
        get {return _money;}
        set
        {
            _money = value;

            if(_moneyText == null)
            {
                _moneyText = GameObject.Find("MoneyText").GetComponent<TMP_Text>();
            }

            _moneyText.SetText($"{_money}");
        }
    }

    private float _powerMultiplier;

    public float PowerMultiplier
    {
        get {return _powerMultiplier;}
        set
        {
            _powerMultiplier = value;

            if(_powerText == null)
            {
                _powerText = GameObject.Find("PlayerPowerText").GetComponent<TMP_Text>();
            }

            _powerText.SetText($"×{_powerMultiplier:.0}");

            if(_powerTile == null)
            {
                _powerTile = GameObject.Find("PowerTile").GetComponent<PlayerPowerTile>();
            }
            _powerTile.UpdatePowerTile();
        }
    }

    [SerializeField]
    private int _maxHitPoint;

    public int MaxHitPoint
    {
        get {return _maxHitPoint;}
        set {_maxHitPoint = value;}
    }

    private int _primaryGrazeCount;

    public int PrimaryGrazeCount
    {
        get {return _primaryGrazeCount;}
        set
        {
            _primaryGrazeCount = value;

            if(PrimaryGrazeCount >= MyShape.PrimaryAttackCost)
            {
                PrimaryAttack();
            }

            if(_playerPrimaryGrazeBar == null)
            {
                _playerPrimaryGrazeBar = GameObject.Find("PlayerPrimaryGrazeBar").GetComponent<PlayerPrimaryGrazeBar>();
            }
            _playerPrimaryGrazeBar.UpdatePrimaryGrazeCount();

            // if(_playerPrimaryGrazeText == null)
            // {
            //     _playerPrimaryGrazeText = GameObject.Find("PlayerPrimaryGrazeText").GetComponent<PlayerPrimaryText>();
            // }
            // _playerPrimaryGrazeText.UpdatePrimaryUI();
        }
    }

    private int _specialGrazeCount;

    public int SpecialGrazeCount
    {
        get {return _specialGrazeCount;}
        set
        {
            _specialGrazeCount = Mathf.Clamp(value, 0, MyShape.SpecialSkillCost);

            if(_playerSpecialGrazeBar == null)
            {
                _playerSpecialGrazeBar = GameObject.Find("PlayerSpecialGrazeBar").GetComponent<PlayerSpecialGrazeBar>();
            }
            _playerSpecialGrazeBar.UpdateSpecialGrazeCount();

            if(_playerSpecialGrazeText == null)
            {
                _playerSpecialGrazeText = GameObject.Find("PlayerSpecialGrazeText").GetComponent<PlayerSpecialText>();
            }
            _playerSpecialGrazeText.UpdateSpecialUI();
        }
    }

    private float _expansionValue;

    public float ExpansionValue
    {
        get {return _expansionValue;}
        set {
                _expansionValue = value;
                _UpdateGrazeCollider();
            }
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        MyShape = _ownShapes[0];

        CurrentSpeed = _defaultSpeed;

        PrimaryGrazeCount = 0;
        SpecialGrazeCount = 0;

        CurrentEnemy = null;

        Direction = Vector2.zero;

        IsInShiftCooldown = false;
        IsSlowingDown = false;
        IsDead = false;

        PowerMultiplier = 1;
        HitPoint = MaxHitPoint;

        ExpansionValue = 1.0f;

        _playerSpecialGrazeBar.UpdateSpecialGrazeCount();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if(IsDashing) {return;}

        _Move();
    }

    // 方向入力を受ける関数
    public void OnMove(InputAction.CallbackContext context)
    {
        //[TODO]アニメーションを入れる

        //十字キーを放したときに方向をリセット
        if(!context.performed) {Direction = Vector2.zero; return;}

        //コンテキストから入力を読み取り正規化
        Direction = context.ReadValue<Vector2>();
        Direction.Normalize();
        // Debug.Log(Direction);
    }

    public void OnSlowDown(InputAction.CallbackContext context)
    {
        // [TODO]アニメーションを入れる？

        if(!context.performed) {IsSlowingDown = false; return;}

        IsSlowingDown = true;
    }

    private void _Move()
    {
        //Directionから次の位置に移動
        if(IsSlowingDown == false)
        {
            _rigidbody.MovePosition(transform.position + ((Vector3)Direction * (CurrentSpeed * Time.fixedDeltaTime)));
        }
        else
        {
            _rigidbody.MovePosition(transform.position + ((Vector3)Direction * (CurrentSpeed * Time.fixedDeltaTime * _slowDownRate))); 
        }
    }

    public void TakeDamage(int value)
    {
        HitPoint -= value;

        StartCoroutine(_cameraManager.Vibrate(0.2f, 0.1f));

        _PlaySound("Damage");
        
        Debug.Log($"TakeDamage HP: {HitPoint}(Damage:{HitPoint})");
    }

    public void Dash()
    {
        Debug.Log("Dash");
        _damageCollider.BeInvincibleWithDash();

        StartCoroutine(StartDash());
    }

    private IEnumerator StartDash()
    {
        IsDashing = true;
        Vector2 currentDirection = Direction;

        for(float i = 0f; i < DashTime; i += Time.deltaTime)
        {
            _rigidbody.MovePosition(transform.position + (new Vector3(currentDirection.x, currentDirection.y, 0) * Time.fixedDeltaTime * DashSpeed));
            yield return null;
        }

        IsDashing = false;
    }

    //自機消滅
    public void DestroyMyself()
    {
        Destroy(this.gameObject);
    }

    public void Die()
    {
        _grazeCollider.enabled = false;
        _damageCollider.enabled = false;
        IsDead = true;
        _gameManager.DiePlayer();
    }

    public IEnumerator StartDeathAnimation()
    {
        Debug.Log("StartDeathAnimation: Player");
        yield return new WaitForSeconds(1.5f);

        for(int i = 0; i < 2; i++)
        {
            _PlaySound("Damage");
            Instantiate(_playerDeathExplodeEffectPrefab, transform.position, Quaternion.identity);
            yield return StartCoroutine(_Vibrate(0.5f, 0.2f));
        }

        _PlaySound("Explosion1");
        StartCoroutine(_cameraManager.Vibrate(2f, 2f));

        Instantiate(_playerDefeatEffectPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);

        yield return new WaitForSeconds(1.5f);
    }

    //通常攻撃
    public void PrimaryAttack()
    {
        MyShape.PrimaryAttack();

        if (SmallRightTriangle != null)
        {
            SmallRightTriangle.PrimaryAttack();
        }
        if (SmallLeftTriangle != null)
        {
            SmallLeftTriangle.PrimaryAttack();
        }

        PrimaryGrazeCount -= MyShape.PrimaryAttackCost;
    }

    //変形入力を受ける関数
    public void OnShiftShape0(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            _ShiftShape(0);
        }
    }

    public void OnShiftShape1(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            _ShiftShape(1);
        }
    }

    public void OnShiftShape2(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            _ShiftShape(2);
        }
    }

    //0~2の値に応じて変形
    private void _ShiftShape(int mode)
    {
        //[TODO]アニメーションを入れる？
        //[TODO]形ごとの無敵技処理を入れる

        Debug.Log($"ShiftShape {mode}");

        if(MyShape == _ownShapes[mode])
        {
            Debug.Log("Shifting to same shape");
            return;
        }

        if(IsInShiftCooldown)
        {
            return;
        }

        MyShape = _ownShapes[mode];
        // _PlaySound("Transform");

        MyShape.ShiftSkill();

        StartCoroutine(StartShiftCooldown());
    }

    public void OnSpecialSkill(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _SpecialSkill();
        }
    }

    private void _SpecialSkill()
    {
        if (SpecialGrazeCount < MyShape.SpecialSkillCost)
        {
            Debug.Log($"Player does not have enough SPGrazeCount");
            return;
        }

        MyShape.SpecialSkill();

        SpecialGrazeCount = 0;
        _grazeCollider.SpecialSkillFlag = false;
    }

    private IEnumerator StartShiftCooldown()
    {
        Debug.Log("Start ShiftCooldown");
        IsInShiftCooldown = true;

        if(_cooltimeImage == null)
        {
            _cooltimeImage = GameObject.Find("CooltimeImage").GetComponent<PlayerShiftCooltimeImage>();
        }

        for(float time = 0; time <= ShiftCooldown; time += Time.deltaTime)
        {
            _cooltimeImage.UpdateCooltimeImage(time / ShiftCooldown);
            yield return null;
        }

        Debug.Log("Finish ShiftCooldown");
        GameObject transEffect = Instantiate(_playerTransformableEffectPrefab, this.transform.position, Quaternion.identity, this.transform);
        _PlaySound("Transformable");
        IsInShiftCooldown = false;
    }

    private void _ShiftShapeOfColliders(PlayerShape pShape)
    {
        SpriteRenderer dcSpriteRenderer = _damageCollider.GetComponent<SpriteRenderer>();
        SpriteRenderer gcSpriteRenderer = _grazeCollider.GetComponent<SpriteRenderer>();

        PolygonCollider2D dcCollider = _damageCollider.GetComponent<PolygonCollider2D>();
        PolygonCollider2D gcCollider = _grazeCollider.GetComponent<PolygonCollider2D>();

        dcSpriteRenderer.sprite = pShape.MySprite;
        gcSpriteRenderer.sprite = pShape.MySprite;

        Color grazeColor = new Color(pShape.MyColor.r, pShape.MyColor.g, pShape.MyColor.b, (pShape.MyColor.a/3));

        dcSpriteRenderer.color = pShape.MyColor;
        gcSpriteRenderer.color = grazeColor;

        dcCollider.pathCount = dcSpriteRenderer.sprite.GetPhysicsShapeCount();
        List<Vector2> path = new List<Vector2>();
        for (int i = 0; i < dcCollider.pathCount; i++)
        {
            path.Clear();
            dcSpriteRenderer.sprite.GetPhysicsShape(i, path);
            dcCollider.SetPath(i, path.ToArray());
        }

        gcCollider.pathCount = gcSpriteRenderer.sprite.GetPhysicsShapeCount();
        for (int i = 0; i < gcCollider.pathCount; i++)
        {
            path.Clear();
            gcSpriteRenderer.sprite.GetPhysicsShape(i, path);
            gcCollider.SetPath(i, path.ToArray());
        }
    }

    private void _UpdateGrazeCollider()
    {
        _grazeCollider.transform.localScale = MyShape.GrazeColliderSize * ExpansionValue;
    }

    public void AddMoney(int reward)
    {
        Money += reward;
        _PlaySound("GetMoney");
    }

    public void UseMoney(int cost)
    {
        Money -= cost;
    }

    public void EnhanceHitPoint(int boost)
    {
        MaxHitPoint += boost;
        HitPoint += boost;
        _PlaySound("PowerUp");
    }

    public void EnhancePower(float coefficient)
    {
        PowerMultiplier += coefficient;
        _PlaySound("PowerUp");
    }

    public void EnhanceGrazeCollider(float coefficient)
    {
        ExpansionValue += coefficient;
        _PlaySound("PowerUp");
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if(_uICollider.TouchingUI == null) {return;}

        if (context.performed)
        {
            _Submit();
        }
    }

    private void _Submit()
    {
        _uICollider.TouchingUI.InvokeUIAction();
    }

    public void ResetStatusInShop()
    {
        HitPoint = MaxHitPoint;
        PrimaryGrazeCount = 0;
        SpecialGrazeCount = 0;
        CurrentSpeed = _defaultSpeed;

        if(SmallLeftTriangle != null) Destroy(SmallLeftTriangle.gameObject);
        if(SmallRightTriangle != null) Destroy(SmallRightTriangle.gameObject);
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

    private void _PlaySound(string name)
    {
        if(_soundManager == null)
        {
            _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        }

        _soundManager.PlaySound(name);
    }
}
