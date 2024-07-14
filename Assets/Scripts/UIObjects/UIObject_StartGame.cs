using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObject_StartGame : UIObject
{
    private SceneController _sceneController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void InvokeUIAction()
    {
        Debug.Log($"InvokeUIAction: {name}");

        if(_sceneController == null)
        {
            _sceneController = GameObject.Find("SceneController").GetComponent<SceneController>();
        }

        _sceneController.LoadNextScene(1);
    }
}
