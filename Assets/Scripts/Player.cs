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
    private Sprite[] _sprites;

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
        // if(IsSlowingDown == false)
        // {
        //     transform.position = transform.position + ((Vector3)Direction * (_speed * Time.deltaTime));
        // }
        // else
        // {
        //     transform.position = transform.position + ((Vector3)Direction * (_speed * Time.deltaTime * _slowDownRate)); 
        // }

        //Directionから次の位置に移動
        transform.position = transform.position + ((Vector3)Direction * (_speed * Time.deltaTime * (IsSlowingDown ? _slowDownRate : 1)));

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
    public void OnChangeShape0(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            _ChangeShape(0);
        }
    }

    public void OnChangeShape1(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            _ChangeShape(1);
        }
    }

    public void OnChangeShape2(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            _ChangeShape(2);
        }
    }

    //0~2の値に応じて変形
    private void _ChangeShape(int mode)
    {
        //[TODO]アニメーションを入れる？
        //[TODO]形ごとの無敵技処理を入れる

        Debug.Log($"ChangeShape {mode}");
        _damageCollider.GetComponent<SpriteRenderer>().sprite = _sprites[mode];
        _grazeCollider.GetComponent<SpriteRenderer>().sprite = _sprites[mode];
    }
}
