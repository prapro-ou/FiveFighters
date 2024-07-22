using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private DamageCollider _damageCollider;

    [SerializeField]
    private GrazeCollider _grazeCollider;

    [SerializeField]
    private UICollider _uICollider;

    [SerializeField]
    private GameManager _gameManager;

    private PlayerHpBar _playerHpBar;

    private PlayerPrimaryGrazeBar _playerPrimaryGrazeBar;

    private PlayerSpecialGrazeBar _playerSpecialGrazeBar;

    private RectTransform _playArea;

    public RectTransform PlayArea
    {
        get {return _playArea;}
        set
        {
            _playArea = value;
            PlayArea.GetWorldCorners(_corners);
        }
    }

    private Vector3[] _corners = new Vector3[4];

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
    private float _speed;

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

    private int _money = 0;

    public int Money
    {
        get {return _money;}
        set {_money = value;}
    }

    private float _powerMultiplier;

    public float PowerMultiplier
    {
        get {return _powerMultiplier;}
        set {_powerMultiplier = value;}
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
        }
    }

    private int _specialGrazeCount;

    public int SpecialGrazeCount
    {
        get {return _specialGrazeCount;}
        set
        {
            _specialGrazeCount = value;

            if(_playerSpecialGrazeBar == null)
            {
                _playerSpecialGrazeBar = GameObject.Find("PlayerSpecialGrazeBar").GetComponent<PlayerSpecialGrazeBar>();
            }
            _playerSpecialGrazeBar.UpdateSpecialGrazeCount();
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
        MyShape = _ownShapes[0];

        PrimaryGrazeCount = 0;
        SpecialGrazeCount = 0;

        CurrentEnemy = null;

        Direction = Vector2.zero;

        IsInShiftCooldown = false;
        IsSlowingDown = false;

        PowerMultiplier = 1;
        HitPoint = MaxHitPoint;

        ExpansionValue = 1.0f;

        _playerSpecialGrazeBar.UpdateSpecialGrazeCount();
    }

    // Update is called once per frame
    void Update()
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
        Debug.Log(Direction);
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
            transform.position = transform.position + ((Vector3)Direction * (_speed * Time.deltaTime));
        }
        else
        {
            transform.position = transform.position + ((Vector3)Direction * (_speed * Time.deltaTime * _slowDownRate)); 
        }
        // transform.position = transform.position + ((Vector3)Direction * (_speed * Time.deltaTime * (IsSlowingDown ? _slowDownRate : 1)));

        //移動範囲を制限
        float clampedX = Mathf.Clamp(transform.position.x, _corners[0].x, _corners[2].x);
        float clampedY = Mathf.Clamp(transform.position.y, _corners[0].y, _corners[2].y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    public void TakeDamage(int value)
    {
        HitPoint -= value;
        
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
            transform.position += (new Vector3(currentDirection.x, currentDirection.y, 0) * Time.deltaTime * DashSpeed);
            yield return null;
        }

        IsDashing = false;
    }

    //自機消滅
    public void DestroyMyself()
    {
        Destroy(this.gameObject);
    }

    //通常攻撃
    public void PrimaryAttack()
    {
        MyShape.PrimaryAttack();
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
            Debug.Log($"SpecialGrazeCount < MyShape.SpecialSkillCost");
            return;
        }

        MyShape.SpecialSkill();

        SpecialGrazeCount = 0;
    }

    private IEnumerator StartShiftCooldown()
    {
        Debug.Log("Start ShiftCooldown");
        IsInShiftCooldown = true;

        yield return new WaitForSeconds(ShiftCooldown);

        Debug.Log("Finish ShiftCooldown");
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
    }

    public void UseMoney(int cost)
    {
        Money -= cost;
    }

    public void EnhanceHitPoint(int boost)
    {
        MaxHitPoint += boost;
        HitPoint += boost;
    }

    public void EnhancePower(float coefficient)
    {
        PowerMultiplier += coefficient;
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
}
