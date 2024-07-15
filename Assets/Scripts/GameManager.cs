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

    [SerializeField]
    private Transform _battleEnemyTransform;

    [SerializeField]
    private GameObject _transitionCanvas;

    [SerializeField]
    private GameObject _transitionObject;

    private Animator _transitionAnimator;

    // Start is called before the first frame update
    void Start()
    {
        StateNumber = 0;

        _transitionCanvas.SetActive(true);
        _transitionAnimator = _transitionObject.GetComponent<Animator>();

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

        if(_enemies.Count == 0)
        {
            Debug.Log("No Enemies!");
            yield break;
        }
        
        Enemy nextEnemy = _enemies[Random.Range(0, _enemies.Count)];

        yield return StartCoroutine(_CloseTransition());

        _ShiftObjects(1);

        yield return StartCoroutine(_OpenTransition());

        //敵の出現
        _SpawnEnemy(nextEnemy);
        _enemies.Remove(nextEnemy);
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

                //背景色の変更
                _backgroundImage.color = _shopImageColor;

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

                //背景色の変更
                _backgroundImage.color = _battleImageColor;

                break;
            }
        }
    }

    private IEnumerator _OpenTransition()
    {
        _transitionAnimator.SetBool("Close", false);

        AnimatorStateInfo animationState = _transitionAnimator.GetCurrentAnimatorStateInfo(0);
        float animationLength = animationState.length;

        yield return new WaitForSeconds(animationLength);
    }

    private IEnumerator _CloseTransition()
    {
        _transitionAnimator.SetBool("Close", true);

        AnimatorStateInfo animationState = _transitionAnimator.GetCurrentAnimatorStateInfo(0);
        float animationLength = animationState.length;

        yield return new WaitForSeconds(animationLength);
    }

    public void DebugShift_GoShop()
    {
        StartCoroutine(ShiftToShop());
    }

    private void _SpawnEnemy(Enemy enemy)
    {
        Instantiate(enemy, _battleEnemyTransform.position, Quaternion.identity);
    }
}
