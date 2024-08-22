using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private SceneController _sceneController;

    [SerializeField]
    private EventSystem _eventSystem;

    [SerializeField]
    private GameObject _transitionCanvas;

    private Canvas _volumeSliderCanvas;

    [SerializeField]
    private Canvas _tutorialCanvas;

    [SerializeField]
    private List<GameObject> _images;

    private int page = 0;

    [SerializeField]
    private GameObject _previousButton;

    [SerializeField]
    private GameObject _nextButton;

    [SerializeField]
    private GameObject _startGameButton;

    [SerializeField]
    private GameObject _transitionObject;

    private Animator _transitionAnimator;

    private SoundManager _soundManager;

    // Start is called before the first frame update
    void Start()
    {
        _transitionCanvas.SetActive(true);
        _transitionAnimator = _transitionObject.GetComponent<Animator>();
        _eventSystem.SetSelectedGameObject(_startGameButton);

        //チュートリアル用のCanvasを非表示に
        _tutorialCanvas.enabled = false;
        for(int i = 0; i < 10; ++i)
        {
            _images[i].SetActive(false);
        }

        StartCoroutine(_OpenTransition());

        _PlayBGM("Shop");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator _OpenTransition()
    {
        _transitionAnimator.SetBool("Close", false);

        if(_volumeSliderCanvas == null)
        {
            _volumeSliderCanvas = GameObject.Find("Canvas_VolumeSlider").GetComponent<Canvas>();
        }
        _volumeSliderCanvas.enabled = true;

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

    public void ShiftToGame()
    {
        _StopBGM();
        StartCoroutine(_StartShiftToGame());
    }

    private IEnumerator _StartShiftToGame()
    {
        yield return StartCoroutine(_CloseTransition());

        if(_volumeSliderCanvas == null)
        {
            _volumeSliderCanvas = GameObject.Find("Canvas_VolumeSlider").GetComponent<Canvas>();
        }
        _volumeSliderCanvas.enabled = false;

        _sceneController.LoadNextScene(1);
    }

    public void ShiftToTutorial()
    {
        _PlaySound("Submit");
        page = 0;
        _previousButton.SetActive(false);
        _tutorialCanvas.enabled = true;
        _images[page].SetActive(true);
        _eventSystem.SetSelectedGameObject(_nextButton);
    }

    public void PreviousSlide()
    {
        _PlaySound("Submit");
        if(page == _images.Count - 1)
            _nextButton.SetActive(true);

        _images[page].SetActive(false);
        page -= 1;
        _images[page].SetActive(true);

        if(page == 0)
        {
            _previousButton.SetActive(false);
            _eventSystem.SetSelectedGameObject(_nextButton);
        }
    }

    public void NextSlide()
    {
        _PlaySound("Submit");
        if(page == 0)
            _previousButton.SetActive(true);

        _images[page].SetActive(false);
        page += 1;
        _images[page].SetActive(true);

        if(page == _images.Count - 1)
        {
            _nextButton.SetActive(false);
            _eventSystem.SetSelectedGameObject(_previousButton);
        }
    }

    public void EndTutorial()
    {
        _PlaySound("Submit");
        _tutorialCanvas.enabled = false;
        _images[page].SetActive(false);
        _eventSystem.SetSelectedGameObject(_startGameButton);
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
