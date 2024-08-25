using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpeedChangeBullet : EnemyBullet
{
    [SerializeField]
    public float _originalSpeed;

    private bool _isSpeedChanged = false;

    public bool IsSpeedChanged
    {
        get {return _isSpeedChanged;}
        set {_isSpeedChanged = value;}
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
