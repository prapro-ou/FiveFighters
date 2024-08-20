using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrazeCollider : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    private int _grazeCount;

    private SoundManager _soundManager;

    public int GrazeCount
    {
        get {return _grazeCount;}
        set {_grazeCount = value;}
    }
    // Start is called before the first frame update
    void Start()
    {
        GrazeCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if(GrazeCount > 0)
        {
            _PlaySound("Graze");
        }
        _player.PrimaryGrazeCount += GrazeCount;
        _player.SpecialGrazeCount += GrazeCount;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        GrazeCount += 1;
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        GrazeCount -= 1;
    }

    private void _PlaySound(string name)
    {
        if(_soundManager == null)
        {
            _soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        }

        _soundManager.PlaySound(name);
    }
}
