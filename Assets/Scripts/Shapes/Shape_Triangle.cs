using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape_Triangle : PlayerShape
{
    private Player _player;    
    private GameObject _triangleDestroyField;
    
    [SerializeField]
    private Shape_SmallTriangle _smallTriangle;

    private Shape_SmallTriangle _smallRightTriangle;
    private Shape_SmallTriangle _smallLeftTriangle;

    [SerializeField]
    private float _smallTrianglePosition;

    [SerializeField]
    private GrazeCollider_SmallTriangle _smallTriangleGrazeCollider;

    private GameObject _smallRightTriangleGraze;
    private GameObject _smallLeftTriangleGraze;

    private List<GameObject> activeTriangles = new List<GameObject>();

    [SerializeField]
    private PrimaryTriangleBullet _primaryTriangleBullet;

    private PlayerBullet _playerbullet;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PrimaryAttack()
    {
        Vector3 vec = _player.transform.position;
        
        PlayerBullet bullet = Instantiate(_primaryTriangleBullet, new Vector3(vec.x + 0.3f, vec.y, vec.z), Quaternion.identity);
        bullet.DamageValue = PrimaryAttackDamage;

        bullet = Instantiate(_primaryTriangleBullet, new Vector3(vec.x - 0.3f, vec.y, vec.z), Quaternion.identity);
        bullet.DamageValue = PrimaryAttackDamage;
    }

    public override void SpecialSkill()
    {
        if (activeTriangles.Count >= 3)
        {
            Debug.Log("Max SmallTriangle.");
            return;
        }
              
        Vector3 rightPosition = new Vector3(_player.transform.position.x + _smallTrianglePosition, _player.transform.position.y + _smallTrianglePosition);
        Vector3 leftPosition  = new Vector3(_player.transform.position.x - _smallTrianglePosition, _player.transform.position.y + _smallTrianglePosition);

        //子機の本体を左右2つ生成
        _smallRightTriangle = Instantiate(_smallTriangle, rightPosition, Quaternion.identity, _player.transform);
        _player.SmallRightTriangle = _smallRightTriangle;
        _smallLeftTriangle = Instantiate(_smallTriangle, leftPosition , Quaternion.identity, _player.transform);
        _player.SmallLeftTriangle = _smallLeftTriangle;

        //子機のかすり判定を左右2つ生成，親を子機本体に設定
        _smallRightTriangleGraze = Instantiate(_smallTriangleGrazeCollider.gameObject, rightPosition, Quaternion.identity, _player.transform);
        _smallRightTriangleGraze.transform.SetParent(_smallRightTriangle.transform);
        _smallLeftTriangleGraze = Instantiate(_smallTriangleGrazeCollider.gameObject, leftPosition, Quaternion.identity, _player.transform);
        _smallLeftTriangleGraze.transform.SetParent(_smallLeftTriangle.transform);

        //生成したオブジェクトをリストに追加
        activeTriangles.Add(_smallRightTriangle.gameObject);
        activeTriangles.Add(_smallLeftTriangle.gameObject);
        
        //リスト内の要素が3個以上なら，リストの要素が2個になるまで古い要素を1つずつ削除
        while (activeTriangles.Count >= 3)
        {
            GameObject triangleToRemove = activeTriangles[0];
            activeTriangles.RemoveAt(0);
            Destroy(triangleToRemove.gameObject);
        }

        Debug.Log($"SpecialSkill {name}");
    }

    public override void ShiftSkill()
    {
        _triangleDestroyField = Instantiate(_destroyField.gameObject, _player.transform.position, Quaternion.identity, _player.transform);
        Destroy(_triangleDestroyField, _player.DashTime);

        _player.Dash();

        Debug.Log($"ShiftSkill {name}");
    }
}
