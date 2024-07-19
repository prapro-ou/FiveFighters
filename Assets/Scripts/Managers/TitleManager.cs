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

    [SerializeField]
    private GameObject _transitionObject;

    private Animator _transitionAnimator;

    // Start is called before the first frame update
    void Start()
    {
        _transitionCanvas.SetActive(true);
        _transitionAnimator = _transitionObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void ShiftToGame()
    {
        StartCoroutine(_StartShiftToGame());
    }

    private IEnumerator _StartShiftToGame()
    {
        yield return StartCoroutine(_CloseTransition());
        _sceneController.LoadNextScene(1);
    }
}
