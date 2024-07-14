using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObject_NextBattle : UIObject
{
    private SceneController _sceneController;

    [SerializeField]
    private GameManager _gameManager;

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

        StartCoroutine(_gameManager.ShiftToBattle());
    }
}
