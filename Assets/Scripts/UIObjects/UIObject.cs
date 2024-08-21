using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIObject : MonoBehaviour
{
    public SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void InvokeUIAction();

    public void PlaySound(string name)
    {
        if(soundManager == null)
        {
            soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        }

        soundManager.PlaySound(name);
    }
}
