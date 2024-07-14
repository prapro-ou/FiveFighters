using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int _stateNumber;

    public int StateNumber
    {
        get {return _stateNumber;}
        set {_stateNumber = value;}
    }

    [SerializeField]
    private Player _player;

    [SerializeField]
    private List<Enemy> _enemies;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private Image _backgroundImage;

    [SerializeField]
    private Color _battleImageColor;

    [SerializeField]
    private Color _shopImageColor;

    [SerializeField]
    private Transform _battleCameraTransform;

    [SerializeField]
    private Transform _shopCameraTransform;

    [SerializeField]
    private Transform _battlePlayerTransform;

    [SerializeField]
    private Transform _shopPlayerTransform;

    [SerializeField]
    private RectTransform _battlePlayArea;

    [SerializeField]
    private RectTransform _shopPlayArea;

    // Start is called before the first frame update
    void Start()
    {
        StateNumber = 0;

        _FirstShiftToShop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void _FirstShiftToShop()
    {
        _ShiftObjects(0);

        StartCoroutine(_OpenTransition());
    }

    public IEnumerator ShiftToShop()
    {
        Debug.Log("ShiftToShop");

        yield return StartCoroutine(_CloseTransition());

        _ShiftObjects(0);

        StartCoroutine(_OpenTransition());
    }

    public IEnumerator ShiftToBattle()
    {
        Debug.Log("ShiftToBattle");

        yield return StartCoroutine(_CloseTransition());

        _ShiftObjects(1);

        yield return StartCoroutine(_OpenTransition());

        //敵の出現
    }

    private void _ShiftObjects(int state)
    {
        switch(state)
        {
            case 0:
            {
                //カメラの移動
                _camera.transform.position = _shopCameraTransform.position;

                //PlayerのPlayAreaの設定
                _player.PlayArea = _shopPlayArea;

                //Playerの移動
                _player.transform.position = _shopPlayerTransform.position;

                break;
            }
            case 1:
            {
                //カメラの移動
                _camera.transform.position = _battleCameraTransform.position;

                //PlayerのPlayAreaの設定
                _player.PlayArea = _battlePlayArea;

                //Playerの移動
                _player.transform.position = _battlePlayerTransform.position;
                break;
            }
        }
    }

    private IEnumerator _OpenTransition()
    {
        yield return null;
    }

    private IEnumerator _CloseTransition()
    {
        yield return null;
    }
}
