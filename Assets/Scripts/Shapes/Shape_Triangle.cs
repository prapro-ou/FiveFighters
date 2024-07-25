using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape_Triangle : PlayerShape
{
    [SerializeField]
    private Player _player;    
    private GameObject _triangleDestroyField;

    [SerializeField]
    private GameObject _trianglePrimaryBullet;

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
        
        Instantiate(_trianglePrimaryBullet, new Vector3(vec.x + 0.3f, vec.y, vec.z), Quaternion.identity);

        Instantiate(_trianglePrimaryBullet, new Vector3(vec.x - 0.3f, vec.y, vec.z), Quaternion.identity);
    }

    public override void SpecialSkill()
    {
        Debug.Log($"ShiftSkill {name}");
    }

    public override void ShiftSkill()
    {
        _triangleDestroyField = Instantiate(_destroyField.gameObject, _player.transform.position, Quaternion.identity, _player.transform);
        Destroy(_triangleDestroyField, 3.0f);
        Debug.Log($"ShiftSkill {name}");
    }
}
