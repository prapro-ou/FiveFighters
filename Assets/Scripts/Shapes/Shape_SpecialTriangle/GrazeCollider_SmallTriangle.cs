using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrazeCollider_SmallTriangle : MonoBehaviour
{
    private Player _player;

    private int _grazeCount;

    private SoundManager _soundManager;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _grazeCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if(_grazeCount >= 1)
        {
            _PlaySound("Graze");
        }
        _player.PrimaryGrazeCount += _grazeCount;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        _grazeCount += 1;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        _grazeCount -= 1;
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
