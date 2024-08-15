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
        int n = 1;
        int rnd = Random.Range(0,2);
        for(int i = 0; i < 86; ++i)
        {
            int lr;
            if(rnd == 0)
                lr = -1;
            else
                lr = 1;

            if(i > 20)
                _text.color -= new Color32(0, 0, 0, 4);

            if(i == 43)
                n = -1;

            this.transform.position += new Vector3(0.03f * lr, 0.02f * n, 0);
            yield return new WaitForSeconds(0.001f);
        }

        Destroy(this.gameObject);
        yield break;
    }
}
