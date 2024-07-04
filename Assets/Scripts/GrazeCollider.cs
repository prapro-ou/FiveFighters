using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrazeCollider : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _player.GrazeCounter += 1;
    }
}
