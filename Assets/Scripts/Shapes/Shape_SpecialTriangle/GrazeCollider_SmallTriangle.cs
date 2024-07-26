using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrazeCollider_SmallTriangle : MonoBehaviour
{
    private Player _player;

    private int _grazeCount;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _grazeCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
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
}
