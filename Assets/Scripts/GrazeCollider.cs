using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrazeCollider : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    private int _grazeCount;

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
}
