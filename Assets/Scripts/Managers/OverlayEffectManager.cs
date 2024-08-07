using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayEffectManager : MonoBehaviour
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
    private Animator _transitionAnimator;

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
            Debug.Log(color.a);
            _redFlashImage.color = color;
            yield return null;
        }

        color.a = 0;
        _redFlashImage.color = color;
    }
}
