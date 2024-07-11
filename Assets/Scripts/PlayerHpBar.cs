using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    private Slider _hpBar;

    // Start is called before the first frame update
    void Start()
    {
        _hpBar = GameObject.Find("PlayerHpBar").GetComponent<Slider>();
        UpdateHp();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateHp()
    {
        _hpBar.value = (float)_player.HitPoint/(float)_player.MaxHitPoint;
    }
}