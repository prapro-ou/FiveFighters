using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObject_Seller_Graze : UIObject
{
    private int _buyCount = 0;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private GrazeCollider _grazeCollider;

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
        _grazeCollider.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
        Test();
        _buyCount++;
    }

    private void Test()
    {
        Debug.Log("Use " + _buyCount + " money");
        Debug.Log("PlayerMoney:" + _player.Money);

    }
}
