using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObject_Seller_Power : UIObject
{
    private int _buyCount = 1;

    [SerializeField]
    private Player _player;

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
        _player.UseMoney(_buyCount);
        _player.EnhancePower(0.1f);
        Test();
        _buyCount++;
    }

    private void Test()
    {
        Debug.Log("Use:" + _buyCount + " money");
        Debug.Log("PlayerMoney:" + _player.Money);
        Debug.Log("PlayerPowerMultiplier:" +  _player.PowerMultiplier);
    }
}
