using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private SceneController _sceneController;

    [SerializeField]
    private GameObject _transitionCanvas;

    private Canvas _volumeSliderCanvas;

    private Canvas _tutorialCanvas;

    [SerializeField]
    private GameObject _transitionObject;

    private Animator _transitionAnimator;

    // Start is called before the first frame update
    void Start()
    {
        _transitionCanvas.SetActive(true);
        _transitionAnimator = _transitionObject.GetComponent<Animator>();

        StartCoroutine(_OpenTransition());
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

        if(_tutorialCanvas == null)
        {
            _tutorialCanvas = GameObject.Find("Canvas_Tutorial").GetComponent<Canvas>();
        }
        _tutorialCanvas.enabled = false;

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
}
