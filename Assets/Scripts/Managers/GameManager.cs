using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int _stateNumber;

    public int StateNumber
    {
        get {return _stateNumber;}
        set {_stateNumber = value;}
    }

    private bool _isRunningShift;

    public bool IsRunningShift
    {
        get {return _isRunningShift;}
        set {_isRunningShift = value;}
    }

    [SerializeField]
    private Player _player;

    [SerializeField]
    private PlayerInput _inputManager;

    [SerializeField]
    private EventSystem _eventSystem;

    [SerializeField]
    private SceneController _sceneController;

    [SerializeField]
    private CameraManager _cameraManager;

    [SerializeField]
    private OverlayManager _overlayManager;

    private Enemy _currentEnemy;

    public Enemy CurrentEnemy
    {
        get {return _currentEnemy;}
        set {_currentEnemy = value;}
    }

    private int _lockedEnemy; //DEBUG!!!!!!!!!!

    [SerializeField]
    private List<Enemy> _enemies;

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
    private Transform _battleEnemyTransform;

    [SerializeField]
    private TMP_Text _enemyNameText;

    [SerializeField]
    private Canvas _clearResultCanvas;

    [SerializeField]
    private GameObject _clearResultButton;

    [SerializeField]
    private Canvas _deathResultCanvas;

    [SerializeField]
    private GameObject _deathResultButton;

    [SerializeField]
    private Canvas _uICanvas;

    // Start is called before the first frame update
    void Start()
    {
        StateNumber = 0;

        IsRunningShift = false;

        _lockedEnemy = -1;

        _FirstShiftToShop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void _FirstShiftToShop()
    {
        _ShiftObjects(0);

        _player.AddMoney(3);

        StartCoroutine(_overlayManager.OpenTransition());
    }

    public IEnumerator ShiftToShop()
    {
        Debug.Log("ShiftToShop");
        _player.ResetStatusInShop();

        yield return StartCoroutine(_overlayManager.CloseTransition());

        _ShiftObjects(0);

        StartCoroutine(_overlayManager.OpenTransition());
    }

    public IEnumerator ShiftToBattle()
    {
        Debug.Log("ShiftToBattle");

        if(IsRunningShift == true)
        {
            Debug.Log("Already Shifting");
            yield break;
        }

        if(_enemies.Count == 0)
        {
            Debug.Log("No Enemies!");
            yield break;
        }

        IsRunningShift = true;

        Enemy nextEnemy = _enemies[Random.Range(0, _enemies.Count)];

        yield return StartCoroutine(_overlayManager.CloseTransition());

        _ShiftObjects(1, nextEnemy);

        yield return StartCoroutine(_overlayManager.OpenTransition());

        //敵の出現
        if(_lockedEnemy != -1) //DEBUG!!!!!!!!!!!!!!!
        {
            Debug.Log($"DEBUG: SpawnLockedEnemy: {_enemies[_lockedEnemy]}");
            _SpawnEnemy(_enemies[_lockedEnemy]);
            _lockedEnemy = -1;
        }
        else
        {
            _SpawnEnemy(nextEnemy);
            _enemies.Remove(nextEnemy);
        }
        
        //登場演出を挟む
        yield return StartCoroutine(CurrentEnemy.StartSpawnAnimation());

        CurrentEnemy.StartAttacking();

        IsRunningShift = false;
    }

    private void _ShiftObjects(int state, Enemy enemy = null)
    {
        switch(state)
        {
            case 0:
            {
                //カメラの移動
                _cameraManager.MoveToPoint(_shopCameraTransform.position);

                //Playerの移動
                _player.transform.position = _shopPlayerTransform.position;

                //背景色の変更
                _backgroundImage.color = _shopImageColor;

                //右側UIの非表示
                _overlayManager.DisenableRightUICanvas();

                break;
            }
            case 1:
            {
                //カメラの移動
                _cameraManager.MoveToPoint(_battleCameraTransform.position);

                //Playerの移動
                _player.transform.position = _battlePlayerTransform.position;

                //背景色の変更
                _backgroundImage.color = _battleImageColor;

                //右側UIの表示
                _overlayManager.EnableRightUICanvas();

                //敵名テキストの変更
                _enemyNameText.SetText(enemy.Name);

                break;
            }
        }
    }

    public void DEBUG_GoShop()
    {
        StartCoroutine(ShiftToShop());
    }

    public void DEBUG_lockEnemy(int enemy)
    {
        _lockedEnemy = enemy;
        Debug.Log($"DEBUG: Locked Enemy as {_enemies[_lockedEnemy]}");
    }

    public void DEBUG_killEnemy()
    {
        CurrentEnemy.TakeDamage(1000000);
        Debug.Log($"DEBUG: KillEnemy(HugeDamage)");
    }

    private void _SpawnEnemy(Enemy enemy)
    {
        CurrentEnemy = Instantiate(enemy, _battleEnemyTransform.position, Quaternion.identity);
    }

    public void ClearStage()
    {
        Debug.Log("ClearStage");

        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("EnemyBullet"))
        {
            Destroy(obj);
        }

        StartCoroutine(_PopUpOnClear());
    }

    private IEnumerator _PopUpOnClear()
    {
        //InputManagerのモードをUIに移行する
        _inputManager.SwitchCurrentActionMap("UI");

        //カメラを敵に寄せる
        _cameraManager.MoveToPoint(CurrentEnemy.transform.position);
        _cameraManager.SetSize(3);

        _overlayManager.DisappearUICanvas();
        
        //フラッシュ
        StartCoroutine(_overlayManager.PlayWhiteFlash());

        //振動
        StartCoroutine(_cameraManager.Vibrate(0.4f, 0.2f));

        //消滅演出待ち
        yield return StartCoroutine(CurrentEnemy.StartDeathAnimation());

        //カメラを戻す
        StartCoroutine(_overlayManager.AppearUICanvas(0.2f));
        StartCoroutine(_cameraManager.SetSizeOnCurve(5));
        yield return StartCoroutine(_cameraManager.MoveToPointOnCurve(new Vector3(0, 0, -10)));

        //「Stage Clear」を出現させる(アニメーションを仕込み、左から右に移動させる)
        _overlayManager.SpawnStageClearText();
        yield return new WaitForSeconds(1.5f);

        //リザルトキャンバスを表示する(リザルトキャンバスの情報を更新する)
        _clearResultCanvas.gameObject.SetActive(true);

        _eventSystem.SetSelectedGameObject(_clearResultButton);

        yield return null;
    }

    public void ShiftToShopWithClearResult()
    {
        //InputManagerのモードをPlayerに移行する
        _inputManager.SwitchCurrentActionMap("Player");

        _clearResultCanvas.gameObject.SetActive(false);

        StartCoroutine(ShiftToShop());
    }

    public void DiePlayer()
    {
        Debug.Log("DiePlayer");

        StartCoroutine(_PopUpOnDeath());
    }

    private IEnumerator _PopUpOnDeath()
    {
        //InputManagerのモードをUIに移行する
        _inputManager.SwitchCurrentActionMap("UI");

        //カメラをプレイヤーに寄せる
        _cameraManager.MoveToPoint(_player.transform.position);
        _cameraManager.SetSize(3);

        _overlayManager.DisappearUICanvas();

        //敵の行動を止める
        CurrentEnemy.StopAllCoroutines();

        //フラッシュ
        StartCoroutine(_overlayManager.PlayRedFlash());

        //振動
        StartCoroutine(_cameraManager.Vibrate(0.4f, 0.2f));

        //Playerの演出を待つ
        yield return StartCoroutine(_player.StartDeathAnimation());

        //カメラを戻す
        StartCoroutine(_overlayManager.AppearUICanvas(0.2f));
        StartCoroutine(_cameraManager.SetSizeOnCurve(5));
        yield return StartCoroutine(_cameraManager.MoveToPointOnCurve(new Vector3(0, 0, -10)));

        //「Game Over」を出現させる(アニメーションを仕込み、左から右に移動させる)
        _overlayManager.SpawnGameOverText();
        yield return new WaitForSeconds(1.5f);

        //リザルトキャンバスを表示する(リザルトキャンバスの情報を更新する)
        _deathResultCanvas.gameObject.SetActive(true);

        _eventSystem.SetSelectedGameObject(_deathResultButton);

        yield return null;
    }

    public void LoadTitleWithDeathResult()
    {
        //InputManagerのモードをPlayerに移行する
        _inputManager.SwitchCurrentActionMap("Player");

        _clearResultCanvas.gameObject.SetActive(false);

        StartCoroutine(_LoadTitleAfterTransition());
    }

    private IEnumerator _LoadTitleAfterTransition()
    {
        yield return StartCoroutine(_overlayManager.CloseTransition());

        _sceneController.LoadNextScene(0);
    }
}
