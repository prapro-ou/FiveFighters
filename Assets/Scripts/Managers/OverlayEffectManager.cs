using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayEffectManager : MonoBehaviour
{
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

    public IEnumerator OpenTransition()
    {
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
}
