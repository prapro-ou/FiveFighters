using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageText : MonoBehaviour
{
    private TextMeshPro _text;

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
        for(int i = 0; i < 255; ++i)
        {
            _text.color -= new Color32(0, 0, 0, 1);
            yield return new WaitForSeconds(0.001f);
        }

        Destroy(this.gameObject);
        yield break;
    }
}
