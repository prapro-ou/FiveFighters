using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageText : MonoBehaviour
{
    private TextMeshPro _text;

    [SerializeField]
    private float _duration;

    [SerializeField]
    private AnimationCurve _animationCurve;

    // Start is called before the first frame update
    void Start()
    {
        _text = this.GetComponent<TextMeshPro>();
        StartCoroutine("_DisplayDamage");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator _DisplayDamage()
    {
        //上昇下降制御用
        // int n = 1;
        //動く方向を決めるための乱数．0なら左，1なら右．
        float rnd = Random.Range(-1f, 1f);

        float fadePoint = _duration / 2;

        byte fadeRatio = (byte)(Time.deltaTime * 510);

        for(float i = 0; i < _duration; i += Time.deltaTime)
        {
            // int lr;
            // if(rnd == 0)
            //     lr = -1;
            // else
            //     lr = 1;

            //しばらくたってから透明度を下げ始める
            if(i > (fadePoint))
                _text.color -= new Color32(0, 0, 0, fadeRatio);

            // //ループの中間で下降開始
            // if(i == 43)
            //     n = -1;

            this.transform.position += new Vector3(rnd * Time.deltaTime, _animationCurve.Evaluate(i / _duration) * Time.deltaTime, 0);
            yield return null;
        }

        Destroy(this.gameObject);
        yield break;
    }
}
