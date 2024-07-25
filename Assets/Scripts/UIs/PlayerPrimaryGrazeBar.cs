using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPrimaryGrazeBar : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private Slider _primaryGrazeBarSlider;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePrimaryGrazeCount()
    {
        _primaryGrazeBarSlider.value = (float)_player.PrimaryGrazeCount / (float)_player.MyShape.PrimaryAttackCost;
    }
}
