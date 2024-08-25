using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private AnimationCurve _curve;

    private SoundManager _soundManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator UpdateProgress()
    {   
        float progress = ((float)(_gameManager.StateNumber)) / (float)_gameManager.MaxStateNumber; 
        yield return StartCoroutine(MoveSlider(progress));
    }

    public IEnumerator UpdateProgressOnDeath()
    {   
        float enemyHpRatio = (float)((float)_gameManager.CurrentEnemy.HitPoint / (float)_gameManager.CurrentEnemy.MaxHitPoint);
        float progress = (((float)(_gameManager.StateNumber)) / (float)_gameManager.MaxStateNumber) + ((1f - enemyHpRatio) / (float)_gameManager.MaxStateNumber); 
        yield return StartCoroutine(MoveSlider(progress));
    }

    public void SetSliderOnDeath()
    {
        float progress = ((float)(_gameManager.StateNumber)) / (float)_gameManager.MaxStateNumber;
        _slider.value = progress;
    }

    private IEnumerator MoveSlider(float endRatio, float duration = 2f)
    {
        float startRatio = _slider.value;

        Debug.Log($"MoveSlider from {startRatio} to {endRatio}");

        for(float i = 0; i <= duration; i += Time.deltaTime)
        {
            _PlaySound("Graze");
            _slider.value = Mathf.Lerp(startRatio, endRatio, _curve.Evaluate(i / duration));
            yield return null;
        }

        _slider.value = endRatio;
    }

    private void _PlaySound(string name)
    {
        if(_soundManager == null)
        {
            _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        }

        _soundManager.PlaySound(name);
    }
}