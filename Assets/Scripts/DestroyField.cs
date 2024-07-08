using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyField : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //触れてきた弾を破壊
    void OnTriggerEnter2D(Collider2D collider)
    {
        Destroy(collider.gameObject);
    }
}
