using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIObject_Seller_HitPoint : UIObject
{
    [SerializeField]
    private int _buyCount;

    public int BuyCount
    {
        get {return _buyCount;}
        set
        {
            _buyCount = value;
            _UpdateMoneyText();

            if(_buyCount >= 11)
            {
                IsSoldOut = true;
            }
        }
    }

    [SerializeField]
    private Player _player;

    [SerializeField]
    private TMP_Text _moneyText;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private GameObject _soldOutPanel;

    private bool _isSoldOut;
    
    public bool IsSoldOut
    {
        get {return _isSoldOut;}
        set
        {
            _isSoldOut = value;
            _soldOutPanel.SetActive(_isSoldOut);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _UpdateMoneyText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void InvokeUIAction()
    {
        if(IsSoldOut)
        {
            Debug.Log("Sold out!");
            PlaySound("CannotBuy");
            return;
        }

        if(_player.Money >= BuyCount)
        {
            _player.UseMoney(BuyCount);
            _player.EnhanceHitPoint(10);
            _animator.SetTrigger("Buy");
            Test();
            BuyCount++;
        }
        else
        {
            //購入不可の時の処理(お金UIを揺らす、効果音を鳴らす等)
            PlaySound("CannotBuy");
            Debug.Log("Player does not have enough money!");
        }
    }

    private void Test()
    {
        Debug.Log($"Use {_buyCount}, PlayerMoney: {_player.Money}, PlayerHitPoint: {_player.HitPoint}, PlayerMaxHitPoint: {_player.MaxHitPoint}");
    }

    private void _UpdateMoneyText()
    {
        if(BuyCount <= 10)
        {
            _moneyText.SetText($"{BuyCount}");
        }
        else
        {
            _moneyText.SetText("-");
        }
    }
}
