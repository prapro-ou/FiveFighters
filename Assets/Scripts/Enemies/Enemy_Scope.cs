using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Scope : MonoBehaviour
{
    private SpriteRenderer sp;

    // Start is called before the first frame update
    void Start()
    {
        sp = this.GetComponent<SpriteRenderer>();
        StartCoroutine("_Flashing");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator _Flashing()
    {
        //0.1秒おきにSpriteRendererの有効・無効を切り替えて点滅させる
        for(int i = 0; i < 10; i++)
        {
            if(i % 2 == 1)
                sp.enabled = false;
            else
                sp.enabled = true;

            yield return new WaitForSeconds(0.1f);
        }

        Destroy(this.gameObject);
        yield return null;
    }
}
