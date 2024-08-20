using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrazeCollider : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private List<PlayerShape> _ownShapes;

    [SerializeField]
    private SoundManager _soundManager;

    [SerializeField]
    private GameObject _playerAvailableSpecialCircleEffectPrefab;

    [SerializeField]
    private GameObject _playerAvailableSpecialTriangleEffectPrefab;

    [SerializeField]
    private GameObject _playerAvailableSpecialSquareEffectPrefab;

    private GameObject _shapeEffect;

    private int _grazeCount;

    public int GrazeCount
    {
        get {return _grazeCount;}
        set {_grazeCount = value;}
    }

    private bool _specialSkillFlag = false;
    public bool SpecialSkillFlag
    {
        get {return _specialSkillFlag;}
        set {_specialSkillFlag = value;}
    }
    // Start is called before the first frame update
    void Start()
    {
        GrazeCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        _player.PrimaryGrazeCount += GrazeCount;
        _player.SpecialGrazeCount += GrazeCount;

        if(_player.SpecialGrazeCount >= _player.MyShape.SpecialSkillCost && SpecialSkillFlag == false)
        {
            SpecialSkillFlag = true;

            if(_player.MyShape == _ownShapes[0])
                _shapeEffect = _playerAvailableSpecialCircleEffectPrefab;
            else if(_player.MyShape == _ownShapes[1])
                _shapeEffect = _playerAvailableSpecialTriangleEffectPrefab;
            else if(_player.MyShape == _ownShapes[2])
                _shapeEffect = _playerAvailableSpecialSquareEffectPrefab;

            GameObject specialEffect = Instantiate(_shapeEffect, _player.transform.position, Quaternion.identity, _player.transform);
            _PlaySound("Special");
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        GrazeCount += 1;
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        GrazeCount -= 1;
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
