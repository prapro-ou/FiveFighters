using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICollider : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    private UIObject _touchingUI;

    public UIObject TouchingUI
    {
        get {return _touchingUI;}
        set {_touchingUI = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        TouchingUI = null;
        _player.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        TouchingUI = collider.gameObject.GetComponent<UIObject>();
        if(TouchingUI != null)
            _player.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
        Debug.Log($"TouchingUI: {TouchingUI}");
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        TouchingUI = null;
        if(TouchingUI == null)
            _player.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
        Debug.Log("Exit UI");
    }
}
