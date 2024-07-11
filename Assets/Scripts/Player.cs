using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private RectTransform _playArea;

    [SerializeField]
    private DamageCollider _damageCollider;

    [SerializeField]
    private GrazeCollider _grazeCollider;

    [SerializeField]
    private PlayerHpBar _playerHpBar;

    private Vector3[] _corners;

    [SerializeField]
    private List<PlayerShape> _ownShapes;

    [SerializeField]
    private int _hitPoint;

    public int HitPoint
    {
        get {return _hitPoint;}
        set
        {
            _hitPoint = Mathf.Clamp(value, 0, 100);
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

    private int _grazeCounter;

    public int GrazeCounter
    {
        get {return _grazeCounter;}
        set {_grazeCounter = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        MyShape = _ownShapes[0];

        CurrentEnemy = null;

        Direction = Vector2.zero;

        IsInShiftCooldown = false;
        IsSlowingDown = false;

        //プレイエリアの角を取得
        _corners = new Vector3[4];
        _playArea.GetWorldCorners(_corners);
    }

    // Update is called once per frame
    void Update()
    {
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

    //自機消滅
    public void DestroyMyself()
    {
        Destroy(this.gameObject);
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

        dcSpriteRenderer.sprite = pShape.MySprite;
        gcSpriteRenderer.sprite = pShape.MySprite;

        Color grazeColor = new Color(pShape.MyColor.r, pShape.MyColor.g, pShape.MyColor.b, (pShape.MyColor.a/3));

        dcSpriteRenderer.color = pShape.MyColor;
        gcSpriteRenderer.color = grazeColor;
    }

    private void AddMoney(int reward)
    {
        Money += reward;
    }

    private void UseMoney(int cost)
    {
        Money -= cost;
    }
}
