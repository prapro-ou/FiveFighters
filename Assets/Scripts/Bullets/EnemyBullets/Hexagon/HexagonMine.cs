using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonMine : EnemyBullet
{
    private SoundManager _soundManager;

    [SerializeField]
    private float _delay;

    [SerializeField]
    private float _maxScale;

    [SerializeField]
    private float _times;

    [SerializeField]
    private float _duration;

    [SerializeField]
    private AnimationCurve _expandCurve;

    [SerializeField]
    private AnimationCurve _shrinkCurve;

    private float _currentScale;

    private float _segmentValue;

    [SerializeField]
    private GameObject _effectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Bomb());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Bomb()
    {
        _currentScale = transform.localScale.x;

        _segmentValue = (_maxScale - _currentScale) / _times;

        yield return new WaitForSeconds(_delay);

        for(int i = 0; i < _times; i++)
        {
            _PlaySound("NormalBullet");
            yield return StartCoroutine(_SetScaleOnCurve(_currentScale + _segmentValue, _duration));
            // yield return StartCoroutine(_SetScaleOnCurve(_currentScale, 0.2f));
            _currentScale += _segmentValue;
        }

        yield return StartCoroutine(_SetScaleOnCurve(0f, 0.2f, true));

        Destroy(this.gameObject);
    }

    private IEnumerator _SetScaleOnCurve(float endScale, float duration = 0.2f, bool isShrink = false)
    {
        float startScale = transform.localScale.x;
        float lerpScale = startScale;

        if(!isShrink)
        {
            for(float i = 0; i <= duration; i += Time.deltaTime)
            {
                lerpScale = Mathf.Lerp(startScale, endScale, _expandCurve.Evaluate(i / duration));
                transform.localScale = new Vector3(lerpScale, lerpScale, 1);
                yield return null;
            }
        }
        else
        {
            for(float i = 0; i <= duration; i += Time.deltaTime)
            {
                lerpScale = Mathf.Lerp(startScale, endScale, _shrinkCurve.Evaluate(i / duration));
                transform.localScale = new Vector3(lerpScale, lerpScale, 1);
                yield return null;
            }
        }

        transform.localScale = new Vector3(endScale, endScale, 1);
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
