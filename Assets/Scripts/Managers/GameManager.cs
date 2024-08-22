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

    [SerializeField]
    private int _maxStateNumber;

    public int MaxStateNumber
    {
        get {return _maxStateNumber;}
        set {_maxStateNumber = value;}
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

    private SoundManager _soundManager;

    private Enemy _currentEnemy;

    public Enemy CurrentEnemy
    {
        get {return _currentEnemy;}
        set {_currentEnemy = value;}
    }

    private int _lockedEnemy; //DEBUG!!!!!!!!!!

    private float _timerValue;

    public float TimerValue
    {
        get {return _timerValue;}
        set
        {
            _timerValue = value;

            string formatedText = _FormatTime(_timerValue);
            _timerText.SetText(formatedText);
            _clearTimeText.SetText($"経過時間： {formatedText}");
            _deathTimeText.SetText($"経過時間： {formatedText}");
            _gameClearTimeText.SetText($"経過時間： {formatedText}");
        }
    }

    private float _timerSumValue;

    public float TimerSumValue
    {
        get {return _timerSumValue;}
        set
        {
            _timerSumValue = value;

            string formatedText = _FormatTime(_timerSumValue);
            // _timerSumText.SetText(formatedText);
            _clearSumTimeText.SetText($"合計時間： {formatedText}");
            _deathSumTimeText.SetText($"合計時間： {formatedText}");
            _gameClearSumTimeText.SetText($"合計時間： {formatedText}");
        }
    }

    private Coroutine _timerCoroutine;

    public Coroutine TimerCoroutine
    {
        get {return _timerCoroutine;}
        set {_timerCoroutine = value;}
    }

    [SerializeField]
    private List<Enemy> _enemies;

    [SerializeField]
    private List<int> _moneysPerBattle;

    [SerializeField]
    private int _moneyPerBonusTime;

    [SerializeField]
    private float _bonusTimeBorder;

    [SerializeField]
    private int _firstMoney;

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
    private TMP_Text _timerText;

    [SerializeField]
    private TMP_Text _clearTimeText;

    [SerializeField]
    private TMP_Text _deathTimeText;

    [SerializeField]
    private TMP_Text _gameClearTimeText;

    // [SerializeField]
    // private TMP_Text _timerText;

    [SerializeField]
    private TMP_Text _clearSumTimeText;

    [SerializeField]
    private TMP_Text _deathSumTimeText;

    [SerializeField]
    private TMP_Text _gameClearSumTimeText;

    [SerializeField]
    private ProgressBar _clearProgressBar;

    [SerializeField]
    private ProgressBar _deathProgressBar;

    [SerializeField]
    private ProgressBar _gameClearProgressBar;

    [SerializeField]
    private TMP_Text _clearCoinText;

    [SerializeField]
    private Canvas _clearResultCanvas;

    [SerializeField]
    private GameObject _clearResultButton;

    [SerializeField]
    private Canvas _deathResultCanvas;

    [SerializeField]
    private GameObject _deathResultButton;

    [SerializeField]
    private Canvas _gameClearResultCanvas;

    [SerializeField]
    private GameObject _gameClearResultButton;


    [SerializeField]
    private Canvas _uICanvas;

    // Start is called before the first frame update
    void Start()
    {
        StateNumber = 0;

        IsRunningShift = false;

        _lockedEnemy = -1;

        TimerValue = 0f;
        TimerSumValue = 0f;

        _FirstShiftToShop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void _FirstShiftToShop()
    {
        _ShiftObjects(0);

        _player.Money = _firstMoney;

        _PlaySound("Transition");

        // _PlayBGM("Shop");

        StartCoroutine(_overlayManager.OpenTransition());
    }

    public IEnumerator ShiftToShop()
    {
        Debug.Log("ShiftToShop");
        _player.ResetStatusInShop();

        yield return StartCoroutine(_overlayManager.CloseTransition());

        _PlayBGM("Shop");

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

        _PlaySound("Transition");

        yield return StartCoroutine(_overlayManager.CloseTransition());

        _StopBGM();

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

        if(CurrentEnemy.BgmName != null)
        {
            _PlayBGM(CurrentEnemy.BgmName);
        }
        
        CurrentEnemy.StartAttacking();

        TimerCoroutine = StartCoroutine(_StartTimer());

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

                //タイマーのリセット、合計タイマーの加算
                TimerValue = 0;

                break;
            }
            case 1:
            {
                //カメラの移動
                _cameraManager.MoveToPoint(_battleCameraTransform.position);

                //Playerの移動
                _player.transform.position = _battlePlayerTransform.position;

                //背景色の変更
                _backgroundImage.color = enemy.BackgroundColor;

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

        _StopTimer();
        TimerSumValue += TimerValue;

        StartCoroutine(_PopUpOnClear());
    }

    private IEnumerator _PopUpOnClear()
    {
        //InputManagerのモードをUIに移行する
        _inputManager.SwitchCurrentActionMap("UI");

        //Stateを進める
        StateNumber += 1;

        //カメラを敵に寄せる
        _cameraManager.MoveToPoint(CurrentEnemy.transform.position);
        _cameraManager.SetSize(3);

        _overlayManager.DisappearUICanvas();
        
        //フラッシュ
        StartCoroutine(_overlayManager.PlayWhiteFlash());

        //振動
        StartCoroutine(_cameraManager.Vibrate(0.4f, 0.2f));

        //BGMを止める
        _StopBGM();

        //消滅演出待ち
        yield return StartCoroutine(CurrentEnemy.StartDeathAnimation());

        //カメラを戻す
        StartCoroutine(_overlayManager.AppearUICanvas(0.2f));
        StartCoroutine(_cameraManager.SetSizeOnCurve(5));
        yield return StartCoroutine(_cameraManager.MoveToPointOnCurve(new Vector3(0, 0, -10)));

        //ゲームクリアか分岐
        if(StateNumber != MaxStateNumber)
        {
            //クリア音
            _PlaySound("StageClear");

            //「Stage Clear」を出現させる(アニメーションを仕込み、左から右に移動させる)
            _overlayManager.SpawnStageClearText();
            yield return new WaitForSeconds(1.5f);

            //リザルトキャンバスを表示する(リザルトキャンバスの情報を更新する)
            _clearCoinText.SetText($"獲得コイン：{CalculateGiveMoney()} ({_moneysPerBattle[StateNumber]}+{(int)(Mathf.Max(0, (-(TimerValue - _bonusTimeBorder)) / _moneyPerBonusTime))})");
            
            _clearResultCanvas.gameObject.SetActive(true);

            _eventSystem.SetSelectedGameObject(_clearResultButton);

            _GiveMoney();

            yield return StartCoroutine(_clearProgressBar.UpdateProgress());

            yield return new WaitForSeconds(0.5f);

            yield return null;
        }
        else
        {
            //ゲームクリア音
            _PlaySound("GameClear");

            //「Game Clear」を出現させる(アニメーションを仕込み、左から右に移動させる)
            _overlayManager.SpawnGameClearText();
            yield return new WaitForSeconds(1.5f);

            //リザルトキャンバスを表示する(リザルトキャンバスの情報を更新する)
            // _clearCoinText.SetText($"獲得コイン：{CalculateGiveMoney()} ({_moneysPerBattle[StateNumber]}+{(int)(Mathf.Max(0, (-(TimerValue - _bonusTimeBorder)) / _moneyPerBonusTime))})");
            
            _gameClearResultCanvas.gameObject.SetActive(true);

            _eventSystem.SetSelectedGameObject(_gameClearResultButton);

            yield return StartCoroutine(_gameClearProgressBar.UpdateProgress());

            yield return null;
        }
    }

    public void ShiftToShopWithClearResult()
    {
        //InputManagerのモードをPlayerに移行する
        _inputManager.SwitchCurrentActionMap("Player");

        _clearResultCanvas.gameObject.SetActive(false);

        //トランジション音
        _PlaySound("Transition");

        StartCoroutine(ShiftToShop());
    }

    public void DiePlayer()
    {
        Debug.Log("DiePlayer");

        _StopTimer();
        TimerSumValue += TimerValue;

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

        //ゲームオーバー音
        _PlaySound("Death");

        //「Game Over」を出現させる(アニメーションを仕込み、左から右に移動させる)
        _overlayManager.SpawnGameOverText();
        yield return new WaitForSeconds(1.5f);

        //リザルトキャンバスを表示する(リザルトキャンバスの情報を更新する)
        _deathProgressBar.SetSliderOnDeath();

        _deathResultCanvas.gameObject.SetActive(true);

        _eventSystem.SetSelectedGameObject(_deathResultButton);

        yield return StartCoroutine(_deathProgressBar.UpdateProgressOnDeath());

        yield return null;
    }

    public void LoadTitleWithDeathResult()
    {
        //InputManagerのモードをPlayerに移行する
        _inputManager.SwitchCurrentActionMap("Player");

        _clearResultCanvas.gameObject.SetActive(false);

        StartCoroutine(_LoadSceneAfterTransition(0));
    }

    public void LoadMainWithResult()
    {
        //InputManagerのモードをPlayerに移行する
        _inputManager.SwitchCurrentActionMap("Player");

        _gameClearResultCanvas.gameObject.SetActive(false);

        StartCoroutine(_LoadSceneAfterTransition(1));
    }

    private IEnumerator _LoadSceneAfterTransition(int number)
    {
        //トランジション音
        _PlaySound("Transition");

        yield return StartCoroutine(_overlayManager.CloseTransition());

        _sceneController.LoadNextScene(number);
    }

    private void _GiveMoney()
    {
        int moneyValue = CalculateGiveMoney();
        _player.AddMoney(moneyValue);
    }

    private int CalculateGiveMoney()
    {
        return _moneysPerBattle[StateNumber] + (int)(Mathf.Max(0, (-(TimerValue - _bonusTimeBorder)) / _moneyPerBonusTime));
    }

    private IEnumerator _StartTimer()
    {
        Debug.Log("StartTimer");
        while(true)
        {
            TimerValue += Time.deltaTime;
            yield return null;
        }
    }

    private void _StopTimer()
    {
        Debug.Log("StopTimer");
        StopCoroutine(TimerCoroutine);
    }

    private string _FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        int milliseconds = Mathf.FloorToInt((time * 100F) % 100F);

        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    private void _PlaySound(string name)
    {
        if(_soundManager == null)
        {
            _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        }

        _soundManager.PlaySound(name);
    }

    private void _PlayBGM(string name)
    {
        if(_soundManager == null)
        {
            _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        }

        _soundManager.PlayBGM(name);
    }

    private void _StopBGM()
    {
        if(_soundManager == null)
        {
            _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        }

        _soundManager.StopBGM();
    }
}
