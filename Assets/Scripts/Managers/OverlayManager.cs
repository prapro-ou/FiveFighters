using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _transitionCanvas;

    [SerializeField]
    private GameObject _transitionObject;

    [SerializeField]
    private Image _whiteFlashImage;

    [SerializeField]
    private Image _redFlashImage;

    [SerializeField]
    private GameObject _stageClearTextPrefab;

    [SerializeField]
    private GameObject _gameOverTextPrefab;

    [SerializeField]
    private Animator _transitionAnimator;

    [SerializeField]
    private Canvas _uICanvas;

    [SerializeField]
    private Canvas _rightUICanvas;

    [SerializeField]
    private AnimationCurve _curve;

    // Start is called before the first frame update
    void Start()
    {
        _transitionCanvas.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator OpenTransition()
    {
        //Delay
        yield return new WaitForSeconds(0.1f);

        _transitionAnimator.SetBool("Close", false);

        AnimatorStateInfo animationState = _transitionAnimator.GetCurrentAnimatorStateInfo(0);
        float animationLength = animationState.length;

        yield return new WaitForSeconds(animationLength);
    }

    public IEnumerator CloseTransition()
    {
        _transitionAnimator.SetBool("Close", true);

        AnimatorStateInfo animationState = _transitionAnimator.GetCurrentAnimatorStateInfo(0);
        float animationLength = animationState.length;

        yield return new WaitForSeconds(animationLength);
    }

    public IEnumerator PlayWhiteFlash()
    {
        Color color = _whiteFlashImage.color;

        for(float i = 0; i <= 0.5f; i += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0, 1, _curve.Evaluate(i * 2));
            _whiteFlashImage.color = color;
            yield return null;
        }

        color.a = 0;
        _whiteFlashImage.color = color;
    }

    public IEnumerator PlayRedFlash()
    {
        Color color = _redFlashImage.color;

        for(float i = 0; i <= 0.5f; i += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0, 1, _curve.Evaluate(i * 2));
            _redFlashImage.color = color;
            yield return null;
        }

        color.a = 0;
        _redFlashImage.color = color;
    }

    public void SpawnStageClearText()
    {
        GameObject text = Instantiate(_stageClearTextPrefab, Vector3.zero, Quaternion.identity, _uICanvas.transform);
        Destroy(text, 5f);
    }

    public void SpawnGameOverText()
    {
        GameObject text = Instantiate(_gameOverTextPrefab, Vector3.zero, Quaternion.identity, _uICanvas.transform);
        Destroy(text, 5f);
    }

    public void DisappearUICanvas()
    {
        CanvasGroup canvasGroup = _uICanvas.GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0;
    }

    public void EnableRightUICanvas()
    {
        _rightUICanvas.enabled = true;
    }

    public void DisenableRightUICanvas()
    {
        _rightUICanvas.enabled = false;
    }

    public IEnumerator AppearUICanvas(float duration)
    {
        CanvasGroup canvasGroup = _uICanvas.GetComponent<CanvasGroup>();

        for(float i = 0; i <= duration; i += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, _curve.Evaluate(i / duration));
            yield return null;
        }

        canvasGroup.alpha = 1;
    }
}
