using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    private Slider hpBar;

    private int clk = 0;

    // Start is called before the first frame update
    void Start()
    {
        hpBar = GameObject.Find("PlayerHpBar").GetComponent<Slider>();
        UpdateHp();
    }

    // Update is called once per frame
    void Update()
    {
        DamageTest();
    }

    private void UpdateHp()
    {
        hpBar.value = (float)_player._hitPoint/(float)100;
    }

    private void DamageTest()
    {
        if(clk > 70)
        {
            Debug.Log("damage");
            hpBar.value -= 0.05f;
            clk = 0;
        }
        else
            clk += 1;
    }
}