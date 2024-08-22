using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_VolumeSlider : MonoBehaviour
{
    public static bool isLoad = false;

    void Awake()
    {
        if(isLoad == true)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            isLoad = true;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
