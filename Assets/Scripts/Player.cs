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

    private Vector3[] _corners;

    [SerializeField]
    private List<PlayerShape> _ownShapes;

    private int _myShapeNumber;

    public int MyShapeNumber
    {
        get {return _myShapeNumber;}
        set {_myShapeNumber = value;}
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
    private float _slowDownRate;

    private bool _isSlowingDown;

    public bool IsSlowingDown
    {
        get {return _isSlowingDown;}
        set {_isSlowingDown = value;}
    }    

    // Start is called before the first frame update
    void Start()
    {
        MyShapeNumber = 0;

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

        MyShapeNumber = mode;

        _ownShapes[MyShapeNumber].ShiftSkill();
        _ShiftShapeOfColliders(_ownShapes[MyShapeNumber]);
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
}
