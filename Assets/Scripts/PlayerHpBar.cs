using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private Slider _hpBarSlider;

    // Start is called before the first frame update
    void Start()
    {
        UpdateHp();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateHp()
    {
        if(_player == null)
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
        }
        
        _hpBarSlider.value = (float)(_player.HitPoint)/(float)(_player.MaxHitPoint);
    }
}